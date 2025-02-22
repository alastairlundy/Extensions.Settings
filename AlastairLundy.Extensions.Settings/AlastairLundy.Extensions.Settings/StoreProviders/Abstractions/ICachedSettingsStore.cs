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

namespace AlastairLundy.Extensions.Settings.StoreProviders.Abstractions;

/// <summary>
/// A settings store interface that uses a Dictionary as a Cache.
/// </summary>
/// <typeparam name="TValue">The type of Value stored in the Settings Store.</typeparam>
public interface ICachedStoreProvider<TValue> : IStoreProvider<TValue>
{
    /// <summary>
    /// A Dictionary that can be used to store settings values in memory, and act as a cache to reduce the need to directly read from the Store's source each time.
    /// </summary>
    /// <remarks>Implementers may wish to implement reading from the Store's source as a fallback, in case the cache does not contain a Settings key asked for.</remarks>
    public Dictionary<string, TValue> Cache { get; }
    
    /// <summary>
    /// The DateTime representing when the Cache will expire, and need clearing and reloading.
    /// </summary>
    public DateTime CacheExpiration { get; }
    
    /// <summary>
    /// How long the Dictionary cache should keep being used before being updated.
    /// </summary>
    /// <remarks>Implementers should set the default value to 1 Hour.</remarks>
    TimeSpan CacheLifetime { get; }
    
    /// <summary>
    /// Clears the Dictionary and then loads the Dictionary cache with the Settings from the Store's source.
    /// </summary>
    /// <remarks><para>The dictionary cache should have a default expiry of 1 Hour.</para>
    /// <para>Implementers must reset the Cache Expiration upon updating the cache.</para>
    /// Implementers should ensure that the cache is updated the next time GetValue is called after Cache Expiration.</remarks>
    /// <returns>The completed task.</returns>
    Task UpdateCacheAsync();
    
    /// <summary>
    /// Clears the Dictionary and then loads the Dictionary cache with the Settings from the Store's source.
    /// </summary>
    /// <param name="expiry">How long the Dictionary cache should remain valid before expiring, and requiring an update.</param>
    /// <remarks>
    /// <para>Implementers must reset the Cache Expiration upon updating the cache.</para>
    /// Implementers should ensure that the cache is updated the next time GetValue is called after Cache Expiration.</remarks>
    /// <returns>The completed task.</returns>
    Task UpdateCacheAsync(TimeSpan expiry);
    
    /// <summary>
    /// Clears the Dictionary cache.
    /// </summary>
    void ClearCache();
}