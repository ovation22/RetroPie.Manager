namespace RetroPie.Manager.Web.Models.DTOs;

public class CpuStats
{
    public float OverallCpuUsage { get; set; }
    public (byte core, float usage)[] CoreUsages { get; set; } = default!;
}
