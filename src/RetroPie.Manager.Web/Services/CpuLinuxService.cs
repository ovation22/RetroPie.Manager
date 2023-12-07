using System.Diagnostics;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class CpuLinuxService : ICpuService
{
    public async Task<CpuStats> GetCpuStats()
    {
        var output = await RunCommandAsync("cat", "/proc/stat");
        
        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var cpuStats = new CpuStats
        {
            OverallCpuUsage = GetOverallCpuUsage(lines),
            CoreUsages = GetCoreUsages(lines)
        };

        return cpuStats;
    }

    private static float GetOverallCpuUsage(IEnumerable<string> lines)
    {
        float overallCpuUsage = 0;
        var overallCpuLine = lines.FirstOrDefault(l => l.StartsWith("cpu "));
        
        if (overallCpuLine == null)
        {
            return overallCpuUsage;
        }

        var overallCpuValues = overallCpuLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (overallCpuValues.Length < 5)
        {
            return overallCpuUsage;
        }

        var user = ulong.Parse(overallCpuValues[1]);
        var nice = ulong.Parse(overallCpuValues[2]);
        var system = ulong.Parse(overallCpuValues[3]);
        var idle = ulong.Parse(overallCpuValues[4]);

        var total = user + nice + system + idle;
        var busy = total - idle;

        overallCpuUsage = (float)busy / total * 100 * 100;

        return overallCpuUsage;
    }

    private static (byte core, float usage)[] GetCoreUsages(IEnumerable<string> lines)
    {
        return lines.Skip(1).Where(l => l.StartsWith("cpu"))
            .Select(line =>
            {
                var coreValues = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (coreValues.Length < 5)
                {
                    Console.WriteLine($"Error: Unable to retrieve CPU core usage for {line}");
                    return (core: (byte)0, usage: 0);
                }

                var coreIndex = byte.Parse(coreValues[0][3..]);
                var user = ulong.Parse(coreValues[1]);
                var nice = ulong.Parse(coreValues[2]);
                var system = ulong.Parse(coreValues[3]);
                var idle = ulong.Parse(coreValues[4]);

                var total = user + nice + system + idle;
                var busy = total - idle;

                var usage = (float)busy / total * 100 * 100;

                return (core: coreIndex, usage);
            })
            .ToArray();
    }

    private static async Task<string> RunCommandAsync(string command, params string[] arguments)
    {
        using var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = string.Join(" ", arguments);
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        var result = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return result;
    }
}
