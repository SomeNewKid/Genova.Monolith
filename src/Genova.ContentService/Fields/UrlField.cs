namespace Genova.ContentService.Fields;

public class UrlField : Field
{
    private UrlValue? _urlValue;

    public override string FieldType => "Url";

    /// <summary>
    /// Stores a URL, which may be absolute, relative, root-relative, or protocol-relative,
    /// subject to validation rules in UrlValue.
    /// </summary>
    /// <param name="value">The raw string to set (trimmed internally). May not be null or empty.</param>
    public override void SetValue(string? value)
    {
        // Use the UrlValue value object to enforce domain-level rules.
        _urlValue = UrlValue.Create(value);
    }

    /// <summary>
    /// Returns the stored URL string, or empty if not yet set.
    /// </summary>
    public override string GetValue()
    {
        return _urlValue?.Value ?? string.Empty;
    }
}
