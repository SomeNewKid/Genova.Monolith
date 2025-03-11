using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class TimeSpanField_Tests
{
    [Theory]
    [InlineData("00:00:00", "00:00:00")]
    [InlineData("02:30:00", "02:30:00")]
    [InlineData("1.01:15:30", "1.01:15:30")] // 1 day, 1 hour, 15 min, 30 sec
    [InlineData("-00:30:00", "-00:30:00")]  // Negative 30 minutes
    public void SetValue_with_valid_times_sets_internal_value(string input, string expected)
    {
        // Arrange
        var timeSpanField = new TimeSpanField();

        // Act
        timeSpanField.SetValue(input);

        // Assert
        var output = timeSpanField.GetValue();
        Assert.Equal(expected, output);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SetValue_with_null_or_empty_throws_ArgumentException(string input)
    {
        // Arrange
        var timeSpanField = new TimeSpanField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => timeSpanField.SetValue(input));
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("not-a-timespan")]
    [InlineData("00:60:00")]  // 60 min in the seconds slot isn't valid
    public void SetValue_with_invalid_string_throws_ArgumentException(string input)
    {
        // Arrange
        var timeSpanField = new TimeSpanField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => timeSpanField.SetValue(input));
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var timeSpanField = new TimeSpanField();

        // Act
        var fieldType = timeSpanField.FieldType;

        // Assert
        Assert.Equal("TimeSpan", fieldType);
    }

    [Fact]
    public void GetValue_returns_constant_format()
    {
        // Arrange
        var timeSpanField = new TimeSpanField();
        var testValue = "1.00:10:05"; // 1 day, 0 hours, 10 minutes, 5 seconds

        // Act
        timeSpanField.SetValue(testValue);
        var result = timeSpanField.GetValue();

        // Assert
        Assert.Equal("1.00:10:05", result);
    }
}
