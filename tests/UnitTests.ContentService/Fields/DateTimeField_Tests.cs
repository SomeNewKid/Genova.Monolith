using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class DateTimeField_Tests
{
    [Theory]
    [InlineData("2025-03-11T13:45:00Z")]
    [InlineData("2025-03-11T13:45:00+02:00")]
    [InlineData("2025-03-11 13:45:00")]
    public void SetValue_with_valid_date_sets_internal_value(string input)
    {
        // Arrange
        var dateTimeField = new DateTimeField();

        // Act
        dateTimeField.SetValue(input);

        // Assert
        var output = dateTimeField.GetValue();
        var parsedBack = DateTimeOffset.Parse(output);
        Assert.Equal(parsedBack, DateTimeOffset.Parse(input));
    }

    [Fact]
    public void SetValue_with_null_throws_ArgumentException()
    {
        // Arrange
        var dateTimeField = new DateTimeField();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => dateTimeField.SetValue(null)
        );
    }

    [Fact]
    public void SetValue_with_empty_throws_ArgumentException()
    {
        // Arrange
        var dateTimeField = new DateTimeField();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => dateTimeField.SetValue(string.Empty)
        );
    }

    [Theory]
    [InlineData("Not a date")]
    [InlineData("2025-99-99T99:99:99Z")]
    public void SetValue_with_invalid_string_throws_ArgumentException(string input)
    {
        // Arrange
        var dateTimeField = new DateTimeField();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => dateTimeField.SetValue(input)
        );
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var dateTimeField = new DateTimeField();

        // Act
        var fieldType = dateTimeField.FieldType;

        // Assert
        Assert.Equal("DateTime", fieldType);
    }

    [Fact]
    public void GetValue_returns_ISO_8601_string()
    {
        // Arrange
        var dateTimeField = new DateTimeField();
        var testDate = new DateTimeOffset(2025, 3, 11, 13, 45, 0, TimeSpan.Zero);

        // Act
        dateTimeField.SetValue(testDate.ToString("o"));
        var valueString = dateTimeField.GetValue();

        // Assert
        Assert.Equal(testDate, DateTimeOffset.Parse(valueString));
    }

    // -------------------------------
    // Overloaded SetValue(DateTime) Tests
    // -------------------------------

    [Fact]
    public void SetValue_overload_with_UTC_datetime_stores_as_same_UTC_offset()
    {
        // Arrange
        var dateTimeField = new DateTimeField();
        var utcDate = new DateTime(2025, 3, 12, 9, 30, 0, DateTimeKind.Utc);

        // Act
        dateTimeField.SetValue(utcDate);
        var stored = DateTimeOffset.Parse(dateTimeField.GetValue());

        // Assert
        Assert.Equal(utcDate, stored.UtcDateTime);
        Assert.Equal(TimeSpan.Zero, stored.Offset);
    }

    [Fact]
    public void SetValue_overload_with_local_datetime_converts_to_UTC()
    {
        // Arrange
        var dateTimeField = new DateTimeField();
        var localDate = new DateTime(2025, 3, 12, 9, 30, 0, DateTimeKind.Local);

        // Act
        dateTimeField.SetValue(localDate);
        var stored = DateTimeOffset.Parse(dateTimeField.GetValue());

        // Assert
        // The stored offset should be zero (UTC).
        Assert.Equal(TimeSpan.Zero, stored.Offset);
        // We confirm it is the same moment in time as localDate in UTC.
        Assert.Equal(localDate.ToUniversalTime(), stored.UtcDateTime);
    }

    [Fact]
    public void SetValue_overload_with_unspecified_kind_treats_as_local_and_converts_to_UTC()
    {
        // Arrange
        var dateTimeField = new DateTimeField();
        var unspecifiedDate = new DateTime(2025, 3, 12, 9, 30, 0, DateTimeKind.Unspecified);

        // Act
        dateTimeField.SetValue(unspecifiedDate);
        var stored = DateTimeOffset.Parse(dateTimeField.GetValue());

        // Assert
        // The offset is zero, so the field is storing UTC time.
        Assert.Equal(TimeSpan.Zero, stored.Offset);

        // Because the 'Unspecified' was treated as local, we compare
        // to 'unspecifiedDate.ToUniversalTime()' for equality.
        Assert.Equal(unspecifiedDate.ToUniversalTime(), stored.UtcDateTime);
    }
}