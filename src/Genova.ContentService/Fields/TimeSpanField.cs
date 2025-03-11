namespace Genova.ContentService.Fields;

public class TimeSpanField : Field
{
    private TimeSpan _value;

    public override string FieldType => "TimeSpan";

    /// <summary>
    /// Attempts to parse the input string as a TimeSpan.
    /// Throws an ArgumentException if the input is null, empty, or not a valid TimeSpan.
    /// </summary>
    public override void SetValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!TimeSpan.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid TimeSpan.",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Returns the current time interval in the constant ("c") format,
    /// e.g. "1.02:03:04" => 1 day, 2 hours, 3 minutes, 4 seconds.
    /// </summary>
    public override string GetValue()
    {
        return _value.ToString("c");
    }
}
