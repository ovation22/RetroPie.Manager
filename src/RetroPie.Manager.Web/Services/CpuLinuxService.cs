using System.Diagnostics;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class CpuLinuxService : ICpuService
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
        var output = RunCommand("cat", "/proc/stat");
        var values = output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var user = ulong.Parse(values[1]);
        var nice = ulong.Parse(values[2]);
        var system = ulong.Parse(values[3]);
        var idle = ulong.Parse(values[4]);

        var total = user + nice + system + idle;
        var busy = total - idle;

        return (float)busy / total * 100 * 100;
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
        var output = RunCommand("cat", "/proc/stat");
        var line = output.Split('\n')
            .FirstOrDefault(l => l.StartsWith($"cpu{core} "));

        if (line == null)
        {
            Console.WriteLine($"Error: CPU core {core} not found in /proc/stat");
            return 0;
        }

        var values = output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        ulong user = ulong.Parse(values[1]);
        ulong nice = ulong.Parse(values[2]);
        ulong system = ulong.Parse(values[3]);
        ulong idle = ulong.Parse(values[4]);

        ulong total = user + nice + system + idle;
        ulong busy = total - idle;

        return (float)busy / total * 100 * 100;
    }

    private static string RunCommand(string command, params string[] arguments)
    {
        using var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = string.Join(" ", arguments);
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        var result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result;
    }
}
