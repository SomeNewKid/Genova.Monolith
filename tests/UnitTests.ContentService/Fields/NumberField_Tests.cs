using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class NumberField_Tests
{
    [Theory]
    [InlineData("0", "0")]
    [InlineData("123.45", "123.45")]
    [InlineData("-99.99", "-99.99")]
    [InlineData("9999999999999999999999999999", "9999999999999999999999999999")]
    public void SetValue_with_valid_numbers_sets_internal_value(string input, string expected)
    {
        // Arrange
        var numberField = new NumberField();

        // Act
        numberField.SetValue(input);

        // Assert
        var output = numberField.GetValue();
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetValue_with_null_or_empty_throws_ArgumentException(string input)
    {
        // Arrange
        var numberField = new NumberField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => numberField.SetValue(input));
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123-45")]
    [InlineData("not-a-number")]
    public void SetValue_with_invalid_strings_throws_ArgumentException(string input)
    {
        // Arrange
        var numberField = new NumberField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => numberField.SetValue(input));
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var numberField = new NumberField();

        // Act
        var fieldType = numberField.FieldType;

        // Assert
        Assert.Equal("Number", fieldType);
    }

    [Fact]
    public void GetValue_returns_string_representation_of_decimal()
    {
        // Arrange
        var numberField = new NumberField();
        const string testString = "42.25";

        // Act
        numberField.SetValue(testString);
        var result = numberField.GetValue();

        // Assert
        Assert.Equal(testString, result);
    }
}
