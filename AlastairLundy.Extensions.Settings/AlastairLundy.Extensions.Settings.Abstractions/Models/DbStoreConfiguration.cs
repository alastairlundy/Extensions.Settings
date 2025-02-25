/*
    AlastairLundy.Extensions.Settings    
    Copyright (c) Alastair Lundy 2025
 
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable ClassNeverInstantiated.Global
namespace AlastairLundy.Extensions.Settings.Abstractions;

/// <summary>
/// 
/// </summary>
/// <param name="connectionString"></param>
/// <param name="tableName"></param>
public class DbStoreConfiguration(string connectionString, string? tableName = null)
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; protected set; } = connectionString;

    /// <summary>
    /// 
    /// </summary>
    public string? TableName { get; protected set; } = tableName ?? string.Empty;
}