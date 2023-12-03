using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;
using System.Text.Json;

namespace RetroPie.Manager.Web.Services;

public class GamingSystemService : IGamingSystemService
{
    public async Task<GamingSystem?> GetAsync(string name)
    {
        var gamingSystems = await GetGamingSystemsAsync();

        return gamingSystems.FirstOrDefault(x => x.Name == name);
    }

    public async Task<GamingSystem[]?> GetAllAsync()
    {
        var gamingSystems = await GetGamingSystemsAsync();

        return (gamingSystems ?? Array.Empty<GamingSystem>()).ToArray();
    }

    private static async Task<GamingSystem[]?> GetGamingSystemsAsync()
    {
        var gamingSystems = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "Config/systems.json"));

        return JsonSerializer.Deserialize<GamingSystem[]>(gamingSystems,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }
}