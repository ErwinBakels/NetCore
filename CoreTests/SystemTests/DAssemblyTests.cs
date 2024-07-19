using System.Reflection;
using DSharp.NetCore.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[TestClass]
public class DAssemblyTests
{
    [TestMethod]
    public void Parse_ValidAssemblyName_ReturnsDAssembly()
    {
        // Arrange
        var assemblyName = new AssemblyName("SampleAssembly")
        {
            Version = new Version(1, 2, 3, 4)
        };

        // Act
        var dAssembly = DAssembly.Parse(assemblyName);

        // Assert
        dAssembly.Name.Should().Be("SampleAssembly");
        dAssembly.Version.ToString().Should().Be("1.2.3");
    }

    [TestMethod]
    public void ToString_ReturnsCorrectString()
    {
        // Arrange
        var dAssembly = new DAssembly
        {
            Name = "TestAssembly",
            Version = new DVersion { Major = 1, Minor = 2, Build = 3 }
        };

        // Act
        var result = dAssembly.ToString();

        // Assert
        result.Should().Be("TestAssembly v1.2.3");
    }
}