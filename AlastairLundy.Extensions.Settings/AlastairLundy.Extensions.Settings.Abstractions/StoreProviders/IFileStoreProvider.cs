/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.ComponentModel;

namespace AlastairLundy.Extensions.Settings.Abstractions.StoreProviders;

public interface IFileStoreProvider<TValue> : IStoreProvider<TValue>
{
    public FileStoreConfiguration FileConfiguration { get; }
    
    /// <summary>
    /// A type converter provided by an implementing class that converts the TValue type to one that is supported by the File store.
    /// </summary>
    public TypeConverter Converter { get; }
}