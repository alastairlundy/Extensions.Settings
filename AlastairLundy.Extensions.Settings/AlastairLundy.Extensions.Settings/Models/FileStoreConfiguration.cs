/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.IO;
// ReSharper disable ClassNeverInstantiated.Global

namespace AlastairLundy.Extensions.Settings;

/// <summary>
/// 
/// </summary>
/// <param name="filePath"></param>
/// <param name="requiresAdminAccess"></param>
public class FileStoreConfiguration(string filePath, bool requiresAdminAccess)
{
    /// <summary>
    /// 
    /// </summary>
    public string FilePath { get; } = filePath;
    
    /// <summary>
    /// 
    /// </summary>
    public string FileExtension { get; } = Path.GetExtension(filePath);

    /// <summary>
    /// 
    /// </summary>
    public bool RequiresAdminAccess { get; } = requiresAdminAccess;
}