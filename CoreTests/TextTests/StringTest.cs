using System.Diagnostics.CodeAnalysis;

using DSharp.NetCore.Extensions;
using DSharp.NetCore.Models;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.TextTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class StringTest
{
    [TestMethod]
    public void NearestDateTest()
    {
        var value = "40-40-2000";
        var result = Safe.NearestValidDate(value);

        result.Should().Be(new DateTime(2000, 12, 31));
    }

    [TestMethod]
    public void CompareTest()
    {
        var obj1 = new TestObject { Id = 1, Name = "Erwin" };
        var obj2 = new TestObject { Id = 2, Name = "Erwin" };
        var result = obj1.CompareTo(obj2);
        result.Should().Be(-1);

        result = string.Compare("Erwin", "Erwin", StringComparison.Ordinal);
        result.Should().Be(0);

        result = string.Compare("Erwin1", "Erwin2", StringComparison.Ordinal);
        result.Should().Be(-1);

        result = 22.CompareTo(23);
        result.Should().Be(-1);

        result = 23.CompareTo(22);
        result.Should().Be(1);


    }


    [TestMethod]
    public void RemoveTrailingCharsTest()
    {
        const string value = "a@bc/@";

        // arrange
        const string expected = "a@bc";

        // act
        var result = value.RemoveTrailingChars("\\@/");

        // Guard
        result.Should().Be(expected);
    }

    [TestMethod]
    public void ContainTest()
    {
        const string search = "bak";

        var result = Check.ContainsCi("erwin", search);

        result.Should().Be(false);

        result = Check.ContainsCi("erwin bakels", search);

        result.Should().Be(true);
    }

    [TestMethod]
    public void ContainingTest()
    {
        const string search = "bak";

        var result = Check.ContainingInCi(search, "erwin", "jammer", "");

        result.Should().Be(false);

        result = Check.ContainingInCi(search, "erwin", "bakels");

        result.Should().Be(true);
    }

    [TestMethod]
    public void UnbraceTest()
    {
        const string name = "Erwin";

        var changed = name.DQuoted();

        var result = changed.Unbrace();

        result.Should().Be(name);
    }


    [TestMethod]
    public void ReplaceTokensTest()
    {
        // arrange
        const string content = "Root #{SystemRoot}#";
        const string expectedContent = @"Root C:\WINDOWS";

        var env = Environment.GetEnvironmentVariables();

        // act
        var path = content.ReplaceTokens(env);

        // Guard
        path.ToLower().Should().Be(expectedContent.ToLower());
    }

    [TestMethod]
    public void ReplaceTokensTestJson()
    {
        // arrange
        const string content = "{Root #{SystemRoot}#}";
        const string expectedContent = @"{Root C:\\WINDOWS}";

        var env = Environment.GetEnvironmentVariables();

        // act
        var path = content.ReplaceTokens(env);

        // Guard
        path.ToLower().Should().Be(expectedContent.ToLower());
    }
}
