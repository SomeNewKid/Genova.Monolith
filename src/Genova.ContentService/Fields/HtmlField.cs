namespace Genova.ContentService.Fields;

public class HtmlField : Field
{
    private string _value = string.Empty;

    public override string FieldType => "Html";

    /// <summary>
    /// Sets the field value. If the provided string is null, stores an empty string.
    /// </summary>
    public override void SetValue(string? value)
    {
        _value = value ?? string.Empty;
    }

    /// <summary>
    /// Returns the raw HTML string.
    /// </summary>
    public override string GetValue()
    {
        return _value;
    }
}
