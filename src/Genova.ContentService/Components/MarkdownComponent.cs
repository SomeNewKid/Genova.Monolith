namespace Genova.ContentService.Components;

using Genova.ContentService.Fields;

/// <summary>
/// A component containing a single MarkdownField keyed "content".
/// </summary>
public class MarkdownComponent : Component
{
    /// <summary>
    /// Identifies this component as "MarkdownComponent".
    /// </summary>
    public override string ComponentType => "MarkdownComponent";

    /// <summary>
    /// Constructor: creates and adds a MarkdownField named "content".
    /// </summary>
    public MarkdownComponent()
    {
        var markdownField = new MarkdownField();
        markdownField.SetKey("content");
        AddField(markdownField);
    }
}
