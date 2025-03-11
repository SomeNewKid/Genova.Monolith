namespace Genova.ContentService.Fields;

public class MarkdownField : Field
{
    private string _value = string.Empty;

    public override string FieldType => "Markdown";

    /// <summary>
    /// Sets the field value to the given string or empty if null.
    /// </summary>
    public override void SetValue(string? value)
    {
        _value = value ?? string.Empty;
    }

    /// <summary>
    /// Returns the stored Markdown text as a string.
    /// </summary>
    public override string GetValue()
    {
        return _value;
    }
}
