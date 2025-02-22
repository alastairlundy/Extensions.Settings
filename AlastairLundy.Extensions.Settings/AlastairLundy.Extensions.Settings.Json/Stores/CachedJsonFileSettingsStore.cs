/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Settings.Stores.Abstractions;

namespace AlastairLundy.Extensions.Settings.Json.Stores;

public class CachedJsonFileSettingsStore<TValue> : JsonFileSettingsStore<TValue>, ICachedSettingsStore<TValue>
{
    public CachedJsonFileSettingsStore(string filePath, Func<string, TValue> toTValueConverter, Func<TValue, string> stringToStringConverter) : base(filePath, toTValueConverter, stringToStringConverter)
    {
        Cache = new Dictionary<string, TValue>();
        CacheLifetime = TimeSpan.FromHours(1);
        CacheExpiration = DateTime.Now.Add(CacheLifetime);
    }

    public Dictionary<string, TValue> Cache { get; }
    public DateTime CacheExpiration { get; }
    public TimeSpan CacheLifetime { get; }


    public new async Task<TValue> GetValueAsync(string key)
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

    public new Task SetValueAsync(string key, TValue value)
    {
        
    }

    public async Task UpdateCacheAsync()
    {
       await UpdateCacheAsync(CacheLifetime);
    }

    public async Task UpdateCacheAsync(TimeSpan expiry)
    {
        
    }

    public void ClearCache()
    {
        Cache.Clear();
    }
}