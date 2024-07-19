using System.Diagnostics.CodeAnalysis;
using DSharp.NetCore.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.TextTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class GeneralTest
{
    internal const object? CNull = null;
    internal const string CEmptyString = "";

    internal const string CValue0 = "000,00000";
    internal const string CValue1 = "1000,10";
    internal const string CValue2 = "1.000,10";
    internal const string CValue3 = "1,000.10";
    internal const string CValue4 = "1000.10";
    internal const string CValue5 = "1,000,000.199392";

    internal const string CValue8 = ".001";
    internal const string CValue9 = "100.";
    internal const string CValue10 = "abd8372.91922aa";
    internal const string CValue11 = "abd8372.91.aa.44bs922aa";
    internal const string CValue12 = "-.001";
    internal const string CValue13 = "-26.5";

    [TestMethod]
    public void SafeDecimalTest()
    {
        // arrange
        const decimal cExpectedValue0 = 0M;
        const decimal cExpectedValue1To4 = 1000.1M;
        const decimal cExpectedValue5 = 1000000.199392M;
        const decimal cExpectedValue6 = 0M;
        const decimal cExpectedValue7 = 0M;
        const decimal cExpectedValue12 = -0.001M;
        const decimal cExpectedValue13 = -26.5M;

        // act
        var result0 = Safe.Decimal(CValue0);
        var result1 = Safe.Decimal(CValue1);
        var result2 = Safe.Decimal(CValue2);
        var result3 = Safe.Decimal(CValue3);
        var result4 = Safe.Decimal(CValue4);
        var result5 = Safe.Decimal(CValue5);
        var result6 = Safe.Decimal(CNull);
        var result7 = Safe.Decimal(CEmptyString);
        var result12 = Safe.Decimal(CValue12);
        var result13 = Safe.Decimal(CValue13);

        // Guard
        result0.Should().Be(cExpectedValue0);
        result1.Should().Be(cExpectedValue1To4);
        result2.Should().Be(cExpectedValue1To4);
        result3.Should().Be(cExpectedValue1To4);
        result4.Should().Be(cExpectedValue1To4);
        result5.Should().Be(cExpectedValue5);
        result6.Should().Be(cExpectedValue6);
        result7.Should().Be(cExpectedValue7);
        result12.Should().Be(cExpectedValue12);
        result13.Should().Be(cExpectedValue13);
    }

    [TestMethod]
    public void SafeFloatTest()
    {
        // arrange
        const float cExpectedValueNull = 0F;
        const float cExpectedValueEmpty = 0F;

        const float cExpectedValue0 = 0F;
        const float cExpectedValue1To4 = 1000.1F;
        const float cExpectedValue5 = 1000000.199392F;

        const float cExpectedValue10 = 8372.91922F;
        const float cExpectedValue11 = 8372.91F;
        const float cExpectedValue12 = -0.001F;
        const float cExpectedValue13 = -26.5F;

        // act
        var resultNull = Safe.Float(CNull);
        var resultEmtpy = Safe.Float(CEmptyString);

        var result0 = Safe.Float(CValue0);
        var result1 = Safe.Float(CValue1);
        var result2 = Safe.Float(CValue2);
        var result3 = Safe.Float(CValue3);
        var result4 = Safe.Float(CValue4);
        var result5 = Safe.Float(CValue5);

        var result10 = Safe.Float(CValue10);
        var result11 = Safe.Float(CValue11);
        var result12 = Safe.Float(CValue12);
        var result13 = Safe.Float(CValue13);

        // Guard
        resultNull.Should().Be(cExpectedValueNull);
        resultEmtpy.Should().Be(cExpectedValueEmpty);

        result0.Should().Be(cExpectedValue0);
        result1.Should().Be(cExpectedValue1To4);
        result2.Should().Be(cExpectedValue1To4);
        result3.Should().Be(cExpectedValue1To4);
        result4.Should().Be(cExpectedValue1To4);
        result5.Should().Be(cExpectedValue5);

        result10.Should().Be(cExpectedValue10);
        result11.Should().Be(cExpectedValue11);
        result12.Should().Be(cExpectedValue12);
        result13.Should().Be(cExpectedValue13);
    }

    [TestMethod]
    public void SafeNumberTest()
    {
        // arrange
        const string cExpectedValueNull = "0";
        const string cExpectedValueEmpty = "0";

        const string cExpectedValue0 = "0";
        const string cExpectedValue1To4 = "1000.1";
        const string cExpectedValue5 = "1000000.199392";
        const string cExpectedValue8 = "0.001";
        const string cExpectedValue9 = "100";
        const string cExpectedValue10 = "8372.91922";
        const string cExpectedValue11 = "8372.91";
        const string cExpectedValue12 = "-0.001";
        const string cExpectedValue13 = "-26.5";

        // act
        var resultNull = Safe.Number(CNull);
        var resultEmpty = Safe.Number(CEmptyString);

        var result0 = Safe.Number(CValue0);
        var result1 = Safe.Number(CValue1);
        var result2 = Safe.Number(CValue2);
        var result3 = Safe.Number(CValue3);
        var result4 = Safe.Number(CValue4);
        var result5 = Safe.Number(CValue5);
        var result8 = Safe.Number(CValue8);
        var result9 = Safe.Number(CValue9);
        var result10 = Safe.Number(CValue10);
        var result11 = Safe.Number(CValue11);
        var result12 = Safe.Number(CValue12);
        var result13 = Safe.Number(CValue13);

        // Guard
        resultNull.Should().Be(cExpectedValueNull);
        resultEmpty.Should().Be(cExpectedValueEmpty);

        result0.Should().Be(cExpectedValue0);
        result1.Should().Be(cExpectedValue1To4);
        result2.Should().Be(cExpectedValue1To4);
        result3.Should().Be(cExpectedValue1To4);
        result4.Should().Be(cExpectedValue1To4);
        result5.Should().Be(cExpectedValue5);
        result8.Should().Be(cExpectedValue8);
        result9.Should().Be(cExpectedValue9);
        result10.Should().Be(cExpectedValue10);
        result11.Should().Be(cExpectedValue11);
        result12.Should().Be(cExpectedValue12);
        result13.Should().Be(cExpectedValue13);
    }

    [TestMethod]
    public void OnlyNumericTest()
    {
        // arrange
        const string cExpectedValueNull = "";
        const string cExpectedValueEmpty = "";

        const string cExpectedValue10 = "8372.91922";
        const string cExpectedValue11 = "8372.91..44922";

        // act
        var resultNull = CEmptyString;
        var resultEmpty = CEmptyString;

        var result10 = CValue10.OnlyNumeric();
        var result11 = CValue11.OnlyNumeric();

        // Guard
        resultNull.Should().Be(cExpectedValueNull);
        resultEmpty.Should().Be(cExpectedValueEmpty);

        result10.Should().Be(cExpectedValue10);
        result11.Should().Be(cExpectedValue11);
    }
}
