using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

using System;
using Xunit;

public class IntegerField_Tests
{
    [Theory]
    [InlineData("0", 0)]
    [InlineData("12345", 12345)]
    [InlineData("-9876", -9876)]
    public void SetValue_with_valid_integers_sets_internal_value(string input, int expected)
    {
        // Arrange
        var intField = new IntegerField();

        // Act
        intField.SetValue(input);

        // Assert
        var outputStr = intField.GetValue();  // e.g. "12345"
        Assert.Equal(expected.ToString(), outputStr);
        Assert.Equal(expected.ToString(), outputStr);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void SetValue_with_null_or_empty_throws_ArgumentException(string input)
    {
        // Arrange
        var intField = new IntegerField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => intField.SetValue(input));
    }

    [Theory]
    [InlineData("123.45")]
    [InlineData("abc")]
    [InlineData("99999999999999999999999")]  // too large for int
    public void SetValue_with_invalid_strings_throws_ArgumentException(string input)
    {
        // Arrange
        var intField = new IntegerField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => intField.SetValue(input));
    }

    [Fact]
    public void GetValue_returns_string_representation_of_integer()
    {
        // Arrange
        var intField = new IntegerField();
        const int testValue = 42;

        // Act
        intField.SetValue(testValue.ToString());
        var result = intField.GetValue();

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var intField = new IntegerField();

        // Act
        var fieldType = intField.FieldType;

        // Assert
        Assert.Equal("Integer", fieldType);
    }
}