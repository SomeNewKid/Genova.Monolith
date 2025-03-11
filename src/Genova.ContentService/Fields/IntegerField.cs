namespace Genova.ContentService.Fields;

public class IntegerField : Field
{
    private int _value;

    public override string FieldType => "Integer";

    /// <summary>
    /// Attempts to parse the input string as an integer.
    /// Throws an ArgumentException if the input is null, empty, or not a valid integer.
    /// </summary>
    public override void SetValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!int.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid integer.",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Returns the stored integer value as a string.
    /// </summary>
    public override string GetValue()
    {
        return _value.ToString();
    }
}