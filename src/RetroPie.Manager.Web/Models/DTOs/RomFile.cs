namespace RetroPie.Manager.Web.Models.DTOs;

public class RomFile
{
    public string FileName { get; set; } = default!;
    public long FileSize { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastAccessTime { get; set; }
    public DateTime LastWriteTime { get; set; }
}
