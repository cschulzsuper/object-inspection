using FluentAssertions;
using System;
using System.Security.Cryptography;
using Xunit;

namespace ChristianSchulz.ObjectInspection.Shared.Tests;

public class TypeNameConverterTests
{
    [Theory]
    [InlineData(typeof(CaseStyleConverterTests), "case-style-converter-tests")]
    [InlineData(typeof(MD5), "md5")]
    [InlineData(typeof(TripleDESCng), "triple-des-cng")]
    [InlineData(typeof(byte), "byte")]
    [InlineData(typeof(ushort), "ushort")]
    [InlineData(typeof(uint), "uint")]
    [InlineData(typeof(ulong), "ulong")]
    [InlineData(typeof(UInt128), "uint128")]
    public void ToKebabCase_ReturnsKebabCase_ForType(Type type, string kebabCase)
    {
        // Act
        var result = TypeNameConverter.ToKebabCase(type);

        // Assert
        result.Should().Be(kebabCase);
    }
}