using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class MemoryUsageLinuxService : IMemoryUsageService
{
    public MemoryUsage GetMemoryUsage()
    {
        var memInfoPath = "/proc/meminfo";

        try
        {
            var lines = File.ReadAllLines(memInfoPath);

            ulong totalMemoryKb = 0;
            ulong freeMemoryKb = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("MemTotal:"))
                {
                    totalMemoryKb = ParseMemoryValue(line);
                }
                else if (line.StartsWith("MemFree:"))
                {
                    freeMemoryKb = ParseMemoryValue(line);
                }
            }

            var totalMemoryGb = totalMemoryKb / (1024f * 1024f);
            var freeMemoryGb = freeMemoryKb / (1024f * 1024f);
            var availableMemoryPercent = (float)(totalMemoryKb - freeMemoryKb) / totalMemoryKb * 100;

            return new MemoryUsage
            {
                OverallUsagePercent = 100 - (freeMemoryGb / totalMemoryGb * 100),
                FreeMemory = freeMemoryGb,
                AvailableMemoryPercent = availableMemoryPercent,
                TotalMemory = totalMemoryGb
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    private static ulong ParseMemoryValue(string line)
    {
        var valueString = line.Split(':')[1].Trim().Split(' ')[0];
        return ulong.Parse(valueString);
    }
}
