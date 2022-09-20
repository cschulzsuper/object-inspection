using FluentAssertions;
using Xunit;

namespace ChristianSchulz.ObjectInspection.Shared.Tests;

public class CaseStyleConverterTests
{
    [Theory]
    [InlineData("CaseStyleConverterTests", "case-style-converter-tests")]
    [InlineData("MD5", "md5")]
    [InlineData("TripleDESCng", "triple-des-cng")]
    [InlineData("Byte", "byte")]
    [InlineData("UInt16", "u-int-16")]
    [InlineData("UInt32", "u-int-32")]
    [InlineData("UInt64", "u-int-64")]
    [InlineData("UInt128", "u-int-128")]
    public void FromPascalCaseToKebabCase_ReturnsKebabCase_ForPascalCase(string pascalCase, string kebabCase)
    {
        // Act
        var result = CaseStyleConverter.FromPascalCaseToKebabCase(pascalCase);

        // Assert
        result.Should().Be(kebabCase);
    }

    [Theory]
    [InlineData("CaseStyleConverterTests", "caseStyleConverterTests")]
    [InlineData("etag", "etag")]
    public void FromPascalCaseToCamelCase_ReturnsKebabCase_ForPascalCase(string pascalCase, string camelCase)
    {
        // Act
        var result = CaseStyleConverter.FromPascalCaseToKebabCase(pascalCase);

        // Assert
        result.Should().Be(camelCase);
    }
}