namespace RetroPie.Manager.Web.Models.DTOs;

public class RetroPieManagerSettings
{
    public string RomsPath { get; set; } = default!;
    public string BiosPath { get; set; } = default!;
    public string ConfigsPath { get; set; } = default!;
    public string EmulationStationPath { get; set; } = default!;
    public string? GameLogs { get; set; } = default!;
}
