﻿/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using AlastairLundy.Extensions.Settings.Internal;
using AlastairLundy.Extensions.Settings.StoreProviders.Abstractions;

// ReSharper disable RedundantExtendsListEntry
// ReSharper disable NullableWarningSuppressionIsUsed

namespace AlastairLundy.Extensions.Settings.StoreProviders;

/// <summary>
/// A text file based settings store with caching.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class CachedTextFileStoreProvider<TValue> : TextFileStoreProvider<TValue>, 
    ICachedStoreProvider<TValue>, IFileStoreProvider<TValue>
{
    private readonly char _keyValueSeparator;
    
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, TValue> Cache { get; protected set; }
    
    /// <summary>
    /// 
    /// </summary>
    public DateTime CacheExpiration { get; protected set; }
    
    /// <summary>
    /// 
    /// </summary>
    public TimeSpan CacheLifetime { get; protected set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="expiration"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetCacheExpiration(DateTime expiration)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expiration"></param>
    public void SetCacheLifetime(TimeSpan expiration)
    {
        if (expiration < TimeSpan.FromDays(365.0))
        {
            CacheLifetime = expiration;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileConfiguration"></param>
    /// <param name="typeConverter"></param>
    /// <param name="keyValueSeparator"></param>
    public CachedTextFileStoreProvider(FileStoreConfiguration fileConfiguration,
        TypeConverter typeConverter,
        char keyValueSeparator = '=') : base(fileConfiguration,
        typeConverter,
        keyValueSeparator)
    {
        Cache = new Dictionary<string, TValue>();
        CacheLifetime = TimeSpan.FromHours(1);
        CacheExpiration = DateTime.Now.Add(CacheLifetime);
        _keyValueSeparator = keyValueSeparator;
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    public async Task UpdateCacheAsync()
    {
        await UpdateCacheAsync(CacheLifetime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expiry"></param>
    public async Task UpdateCacheAsync(TimeSpan expiry)
    {
        ClearCache();
        
#if NET6_0_OR_GREATER
        string[] lines = await File.ReadAllLinesAsync(FileConfiguration.FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FileConfiguration.FilePath);
#endif
        
        foreach (string line in lines)
        {
            string[] parts = line.Split(_keyValueSeparator);

            if (Converter.CanConvertTo(typeof(TValue)) && Converter.CanConvertFrom(typeof(string)))
            {
                TValue value = (TValue)Converter.ConvertFromString(parts[1])!;
                
                Cache[key: parts[0]] = value;
            }
            else
            {
                throw new ArgumentException(
                    Resources.Exceptions_Conversions_CannotConvertFromString.Replace("{x}", nameof(TValue)));
            }
        }
        
        CacheLifetime = expiry;
        CacheExpiration = DateTime.Now.Add(CacheLifetime);
    }

    /// <summary>
    /// Clears the Dictionary cache.
    /// </summary>
    public void ClearCache()
    {
        Cache.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public new async Task<TValue> GetValueAsync(string key)
    {
        if (Cache.Count == 0)
        {
            await UpdateCacheAsync(CacheLifetime);
        }

        if (Cache.ContainsKey(key) && CacheExpiration < DateTime.Now)
        {
            try
            {
                return Cache[key];
            }
            catch
            {
               return await base.GetValueAsync(key);
            }
        }
        else if (CacheExpiration >= DateTime.Now)
        {
            await UpdateCacheAsync(CacheLifetime);
            
            return await base.GetValueAsync(key);
        }
        else
        {
           return await base.GetValueAsync(key);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public new async Task SetValueAsync(string key, TValue value)
    {
        Cache[key] = value;
        
        await WriteToFileAsync(key, value);
    }
    
}