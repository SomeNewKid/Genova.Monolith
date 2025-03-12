using Genova.ContentService.Components;
using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Components;

public class MarkdownComponent_Tests
{
    [Fact]
    public void ComponentType_is_MarkdownComponent()
    {
        // Arrange
        var component = new MarkdownComponent();

        // Act
        var type = component.ComponentType;

        // Assert
        Assert.Equal("MarkdownComponent", type);
    }

    [Fact]
    public void Has_one_field_with_key_content()
    {
        // Arrange
        var component = new MarkdownComponent();

        // Act
        var fields = component.Fields;

        // Assert
        Assert.Single(fields);
        var field = fields[0];
        Assert.Equal("content", field.Key);
        Assert.IsType<MarkdownField>(field);
    }

    [Fact]
    public void MarkdownField_can_store_and_retrieve_value()
    {
        // Arrange
        var component = new MarkdownComponent();
        var markdownField = component.Fields[0]; // The only field

        // Act
        markdownField.SetValue("# Heading\n\nSome **bold** text.");
        var value = markdownField.GetValue();

        // Assert
        Assert.Equal("# Heading\n\nSome **bold** text.", value);
    }

    [Fact]
    public void Children_is_empty_by_default()
    {
        // Arrange
        var component = new MarkdownComponent();

        // Act
        var children = component.Children;

        // Assert
        Assert.Empty(children);
    }
}
