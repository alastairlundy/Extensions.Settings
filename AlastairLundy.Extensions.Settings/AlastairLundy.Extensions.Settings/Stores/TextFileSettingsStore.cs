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

namespace AlastairLundy.Extensions.Settings.Stores;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class TextFileSettingsStore<TValue> : IFileSettingsStore<TValue>
{
    private readonly char _keyValueSeparator;
    
    public Func<string, TValue> ToTValueConverter { get; }
    public Func<TValue, string> ToStringConverter { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileConfiguration"></param>
    /// <param name="toTValueConverter"></param>
    /// <param name="stringToStringConverter"></param>
    /// <param name="keyValueSeparator"></param>
    public TextFileSettingsStore(FileStoreConfiguration fileConfiguration, Func<string, TValue> toTValueConverter,
        Func<TValue, string> stringToStringConverter, char keyValueSeparator = '=')
    {
        ToTValueConverter = toTValueConverter;
        ToStringConverter = stringToStringConverter;
        
        FileConfiguration = fileConfiguration;
        _keyValueSeparator = keyValueSeparator;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<TValue> GetValueAsync(string key)
    {
#if NET6_0_OR_GREATER
        string[] lines = await File.ReadAllLinesAsync(FileConfiguration.FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FileConfiguration.FilePath);
#endif
        
        string line = lines.First(x => x.Contains(key) && x.Contains(_keyValueSeparator));
        
        string[] parts = line.Split(_keyValueSeparator);
                
        TValue value = ToTValueConverter(parts[1]);

        return value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public async Task SetValueAsync(string key, TValue value)
    {
#if NET6_0_OR_GREATER
                string[] lines = await File.ReadAllLinesAsync(FileConfiguration.FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FileConfiguration.FilePath);
#endif
        
        StringBuilder stringBuilder = new StringBuilder();
        
        foreach (string line in lines)
        {
            if (line.Contains(_keyValueSeparator))
            {
                string[] parts = line.Split(_keyValueSeparator);

                stringBuilder.Append(parts[0]);
                stringBuilder.Append(_keyValueSeparator);

                if (parts[0].Equals(key))
                {
                    string val = ToStringConverter(value);
                    
                    stringBuilder.Append(val);
                }
                else
                {
                       stringBuilder.Append(parts[1]);
                }
            }
            else
            {
                stringBuilder.AppendLine(line);
            }
        }
        
        File.WriteAllText(FileConfiguration.FilePath, stringBuilder.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetSettingsKeysAsync()
    {
        IEnumerable<KeyValuePair<string, TValue>> values = await GetSettingsAsync();

        return values.Select(x => x.Key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<KeyValuePair<string, TValue>>> GetSettingsAsync()
    {
#if NET6_0_OR_GREATER
                string[] lines = await File.ReadAllLinesAsync(FileConfiguration.FilePath);
#else
        string[] lines = await FilePolyfill.ReadAllLinesAsync(FileConfiguration.FilePath);
#endif

        List<KeyValuePair<string, TValue>> output = new();
        
        foreach (string line in lines)
        {
            if (line.Contains(_keyValueSeparator))
            {
                string[] parts = line.Split(_keyValueSeparator);
                
                TValue value = ToTValueConverter(parts[1]);
                
                output.Add(new KeyValuePair<string, TValue>(parts[0], value));
            }
        }
        
        return output;
    }


    public FileStoreConfiguration FileConfiguration { get; }
}