using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class UrlField_Tests
{
    [Fact]
    public void FieldType_returns_Url()
    {
        // Arrange
        var field = new UrlField();

        // Act
        var fieldType = field.FieldType;

        // Assert
        Assert.Equal("Url", fieldType);
    }

    [Fact]
    public void GetValue_returns_empty_string_if_not_set()
    {
        // Arrange
        var field = new UrlField();

        // Act
        var result = field.GetValue();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("path")]
    [InlineData("/root/relative")]
    [InlineData("//example.com")]
    [InlineData("http://example.com/path")]
    [InlineData("custom-scheme://any.domain?q=123#anchor")]
    public void SetValue_with_valid_url_stores_it(string input)
    {
        // Arrange
        var field = new UrlField();

        // Act
        field.SetValue(input);
        var result = field.GetValue();

        // Assert
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void SetValue_with_empty_throws_ArgumentException(string input)
    {
        // Arrange
        var field = new UrlField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }

    [Theory]
    [InlineData("  path with space  ")]
    [InlineData("://somewhere")]
    [InlineData("http://double://bad")]
    [InlineData("multiple?questions?here")]
    [InlineData("myurl#two#anchors")]
    [InlineData("invalid<chars")]
    public void SetValue_with_invalid_url_throws_ArgumentException(string input)
    {
        // Arrange
        var field = new UrlField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }
}
