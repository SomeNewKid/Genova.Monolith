namespace Genova.ContentService.Fields;

public class NumberField : Field
{
    private decimal _value;

    public override string FieldType => "Number";

    /// <summary>
    /// Attempts to parse the input string as a decimal.
    /// Throws an ArgumentException if the input is null, empty, or not a valid decimal.
    /// </summary>
    public override void SetValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!decimal.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid decimal.",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Returns the stored decimal as a string.
    /// </summary>
    public override string GetValue()
    {
        return _value.ToString();
    }
}
