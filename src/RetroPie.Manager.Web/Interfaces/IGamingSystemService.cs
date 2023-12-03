using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Interfaces;

public interface IGamingSystemService
{
    Task<GamingSystem?> GetAsync(string name);
    Task<GamingSystem[]?> GetAllAsync();
}