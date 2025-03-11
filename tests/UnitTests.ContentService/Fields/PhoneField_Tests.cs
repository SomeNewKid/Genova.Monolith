using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class PhoneField_Tests
{
    [Fact]
    public void FieldType_returns_Phone()
    {
        // Arrange
        var field = new PhoneField();

        // Act
        var fieldType = field.FieldType;

        // Assert
        Assert.Equal("Phone", fieldType);
    }

    [Fact]
    public void GetValue_returns_empty_if_not_set()
    {
        // Arrange
        var field = new PhoneField();

        // Act
        var result = field.GetValue();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("131456", "131456")]
    [InlineData("93321234", "93321234")]
    [InlineData("08 9332 1234", "0893321234")]
    [InlineData("(08) 9332 1234", "0893321234")]
    [InlineData("(08) 9332-1234", "0893321234")]
    [InlineData("+61 8 9332-1234", "+61893321234")]
    public void SetValue_with_valid_input_stores_normalized_value(string input, string expected)
    {
        // Arrange
        var field = new PhoneField();

        // Act
        field.SetValue(input);
        var actual = field.GetValue();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("()")]
    public void SetValue_with_empty_or_no_digits_throws_ArgumentException(string? input)
    {
        // Arrange
        var field = new PhoneField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }

    [Theory]
    [InlineData("12345")]        // too few digits
    [InlineData("123456789012345678901")] // 21 digits => too many
    public void SetValue_with_out_of_range_digit_count_throws(string input)
    {
        // Arrange
        var field = new PhoneField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }

    [Theory]
    [InlineData("08+9332")]
    [InlineData("089+3321234")]
    public void SetValue_with_plus_not_in_first_position_throws(string input)
    {
        // Arrange
        var field = new PhoneField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }

    [Theory]
    [InlineData("08##9332")]
    [InlineData("Phone12345")]
    [InlineData("+1234_5678")]
    public void SetValue_with_invalid_characters_throws_ArgumentException(string input)
    {
        // Arrange
        var field = new PhoneField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }
}
