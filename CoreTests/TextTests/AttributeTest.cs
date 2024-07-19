using System.Diagnostics.CodeAnalysis;
using DSharp.NetCore.Attributes;
using DSharp.NetCore.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.TextTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class AttributeTest
{
    [TestMethod]
    public void IsFunctional()
    {
        TestEnumerator.Entry.GetStringValue().Should().Be("Value");
        EnumExtensions.EnumerateValues<TestEnumerator>();

        var v = TestEnumerator.Entry.GetStringValue();
    }

    public enum TestEnumerator
    {
        [StringValue("Value")]
        Entry
    }
}