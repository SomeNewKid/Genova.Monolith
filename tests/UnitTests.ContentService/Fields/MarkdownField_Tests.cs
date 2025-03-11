using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class MarkdownField_Tests
{
    [Fact]
    public void SetValue_with_null_sets_empty_string()
    {
        // Arrange
        var markdownField = new MarkdownField();

        // Act
        markdownField.SetValue(null);

        // Assert
        Assert.Equal(string.Empty, markdownField.GetValue());
    }

    [Theory]
    [InlineData("")]
    [InlineData("# Heading\nSome body text with **bold** or *italic*.")]
    [InlineData("Plain text without special Markdown syntax.")]
    public void SetValue_with_valid_strings_sets_value(string input)
    {
        // Arrange
        var markdownField = new MarkdownField();

        // Act
        markdownField.SetValue(input);

        // Assert
        Assert.Equal(input, markdownField.GetValue());
    }

    [Fact]
    public void FieldType_returns_expected_identifier()
    {
        // Arrange
        var markdownField = new MarkdownField();

        // Act
        var fieldType = markdownField.FieldType;

        // Assert
        Assert.Equal("Markdown", fieldType);
    }
}
