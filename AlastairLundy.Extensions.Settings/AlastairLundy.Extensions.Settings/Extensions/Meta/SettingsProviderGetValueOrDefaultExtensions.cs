using System.Threading.Tasks;
using AlastairLundy.Extensions.Settings.Stores.Abstractions;

namespace AlastairLundy.Extensions.Settings;

public static class SettingsStoreGetValueOrDefaultExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static async Task<TValue> GetValueOrDefault<TValue>(this ISettingsStore<TValue> provider, string key,
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