using System;
using System.Linq;
using Xunit;
using Genova.ContentService.Components;
using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Components;

public class MetadataComponent_Tests
{
    [Fact]
    public void ComponentType_is_MetadataComponent()
    {
        // Arrange
        var metadata = new MetadataComponent();

        // Act
        var type = metadata.ComponentType;

        // Assert
        Assert.Equal("MetadataComponent", type);
    }

    [Fact]
    public void Has_title_and_description_fields()
    {
        // Arrange
        var metadata = new MetadataComponent();

        // Act
        var fields = metadata.Fields;

        // Assert
        Assert.Equal(2, fields.Count);

        // Ensure there's a title field of type TextField
        var titleField = fields.SingleOrDefault(f => f.Key == "title");
        Assert.NotNull(titleField);
        Assert.IsType<TextField>(titleField);

        // Ensure there's a description field of type TextField
        var descField = fields.SingleOrDefault(f => f.Key == "description");
        Assert.NotNull(descField);
        Assert.IsType<TextField>(descField);
    }

    [Fact]
    public void Validate_returns_error_if_title_is_empty()
    {
        // Arrange
        var metadata = new MetadataComponent();
        metadata.SetKey("meta1");
        // The title field is empty by default

        // Act
        // We want content-level checks (title must not be empty), so pass Content mode
        var errors = metadata.Validate(ValidationMode.Content).ToList();

        // Assert
        // We expect at least one error: "Title cannot be empty in MetadataComponent."
        Assert.Contains(errors,
            e => e == "Title cannot be empty in MetadataComponent.");

        // Because we assigned a valid Key, we do NOT expect a "has no Key set" error
        Assert.DoesNotContain(errors,
            e => e.Contains("has no Key set", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_includes_base_error_if_Key_not_set()
    {
        // Arrange
        var metadata = new MetadataComponent();
        // Intentionally NOT calling SetKey

        // Act
        var errors = metadata.Validate(ValidationMode.Content).ToList();

        // Assert
        // The base class warns about missing Key
        Assert.Contains(errors,
            e => e.Contains("has no Key set", StringComparison.OrdinalIgnoreCase));

        // Also warns about the empty title (since we're in Content mode)
        Assert.Contains(errors, e => e == "Title cannot be empty in MetadataComponent.");
    }

    [Fact]
    public void Validate_no_errors_with_non_empty_title()
    {
        // Arrange
        var metadata = new MetadataComponent();
        metadata.SetKey("meta2");

        // Fill the title field
        var titleField = metadata.Fields.Single(f => f.Key == "title");
        titleField.SetValue("My SEO Title");

        // Description can remain empty or be set to something
        var descriptionField = metadata.Fields.Single(f => f.Key == "description");
        descriptionField.SetValue("");

        // Act
        var errors = metadata.Validate(ValidationMode.Content).ToList();

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Can_set_and_retrieve_fields()
    {
        // Arrange
        var metadata = new MetadataComponent();
        metadata.SetKey("meta3");

        var titleField = metadata.Fields.Single(f => f.Key == "title");
        var descField = metadata.Fields.Single(f => f.Key == "description");

        // Act
        titleField.SetValue("Sample Title");
        descField.SetValue("Short description goes here.");

        // Assert
        Assert.Equal("Sample Title", titleField.GetValue());
        Assert.Equal("Short description goes here.", descField.GetValue());
    }
}
