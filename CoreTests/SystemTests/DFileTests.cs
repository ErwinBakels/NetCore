using DSharp.NetCore.IO;
using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[TestClass]
public class DFileTests
{
    private static readonly string CurrentPath = Directory.GetCurrentDirectory();
    private static readonly string TestFileName = Path.Combine(CurrentPath, $"testfile_{Guid.NewGuid()}.txt");

    [TestMethod]
    public void WriteMemoryStream_WithValidArguments_CreatesFile()
    {
        // Arrange
        var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });

        // Act
        Action act = () => DFile.WriteMemoryStream(TestFileName, memoryStream);

        // Assert
        act.Should().NotThrow();
        File.Exists(TestFileName).Should().BeTrue();
    }

    [TestMethod]
    public void WriteMemoryStream_WithInvalidArguments_ThrowsException()
    {
        // Arrange
        MemoryStream? memoryStream = null;

        // Act
        Action act = () => DFile.WriteMemoryStream(TestFileName, memoryStream);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*memoryStream*");
    }

    [TestMethod]
    public void ToMemoryStream_WithValidFile_ReturnsMemoryStream()
    {
        // Arrange
        File.WriteAllText(TestFileName, "Test content");

        // Act
        var result = DFile.ToMemoryStream(TestFileName);

        // Assert
        result.Should().NotBeNull();
        result?.ToArray().Should().NotBeEmpty();
    }

    [TestMethod]
    public void ToMemoryStream_WithInvalidFile_ReturnsNull()
    {
        // Act
        var result = DFile.ToMemoryStream(Path.Combine(CurrentPath, Path.Combine(CurrentPath, $"NonExistingFile_{Guid.NewGuid()}.txt")));

        // Assert
        result.Should().BeNull();
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Clean up the test file
        if (File.Exists(TestFileName))
        {
            File.Delete(TestFileName);
        }
    }
}