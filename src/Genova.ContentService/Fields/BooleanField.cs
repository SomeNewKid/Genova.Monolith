namespace Genova.ContentService.Fields;

public class BooleanField : Field
{
    private bool _value;

    public override string FieldType => "Boolean";

    /// <summary>
    /// Attempts to parse the input string as a boolean value.
    /// If parsing fails, throws an ArgumentException.
    /// </summary>
    public override void SetValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!bool.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid boolean (expected true/false).",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Returns "True" or "False" depending on the current internal value.
    /// </summary>
    public override string GetValue()
    {
        return _value.ToString();
    }
}