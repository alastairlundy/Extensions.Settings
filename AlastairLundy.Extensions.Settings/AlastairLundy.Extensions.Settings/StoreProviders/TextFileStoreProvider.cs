/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlastairLundy.Extensions.Settings.Internal;
using AlastairLundy.Extensions.Settings.StoreProviders.Abstractions;

namespace AlastairLundy.Extensions.Settings.StoreProviders;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class TextFileStoreProvider<TValue> : IFileStoreProvider<TValue>
{
    private readonly char _keyValueSeparator;
    
    public TypeConverter Converter { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileConfiguration"></param>
    /// <param name="typeConverter">A type converter that converts the TValue type to strings and vice versa.</param>
    /// <param name="keyValueSeparator"></param>
    public TextFileStoreProvider(FileStoreConfiguration fileConfiguration, TypeConverter typeConverter, char keyValueSeparator = '=')
    {
        Converter = typeConverter;
        
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

        if (Converter.CanConvertFrom(typeof(string)))
        {
            // ReSharper disable once NullableWarningSuppressionIsUsed
            TValue value = (TValue)Converter.ConvertFromString(parts[0])!;
            
            return value;
        }
        else
        {
            throw new ArgumentException(Resources.Exceptions_Conversions_CannotConvertFromString.Replace("{x}", nameof(TValue)));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public async Task SetValueAsync(string key, TValue value)
    {
        await WriteToFileAsync(key, value);
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
                
                if(Converter.CanConvertFrom(typeof(string)) && Converter.CanConvertTo(typeof(TValue)))
                {
                    // ReSharper disable once NullableWarningSuppressionIsUsed
                    TValue value = (TValue)Converter.ConvertFromString(parts[1])!;
                
                    output.Add(new KeyValuePair<string, TValue>(parts[0], value));
                }
                else
                {
                    throw new ArgumentException(Resources.Exceptions_Conversions_CannotConvertFromString.Replace("{x}", nameof(TValue)));
                }
            }
        }
        
        return output;
    }

    protected async Task WriteToFileAsync(string key, TValue value)
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
                    if (Converter.CanConvertTo(typeof(TValue)) && Converter.CanConvertFrom(typeof(string)))
                    {
                        // ReSharper disable once NullableWarningSuppressionIsUsed
                        string val = Converter.ConvertToString(parts[1])!;
                    
                        stringBuilder.Append(val);
                    }
                    else
                    {
                        throw new ArgumentException(Resources.Exceptions_Conversions_CannotConvertToString.Replace("{x}", nameof(TValue)));
                    }
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
       

#if NET6_0_OR_GREATER
        await File.WriteAllTextAsync(FileConfiguration.FilePath, stringBuilder.ToString());
#else
        await FilePolyfill.WriteAllTextAsync(FileConfiguration.FilePath, stringBuilder.ToString());
#endif
    }

    public FileStoreConfiguration FileConfiguration { get; }
}