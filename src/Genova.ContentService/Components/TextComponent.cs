using Genova.ContentService.Fields;

namespace Genova.ContentService.Components;

/// <summary>
/// A minimal component containing a single text field.
/// </summary>
public class TextComponent : Component
{
    /// <summary>
    /// Indicates this component's type (e.g., "TextComponent").
    /// </summary>
    public override string ComponentType => "TextComponent";

    /// <summary>
    /// Constructor. Creates and adds a TextField with the key "text".
    /// </summary>
    public TextComponent()
    {
        // Create a TextField for the body text
        var textField = new TextField();
        textField.SetKey("text");

        // Add it to the component's field list
        AddField(textField);
    }
}