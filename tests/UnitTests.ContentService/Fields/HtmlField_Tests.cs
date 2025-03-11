using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class HtmlField_Tests
{
    [Fact]
    public void SetValue_with_null_sets_empty_string()
    {
        // Arrange
        var htmlField = new HtmlField();

        // Act
        htmlField.SetValue(null);

        // Assert
        Assert.Equal(string.Empty, htmlField.GetValue());
    }

    [Theory]
    [InlineData("")]
    [InlineData("<p>Hello World</p>")]
    [InlineData("<div class='test'>Some <strong>bold</strong> text</div>")]
    public void SetValue_with_valid_strings_stores_value(string input)
    {
        // Arrange
        var htmlField = new HtmlField();

        // Act
        htmlField.SetValue(input);

        // Assert
        Assert.Equal(input, htmlField.GetValue());
    }

    [Fact]
    public void FieldType_returns_expected_identifier()
    {
        // Arrange
        var htmlField = new HtmlField();

        // Act
        var fieldType = htmlField.FieldType;

        // Assert
        Assert.Equal("Html", fieldType);
    }
}
