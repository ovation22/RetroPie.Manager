using System.Runtime.InteropServices;
using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class MemoryUsageWindowsService : IMemoryUsageService
{
    public MemoryUsage GetMemoryUsage()
    {
        MemoryStatusEx memoryStatus = new MemoryStatusEx { dwLength = (uint)Marshal.SizeOf(typeof(MemoryStatusEx)) };


        if (GlobalMemoryStatusEx(memoryStatus))
        {
            float overallUsagePercent = memoryStatus.dwMemoryLoad;
            float freeMemoryGb = memoryStatus.dwAvailPhys / (1024f * 1024f * 1024f);
            float totalMemoryGb = memoryStatus.ullTotalPhys / (1024f * 1024f * 1024f);
            var availableMemoryPercent = freeMemoryGb / totalMemoryGb * 100;
            GetPhysicallyInstalledSystemMemory(out var totalMemory);
            float totalPhysicalMemoryGb = totalMemory / (1024 * 1024);

            return new MemoryUsage
            {
                OverallUsagePercent = overallUsagePercent,
                FreeMemory = freeMemoryGb,
                AvailableMemoryPercent = availableMemoryPercent,
                TotalMemory = totalPhysicalMemoryGb
            };
        }

        throw new InvalidOperationException("Failed to retrieve memory information.");
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx([In] [Out] MemoryStatusEx lpBuffer);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetPhysicallyInstalledSystemMemory(out ulong totalMemory);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class MemoryStatusEx
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public uint dwAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }
}
