using System.Diagnostics;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class CpuWindowsService : ICpuService
{
    public Task<CpuStats> GetCpuStats()
    {
        var overallCpuUsage = Task.Run(GetCpuUsage);
        var coreUsages = Task.Run(GetCpuCoreUsage);

        Task.WaitAll(overallCpuUsage, coreUsages);

        return Task.FromResult(new CpuStats
        {
            OverallCpuUsage = overallCpuUsage.Result,
            CoreUsages = coreUsages.Result
        });
    }

    private static float GetCpuUsage()
    {
        using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue(); // Call this to get initial value
        Thread.Sleep(1000);     // Sleep for 1 second
        return cpuCounter.NextValue();
    }

    private static (byte core, float usage)[] GetCpuCoreUsage()
    {
        var coreCount = Environment.ProcessorCount;
        var usageValues = new (byte core, float usage)[coreCount];

        Parallel.For((long)0, coreCount, core =>
        {
            usageValues[core] = ((byte)core, GetCoreUsage(core));
        });

        return usageValues;
    }

    private static float GetCoreUsage(long core)
    {
        using var coreCounter = new PerformanceCounter("Processor", "% Processor Time", $"{core}");
        coreCounter.NextValue(); // Call this to get initial value
        Thread.Sleep(1000);     // Sleep for 1 second
        return coreCounter.NextValue();
    }
}
