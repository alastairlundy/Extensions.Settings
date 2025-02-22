/*
    AlastairLundy.Extensions.Configuration    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using Microsoft.Extensions.Configuration;

// ReSharper disable RedundantIfElseBlock

namespace AlastairLundy.Extensions.Configuration.Binder;

public static class GetValueOrDefaultBinderExtensions
{
    /// <summary>
    /// Extracts the value with the specified key if found, and uses a specified fallback default value if it isn't.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="key">The key of the configuration section's value to convert.</param>
    /// <param name="defaultValue">The fallback value to use in case the specified key or the associated value cannot be found.</param>
    /// <typeparam name="T">The type to convert the value to.</typeparam>
    /// <returns>The converted value if found; the specified defaultValue otherwise.</returns>
    public static T GetValueOrDefault<T>(this IConfiguration configuration, string key, T defaultValue)
    {
        try
        {
            T? value = configuration.GetValue<T>(key);

            if (value is null)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }
        catch
        {
            return defaultValue;
        }
    }
}