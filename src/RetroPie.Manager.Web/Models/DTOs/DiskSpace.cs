namespace RetroPie.Manager.Web.Models.DTOs;

public class DiskSpace
{
    public float OverallUsagePercent { get; set; }
    public float FreeSpace { get; set; }
    public float AvailableSpacePercent { get; set; }
    public float TotalSpace { get; set; }
}
