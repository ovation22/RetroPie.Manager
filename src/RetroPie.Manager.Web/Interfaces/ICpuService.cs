using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Interfaces;

public interface ICpuService
{
    Task<CpuStats> GetCpuStats();
}
