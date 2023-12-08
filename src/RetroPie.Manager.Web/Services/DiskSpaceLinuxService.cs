using System.Diagnostics;
using System.Globalization;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class DiskSpaceLinuxService : IDiskSpaceService
{
    public DiskSpace GetDiskSpace()
    {
        var output = RunCommand("df", "-k");

        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2)
        {
            throw new InvalidOperationException("Unexpected output format from df command.");
        }

        var columns = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (columns.Length < 6)
        {
            throw new InvalidOperationException("Unexpected output format from df command.");
        }

        float totalSpaceKb = float.Parse(columns[1], CultureInfo.InvariantCulture);
        float availableSpaceKb = float.Parse(columns[3], CultureInfo.InvariantCulture);
        float overallUsagePercent = float.Parse(columns[4].Replace("%", ""), CultureInfo.InvariantCulture);
        float availableSpacePercent = 100f - overallUsagePercent;

        float totalSpaceGb = totalSpaceKb / (1024f * 1024f);
        float availableSpaceGb = availableSpaceKb / (1024f * 1024f);

        return new DiskSpace
        {
            OverallUsagePercent = overallUsagePercent,
            FreeSpace = availableSpaceGb,
            AvailableSpacePercent = availableSpacePercent,
            TotalSpace = totalSpaceGb
        };
    }

    private static string RunCommand(string command, string arguments)
    {
        using var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return result;
    }
}
