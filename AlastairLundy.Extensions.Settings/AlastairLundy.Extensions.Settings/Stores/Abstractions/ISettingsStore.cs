﻿/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlastairLundy.Extensions.Settings.Stores.Abstractions;

public interface ISettingsStore<TValue>
{
    /// <summary>
    /// A func, provided by the User of the implementing interface, that parses a string into a TValue object.
    /// </summary>
    public Func<string, TValue> ToTValueConverter { get; }
    
    /// <summary>
    /// A func, provided by the User of the implementing interface, that converts a TValue object to a string.
    /// </summary>
    public Func<TValue, string> ToStringConverter { get; }
    
    /// <summary>
    /// Reads the value associated with a key from a Settings Store.
    /// </summary>
    /// <param name="key">The key to get the value of.</param>
    /// <returns>The value associated with the key if found; null otherwise.</returns>
    Task<TValue> GetValueAsync(string key);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    Task SetValueAsync(string key, TValue value);
    
    /// <summary>
    /// Gets an IEnumerable of Settings Keys.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string>> GetSettingsKeysAsync();
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<KeyValuePair<string, TValue>>> GetSettingsAsync();
}