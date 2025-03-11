namespace Genova.ContentService.Fields;

using System;

public class TimeField : Field
{
    private TimeOnly _value = TimeOnly.MinValue;

    public override string FieldType => "Time";

    /// <summary>
    /// Attempts to parse the input string as a TimeOnly.
    /// Throws an ArgumentException if the input is null, empty, or not a valid time.
    /// </summary>
    public override void SetValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!TimeOnly.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid time format.",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Returns the time in the format "HH:mm:ss" (24-hour clock).
    /// </summary>
    public override string GetValue()
    {
        return _value.ToString("HH:mm:ss");
    }
}
