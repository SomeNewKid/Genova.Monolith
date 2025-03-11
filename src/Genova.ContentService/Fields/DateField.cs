namespace Genova.ContentService.Fields;

using System;

public class DateField : Field
{
    private DateOnly _value = DateOnly.MinValue;

    public override string FieldType => "Date";

    /// <summary>
    /// Attempts to parse the input as a date (DateOnly).
    /// Throws an ArgumentException if the input is null, empty, or not a valid date.
    /// </summary>
    public override void SetValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!DateOnly.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid date format.",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Returns the date in the format "yyyy-MM-dd".
    /// </summary>
    public override string GetValue()
    {
        return _value.ToString("yyyy-MM-dd");
    }
}
