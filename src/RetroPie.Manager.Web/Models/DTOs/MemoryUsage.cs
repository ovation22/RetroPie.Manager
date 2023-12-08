namespace RetroPie.Manager.Web.Models.DTOs;

public class MemoryUsage
{
    public float OverallUsagePercent { get; set; }
    public float FreeMemory { get; set; }
    public float AvailableMemoryPercent { get; set; }
    public float TotalMemory { get; set; }
}
