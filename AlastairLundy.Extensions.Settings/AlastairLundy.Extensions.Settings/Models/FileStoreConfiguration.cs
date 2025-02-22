using System.IO;

namespace AlastairLundy.Extensions.Settings;

public class FileStoreConfiguration
{
    public string FilePath { get; }
    public string FileExtension { get; }
    
    public bool RequiresAdminAccess { get; }
    

    public FileStoreConfiguration(string filePath, bool requiresAdminAccess)
    {
        FilePath = filePath;
        FileExtension = Path.GetExtension(filePath);
        RequiresAdminAccess = requiresAdminAccess;
    }
}