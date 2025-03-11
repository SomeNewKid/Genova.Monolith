using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class BooleanField_Tests
{
    [Theory]
    [InlineData("true", true)]
    [InlineData("True", true)]
    [InlineData("false", false)]
    [InlineData("False", false)]
    public void SetValue_with_valid_strings_sets_internal_value(string input, bool expected)
    {
        // Arrange
        var booleanField = new BooleanField();

        // Act
        booleanField.SetValue(input);

        // Assert
        var actualValue = bool.Parse(booleanField.GetValue());
        Assert.Equal(expected, actualValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("yes")]
    [InlineData("no")]
    [InlineData("not-a-boolean")]
    public void SetValue_with_invalid_strings_throws_ArgumentException(string input)
    {
        // Arrange
        var booleanField = new BooleanField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => booleanField.SetValue(input));
    }

    [Theory]
    [InlineData("true", "True")]
    [InlineData("false", "False")]
    public void GetValue_returns_string_representation_of_boolean(string input, string expected)
    {
        // Arrange
        var booleanField = new BooleanField();

        // Act
        booleanField.SetValue(input);
        var result = booleanField.GetValue();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FieldType_returns_expected_value()
    {
        // Arrange
        var booleanField = new BooleanField();

        // Act
        var fieldType = booleanField.FieldType;

        // Assert
        Assert.Equal("Boolean", fieldType);
    }
}
