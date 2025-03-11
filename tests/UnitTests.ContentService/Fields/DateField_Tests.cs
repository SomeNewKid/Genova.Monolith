using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class DateField_Tests
{
    [Theory]
    [InlineData("2025-03-11")]
    [InlineData("2000-01-01")]
    [InlineData("9999-12-31")]
    public void SetValue_with_valid_date_sets_internal_value(string input)
    {
        // Arrange
        var dateField = new DateField();

        // Act
        dateField.SetValue(input);

        // Assert
        var output = dateField.GetValue();
        Assert.Equal(input, output);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SetValue_with_null_or_empty_throws_ArgumentException(string input)
    {
        // Arrange
        var dateField = new DateField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => dateField.SetValue(input));
    }

    [Theory]
    [InlineData("Not a date")]
    [InlineData("2025-13-01")] // invalid month
    [InlineData("2025-02-29")] // invalid day if not a leap year
    [InlineData("99999-01-01")] // year out of range
    public void SetValue_with_invalid_string_throws_ArgumentException(string input)
    {
        // Arrange
        var dateField = new DateField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => dateField.SetValue(input));
    }

    [Fact]
    public void GetValue_returns_formatted_date()
    {
        // Arrange
        var dateField = new DateField();
        const string input = "2025-03-15";

        // Act
        dateField.SetValue(input);
        var result = dateField.GetValue();

        // Assert
        Assert.Equal("2025-03-15", result);
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var dateField = new DateField();

        // Act
        var fieldType = dateField.FieldType;

        // Assert
        Assert.Equal("Date", fieldType);
    }
}
