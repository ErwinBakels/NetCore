using System.Diagnostics.CodeAnalysis;
using DSharp.NetCore.IO;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class DApplicationInfoTest
{

    [TestMethod]
    public void Get_TypeWithAssembly_ReturnsDApplicationInfo()
    {
        // Arrange
        var typeWithAssembly = typeof(DAssembly);

        // Act
        var dApplicationInfo = DApplicationInfo.Get(typeWithAssembly);

        // Assert
        dApplicationInfo.Name.Should().Be("DSharp.NetCore.System");
        dApplicationInfo.Version.Should().NotBeNull();
        dApplicationInfo.References.Should().NotBeNull();
    }

    [TestMethod]
    public void Get_TypeWithoutAssembly_ReturnsEmptyDApplicationInfo()
    {
        // Arrange
        var typeWithoutAssembly = typeof(object);

        // Act
        var dApplicationInfo = DApplicationInfo.Get(typeWithoutAssembly);

        // Assert
        dApplicationInfo.Name.Should().Be("System.Private.CoreLib");
        dApplicationInfo.Version.Should().NotBeNull();
        dApplicationInfo.References.Should().NotBeNull();
        dApplicationInfo.References.Should().BeEmpty();
    }
}