using System.Diagnostics.CodeAnalysis;
using DSharp.NetCore.IO;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class DPathTests
{
    [TestMethod]
    public void GetLogPath_ReturnsValidPath()
    {
        // Act
        var logPath = DPath.GetLogPath();

        // Assert
        logPath.Should().NotBeNullOrWhiteSpace();
        Directory.Exists(logPath).Should().BeTrue();
    }

    [TestMethod]
    public void GoBackToPath_ReturnsCorrectPath()
    {
        // arrange

        const string expectedPath = @"D:\Program Files\MijnPath";

        var currentPath = Path.Combine(expectedPath, "dummy1", "dummy2", "dummy4");

        // act
        var path = DPath.GoBackToPath(currentPath, "MijnPath");

        // Guard
        path.Should().Be(expectedPath);
    }

    [TestMethod]
    public void GoBackToFile_ReturnsCorrectPath()
    {
        // arrange

        const string searchPattern = "cmd.exe";
        const string expectedPath = @"C:\Windows\System32";
        const string currentPath = @"C:\Windows\System32\drivers\etc";

        // act
        var path = DPath.GoBackToFile(currentPath, searchPattern);

        // Guard
        path.Should().Be(expectedPath);
    }

    [TestMethod]
    public void GetPath_KnownFolder_ReturnsValidPath()
    {
        // Act
        var downloadsPath = DPath.GetPath(KnownFolder.Downloads);

        // Assert
        downloadsPath.Should().NotBeNullOrWhiteSpace();
        Directory.Exists(downloadsPath).Should().BeTrue();
    }

    [TestMethod]
    public void SHGetKnownFolderPath_InvalidGuid_ThrowsException()
    {
        // Arrange
        var invalidGuid = Guid.NewGuid();

        // Act
        Action act = () => DPath.SHGetKnownFolderPath(invalidGuid, 0);

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }
}