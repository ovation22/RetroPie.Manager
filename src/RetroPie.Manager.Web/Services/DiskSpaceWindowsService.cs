using RetroPie.Manager.Web.Interfaces;
using RetroPie.Manager.Web.Models.DTOs;

namespace RetroPie.Manager.Web.Services;

public class DiskSpaceWindowsService : IDiskSpaceService
{
    public DiskSpace GetDiskSpace()
    {
        DriveInfo drive = new DriveInfo("C");

        float driveTotalSize = drive.TotalSize / (1024f * 1024f * 1024f);
        float driveTotalFreeSpace = drive.TotalFreeSpace / (1024f * 1024f * 1024f);
        float overallUsagePercent = 100f - (100f * driveTotalFreeSpace / driveTotalSize);
        float availableSpacePercent = 100f - overallUsagePercent;

        return new DiskSpace
        {
            OverallUsagePercent = overallUsagePercent,
            FreeSpace = driveTotalFreeSpace,
            AvailableSpacePercent = availableSpacePercent,
            TotalSpace = driveTotalSize
        };
    }
}
