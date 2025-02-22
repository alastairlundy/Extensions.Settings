/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Settings.Stores.Abstractions;
// ReSharper disable RedundantExtendsListEntry

namespace AlastairLundy.Extensions.Settings.Stores;

/// <summary>
/// A text file based settings store with caching.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class CachedTextFileSettingsStore<TValue> : ICachedSettingsStore<TValue>, IFileSettingsStore<TValue>
{
    private readonly char _keyValueSeparator;
    public Dictionary<string, TValue> Cache { get; protected set; }
    public DateTime CacheExpiration { get; protected set; }
    public TimeSpan CacheLifetime { get; protected set; }

    public Func<string, TValue> ToTValueConverter { get; }
    public Func<TValue, string> ToStringConverter { get; }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="toTValueFunc"></param>
    /// <param name="toStringFunc"></param>
    /// <param name="keyValueSeparator"></param>
    public CachedTextFileSettingsStore(string filePath, Func<string, TValue> toTValueFunc, Func<TValue, string> toStringFunc, char keyValueSeparator = '=')
    {
        FilePath = filePath;
        FileName = Path.GetFileName(filePath);
        FileExtension = Path.GetExtension(filePath);
        
        ToTValueConverter = toTValueFunc;
        ToStringConverter = toStringFunc;
        
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
        string[] lines = await File.ReadAllLinesAsync(FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FilePath);
#endif
        
        foreach (string line in lines)
        {
            string[] parts = line.Split(_keyValueSeparator);
                
            TValue value = ToTValueConverter(parts[1]);
                
            Cache[key: parts[0]] = value;
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

    private async Task<TValue> LoadFromFileAsync(string key)
    {
#if NET6_0_OR_GREATER
                string[] lines = await File.ReadAllLinesAsync(FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FilePath);
#endif
                
        string line = lines.First(x => x.Contains(key));

        string[] parts = line.Split('=');
                
        TValue value = ToTValueConverter(parts[1]);
                
        return value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<TValue>? GetValueAsync(string key)
    {
        if (Cache.Count == 0)
        {
            await UpdateCacheAsync();
        }

        if (Cache.ContainsKey(key) && CacheExpiration < DateTime.Now)
        {
            try
            {
                return Cache[key];
            }
            catch
            {
               return await LoadFromFileAsync(key);
            }
        }
        else if (CacheExpiration >= DateTime.Now)
        {
            await UpdateCacheAsync(CacheLifetime);
            
            return await LoadFromFileAsync(key);
        }
        else
        {
           return await LoadFromFileAsync(key);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public async Task SetValueAsync(string key, TValue value)
    {
        Cache[key] = value;
        
#if NET6_0_OR_GREATER
        string[] lines = await File.ReadAllLinesAsync(FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FilePath);
#endif

        StringBuilder stringBuilder = new StringBuilder();
        
        for (int index = 0; index < lines.Length; index++)
        {
            string line = lines[index];

            if (line.Contains(_keyValueSeparator))
            {
                string[] parts = line.Split(_keyValueSeparator);

                stringBuilder.Append(parts[0]);
                stringBuilder.Append(_keyValueSeparator);
                
                if (parts[0].Equals(key))
                {
                    stringBuilder.Append(ToStringConverter(value));
                }

                stringBuilder.AppendLine();
            }
            else
            {
                stringBuilder.Append(line);
                stringBuilder.AppendLine();
            }
        }
        
        File.WriteAllText(FilePath, stringBuilder.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetSettingsKeysAsync()
    {
        IEnumerable<KeyValuePair<string, TValue>> pairs = await GetSettingsAsync();
        
        return pairs.Select(x => x.Key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<KeyValuePair<string, TValue>>> GetSettingsAsync()
    {
#if NET6_0_OR_GREATER
        string[] lines = await File.ReadAllLinesAsync(FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FilePath);
#endif

        List<KeyValuePair<string, TValue>> output = new List<KeyValuePair<string, TValue>>();
        
        foreach (string line in lines)
        {
            if (line.Contains('='))
            {
                string[] parts = line.Split('=');
                
                string key = parts[0];

                TValue value = ToTValueConverter(parts[1]);
                
                output.Add(new KeyValuePair<string, TValue>(key, value));
            }    
        }
                
        return output;
    }

    public string FilePath { get; }
    public string FileName { get; }
    public string FileExtension { get; }
    
}