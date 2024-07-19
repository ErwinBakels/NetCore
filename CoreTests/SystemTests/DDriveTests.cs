using DSharp.NetCore.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[TestClass]
public class DDriveTests
{
    [TestMethod]
    public void GetLocalDrives_ReturnsListOfDrives()
    {
        // Act
        var drives = DDrive.GetLocalDrives();

        // Assert
        drives.Should().NotBeNull();
        drives.Should().NotBeEmpty();
    }

    [TestMethod]
    public void GetLocalDrives_ReturnsOnlyReadyDrives()
    {
        // Act
        var drives = DDrive.GetLocalDrives();

        // Assert
        foreach (var drive in drives)
        {
            var driveInfo = new DriveInfo(drive);
            driveInfo.IsReady.Should().BeTrue();
        }
    }

    [TestMethod]
    public void GetLocalDrives_ReturnsValidDriveNames()
    {
        // Act
        var drives = DDrive.GetLocalDrives();

        // Assert
        foreach (var drive in drives)
        {
            drive.Should().MatchRegex(@"^[A-Za-z]:\\$");
        }
    }
}