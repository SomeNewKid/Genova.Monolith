using Genova.ContentService.Fields;
using Genova.ContentService.Components;

namespace UnitTests.ContentService.Components;

public class TextComponent_Tests
{
    [Fact]
    public void ComponentType_is_TextComponent()
    {
        // Arrange
        var component = new TextComponent();

        // Act
        var type = component.ComponentType;

        // Assert
        Assert.Equal("TextComponent", type);
    }

    [Fact]
    public void Has_one_field_with_key_text()
    {
        // Arrange
        var component = new TextComponent();

        // Act
        var fields = component.Fields;

        // Assert
        Assert.Single(fields);
        var field = fields[0];
        Assert.Equal("text", field.Key);
        Assert.IsType<TextField>(field);
    }

    [Fact]
    public void TextField_can_store_and_retrieve_value()
    {
        // Arrange
        var component = new TextComponent();
        var textField = component.Fields[0]; // The only field

        // Act
        textField.SetValue("Hello, world!");
        var value = textField.GetValue();

        // Assert
        Assert.Equal("Hello, world!", value);
    }

    [Fact]
    public void Children_is_empty_by_default()
    {
        // Arrange
        var component = new TextComponent();

        // Act
        var children = component.Children;

        // Assert
        Assert.Empty(children);
    }
}
