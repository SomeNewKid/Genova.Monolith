using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class TextField_Tests
{
    [Fact]
    public void SetValue_with_null_sets_empty_string()
    {
        // Arrange
        var textField = new TextField();

        // Act
        textField.SetValue(null);

        // Assert
        Assert.Equal(string.Empty, textField.GetValue());
    }

    [Theory]
    [InlineData("")]
    [InlineData("Hello World")]
    public void SetValue_with_valid_strings_sets_value(string input)
    {
        // Arrange
        var textField = new TextField();

        // Act
        textField.SetValue(input);

        // Assert
        Assert.Equal(input, textField.GetValue());
    }

    [Fact]
    public void FieldType_returns_expected_identifier()
    {
        // Arrange
        var textField = new TextField();

        // Act
        var fieldType = textField.FieldType;

        // Assert
        Assert.Equal("Text", fieldType);
    }
}