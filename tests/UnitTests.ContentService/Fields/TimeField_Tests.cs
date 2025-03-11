using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class TimeField_Tests
{
    [Theory]
    [InlineData("00:00:00", "00:00:00")]
    [InlineData("01:23:45", "01:23:45")]
    [InlineData("23:59:59", "23:59:59")]
    [InlineData("7:05:09", "07:05:09")] // single-digit hour => 07
    public void SetValue_with_valid_time_sets_internal_value(string input, string expected)
    {
        // Arrange
        var timeField = new TimeField();

        // Act
        timeField.SetValue(input);

        // Assert
        var output = timeField.GetValue();
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SetValue_with_null_or_empty_throws_ArgumentException(string input)
    {
        // Arrange
        var timeField = new TimeField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => timeField.SetValue(input));
    }

    [Theory]
    [InlineData("24:00:00")]   // 24:00:00 isn't a valid time in .NET's parsing
    [InlineData("not-a-time")]
    [InlineData("12:60:00")]   // 60 minutes is invalid
    [InlineData("12:00:60")]   // 60 seconds is invalid
    public void SetValue_with_invalid_string_throws_ArgumentException(string input)
    {
        // Arrange
        var timeField = new TimeField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => timeField.SetValue(input));
    }

    [Fact]
    public void GetValue_returns_formatted_time()
    {
        // Arrange
        var timeField = new TimeField();

        // Act
        timeField.SetValue("08:07:06");
        var result = timeField.GetValue();

        // Assert
        Assert.Equal("08:07:06", result);
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var timeField = new TimeField();

        // Act
        var fieldType = timeField.FieldType;

        // Assert
        Assert.Equal("Time", fieldType);
    }
}
