/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading.Tasks;

using AlastairLundy.Extensions.Settings.Abstractions.StoreProviders;

namespace AlastairLundy.Extensions.Settings.Abstractions.Extensions;

public static class StoreProviderGetValueOrDefaultExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static async Task<TValue> GetValueOrDefault<TValue>(this IStoreProvider<TValue> provider, string key,
        TValue defaultValue)
    {
        try
        {
           return await provider.GetValueAsync(key);
        }
        catch
        {
            return defaultValue;
        }
    }
}