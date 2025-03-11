namespace Genova.ContentService.Fields;

public class DateTimeField : Field
{
    private DateTimeOffset _value = DateTimeOffset.MinValue;

    public override string FieldType => "DateTime";

    public override void SetValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }

        if (!DateTimeOffset.TryParse(value, out var parsed))
        {
            throw new ArgumentException(
                $"'{value}' is not a valid date/time format.",
                nameof(value));
        }

        _value = parsed;
    }

    /// <summary>
    /// Overloaded setter that accepts a System.DateTime.
    /// Internally stores it as a DateTimeOffset in UTC.
    /// </summary>
    public void SetValue(DateTime value)
    {
        // If the DateTime has a Kind of Local or Unspecified,
        // convert it to UTC. If it is already Utc, we keep it as is.
        var utcValue = value.Kind == DateTimeKind.Utc
            ? value
            : value.ToUniversalTime();

        // Now store as DateTimeOffset at UTC offset (i.e. +00:00).
        _value = new DateTimeOffset(utcValue, TimeSpan.Zero);
    }

    public override string GetValue()
    {
        // Return in an ISO 8601 round-trip format, e.g. 2025-03-12T09:30:00.0000000Z
        return _value.ToString("o");
    }
}