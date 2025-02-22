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
using System.Threading.Tasks;

using AlastairLundy.Extensions.Settings.Stores.Abstractions;

namespace AlastairLundy.Extensions.Settings.Json.Stores;

public class JsonFileSettingsStore<TValue> : IFileSettingsStore<TValue>
{
    public Func<string, TValue> ToTValueConverter { get; }
    public Func<TValue, string> ToStringConverter { get; }

    public JsonFileSettingsStore(FileStoreConfiguration fileConfiguration, Func<string, TValue> toTValueConverter, Func<TValue, string> stringToStringConverter)
    {
        ToTValueConverter = toTValueConverter;
        ToStringConverter = stringToStringConverter;
        FileConfiguration = fileConfiguration;
    }
    
    public async Task<TValue> GetValueAsync(string key)
    {
       
    }

    public async Task SetValueAsync(string key, TValue value)
    {
       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetSettingsKeysAsync()
    {
        IEnumerable<KeyValuePair<string, TValue>> settings = await GetSettingsAsync();

        return settings.Select(x => x.Key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<KeyValuePair<string, TValue>>> GetSettingsAsync()
    {
        
    }

    public FileStoreConfiguration FileConfiguration { get; }
}