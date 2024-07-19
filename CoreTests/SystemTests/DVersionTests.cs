using DSharp.NetCore.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[TestClass]
public class DVersionTests
{
    [TestMethod]
    public void Parse_WithValidVersion_ReturnsDVersion()
    {
        // Arrange
        var inputVersion = new Version(1, 2, 3);

        // Act
        var dVersion = DVersion.Parse(inputVersion);

        // Assert
        dVersion.Should().NotBeNull();
        dVersion.Major.Should().Be(1);
        dVersion.Minor.Should().Be(2);
        dVersion.Build.Should().Be(3);
    }

    [TestMethod]
    public void Parse_WithNullVersion_ReturnsDefaultDVersion()
    {
        // Act
        var dVersion = DVersion.Parse(null);

        // Assert
        dVersion.Should().NotBeNull();
        dVersion.Major.Should().Be(0);
        dVersion.Minor.Should().Be(0);
        dVersion.Build.Should().Be(0);
    }

    [TestMethod]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var dVersion = new DVersion { Major = 1, Minor = 2, Build = 3 };

        // Act
        var result = dVersion.ToString();

        // Assert
        result.Should().Be("1.2.3");
    }

    [TestMethod]
    public void ToString_WithDefaultValues_ReturnsDefaultFormattedString()
    {
        // Arrange
        var dVersion = new DVersion(); // Default values (0, 0, 0)

        // Act
        var result = dVersion.ToString();

        // Assert
        result.Should().Be("0.0.0");
    }
}