﻿/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace AlastairLundy.Extensions.Settings.Stores.Abstractions;

public interface IFileSettingsStore<TValue> : ISettingsStore<TValue>
{
    public FileStoreConfiguration FileConfiguration { get; }
}