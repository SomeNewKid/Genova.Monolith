using System;
using System.Linq;
using Xunit;
using Genova.ContentService.Components;
using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Components;

public class ArticleComponent_Tests
{
    [Fact]
    public void ComponentType_is_ArticleComponent()
    {
        // Arrange
        var component = new ArticleComponent();

        // Act
        var type = component.ComponentType;

        // Assert
        Assert.Equal("ArticleComponent", type);
    }

    [Fact]
    public void Has_expected_fields()
    {
        // Arrange
        var component = new ArticleComponent();

        // Act
        var fields = component.Fields;

        // Assert
        Assert.Equal(4, fields.Count);

        // We'll verify each key and type
        Assert.Contains(fields, f => f.Key == "title" && f is TextField);
        Assert.Contains(fields, f => f.Key == "summary" && f is MarkdownField);
        Assert.Contains(fields, f => f.Key == "content" && f is MarkdownField);
        Assert.Contains(fields, f => f.Key == "image" && f is ImageField);
    }

    [Theory]
    [InlineData("title", "Hello World")]
    [InlineData("summary", "# Summary\n\nSome *markdown* content.")]
    [InlineData("content", "## Heading\n\nDetail text here.")]
    [InlineData("image", "FAKE-GUID-1234")]
    public void Can_set_and_retrieve_field_values(string key, string testValue)
    {
        // Arrange
        var component = new ArticleComponent();
        var field = component.Fields.SingleOrDefault(f => f.Key == key);

        // Act
        field!.SetValue(testValue);
        var result = field.GetValue();

        // Assert
        Assert.Equal(testValue, result);
    }

    [Fact]
    public void Children_is_empty_by_default()
    {
        // Arrange
        var component = new ArticleComponent();

        // Act
        var children = component.Children;

        // Assert
        Assert.Empty(children);
    }

    [Fact]
    public void No_key_yields_error_from_base_class()
    {
        // Arrange
        var article = new ArticleComponent();
        // We intentionally do NOT call SetKey

        // Act
        // We pass Content mode so that "title cannot be empty" might also appear.
        var errors = article.Validate(ValidationMode.Content).ToArray();

        // Assert
        Assert.Contains(
            errors,
            e => e.Contains("has no Key set", StringComparison.OrdinalIgnoreCase)
        );
    }

    [Fact]
    public void Key_provided_but_empty_title_yields_error()
    {
        // Arrange
        var article = new ArticleComponent();
        article.SetKey("article1");

        // The 'title' field exists but is empty (default)
        // so we expect "Title cannot be empty."

        // Act
        var errors = article.Validate(ValidationMode.Content).ToList();

        // Assert
        Assert.Contains(
            errors,
            e => e == "Title cannot be empty."
        );
    }

    [Fact]
    public void Non_empty_title_produces_no_errors()
    {
        // Arrange
        var article = new ArticleComponent();
        article.SetKey("article2");

        // Fill the 'title' field
        var titleField = article.Fields.SingleOrDefault(f => f.Key == "title");
        titleField?.SetValue("My Great Article");

        // Act
        var errors = article.Validate(ValidationMode.Content).ToList();

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Multiple_errors_returned_if_no_key_and_empty_title()
    {
        // Arrange
        var article = new ArticleComponent();
        // We do NOT set a key => error #1
        // The 'title' field is also empty => error #2

        // Act
        var errors = article.Validate(ValidationMode.Content).ToArray();

        // Assert
        Assert.True(errors.Length >= 2, "We expect at least 2 errors.");

        // Check for mention of 'no Key set'
        Assert.Contains(errors, e => e.Contains("has no Key set", StringComparison.OrdinalIgnoreCase));

        // Check for mention of 'Title cannot be empty.'
        Assert.Contains(errors, e => e == "Title cannot be empty.");
    }
}
