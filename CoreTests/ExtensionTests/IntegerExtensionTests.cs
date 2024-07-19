using System.Diagnostics.CodeAnalysis;

using DSharp.NetCore.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.ExtensionTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class IntegerExtensionTests
{
    [TestMethod]
    public void BitsTests()
    {
        // arrange

        int a = 1;
        a.SetBits(4);


        // Guard
        a.Should().Be(5);
    }

    [TestMethod]
    public void MinMaxTest()
    {
        // Arrange
        const int value = 5;
        const int minValue = 1;
        const int maxValue = 10;

        // Act
        int result = value.MinMax(minValue, maxValue);

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void MinMaxTest_ValueLessThanMin()
    {
        // Arrange
        const int value = -1;
        const int minValue = 1;
        const int maxValue = 10;

        // Act
        int result = value.MinMax(minValue, maxValue);

        // Assert
        Assert.AreEqual(minValue, result);
    }

    [TestMethod]
    public void MinMaxTest_ValueGreaterThanMax()
    {
        // Arrange
        const int value = 11;
        const int minValue = 1;
        const int maxValue = 10;

        // Act
        int result = value.MinMax(minValue, maxValue);

        // Assert
        Assert.AreEqual(maxValue, result);
    }
}

