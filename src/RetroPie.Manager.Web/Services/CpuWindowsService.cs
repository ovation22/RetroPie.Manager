using System.Diagnostics;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class CpuWindowsService : ICpuService
{
    public async Task<CpuStats> GetCpuStats()
    {
        var overallCpuUsage = GetCpuUsage();
        var coreUsages = GetCoreUsages();

        await Task.WhenAll(overallCpuUsage, coreUsages);

        return new CpuStats
        {
            OverallCpuUsage = overallCpuUsage.Result,
            CoreUsages = coreUsages.Result
        };
    }

    private static async Task<float> GetCpuUsage()
    {
        using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue(); // Call this to get initial value
        await Task.Delay(1000); // Asynchronously sleep for 1 second
        return cpuCounter.NextValue();
    }

    private static async Task<(byte core, float usage)[]> GetCoreUsages()
    {
        var coreCount = Environment.ProcessorCount;
        var usageValues = new (byte core, float usage)[coreCount];
        var tasks = new Task<float>[coreCount];

        for (int i = 0; i < coreCount; i++)
        {
            tasks[i] = GetCoreUsage(i);
        }

        await Task.WhenAll(tasks);

        for (int core = 0; core < coreCount; core++)
        {
            usageValues[core] = ((byte)core, tasks[core].Result);
        }
        
        return usageValues;
    }

    private static async Task<float> GetCoreUsage(long core)
    {
        using var coreCounter = new PerformanceCounter("Processor", "% Processor Time", $"{core}");
        coreCounter.NextValue(); // Call this to get initial value
        await Task.Delay(1000);    // Sleep for 1 second
        return coreCounter.NextValue();
    }
}
