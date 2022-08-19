using FluentAssertions;
using System;
using System.Security.Cryptography;
using Xunit;

namespace Super.Paula
{
    public class TypeNameConverterTests
    {
        [Theory]
        [InlineData(typeof(CaseStyleConverterTests), "case-style-converter-tests")]
        [InlineData(typeof(MD5), "md5")]
        [InlineData(typeof(TripleDESCng), "triple-des-cng")]
        [InlineData(typeof(Byte), "byte")]
        [InlineData(typeof(UInt16), "ushort")]
        [InlineData(typeof(UInt32), "uint")]
        [InlineData(typeof(UInt64), "ulong")]
        [InlineData(typeof(UInt128), "uint128")]
        public void ToKebabCase_ReturnsKebabCase_ForType(Type type, string kebabCase)
        {
            // Act
            var result = TypeNameConverter.ToKebabCase(type);

            // Assert
            result.Should().Be(kebabCase);
        }
    }
}
