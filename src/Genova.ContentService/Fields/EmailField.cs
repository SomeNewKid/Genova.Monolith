namespace Genova.ContentService.Fields;

/// <summary>
/// A field that stores an email address. Internally uses <see cref="EmailValue"/>
/// to validate and normalize the address string.
/// </summary>
public class EmailField : Field
{
    private EmailValue? _emailValue;

    /// <summary>
    /// Indicates this field’s type string (e.g., "Email"),
    /// used in serialization or reflection-based logic.
    /// </summary>
    public override string FieldType => "Email";

    /// <summary>
    /// Sets the field’s value by creating an <see cref="EmailValue"/> object,
    /// which will handle validation. If validation fails, an ArgumentException is thrown.
    /// </summary>
    /// <param name="value">The raw input string (may be null or empty).</param>
    public override void SetValue(string? value)
    {
        // Decide if you want to allow null/empty as “no email.” 
        // Example: if null/empty => clear any existing EmailValue:
        if (string.IsNullOrWhiteSpace(value))
        {
            _emailValue = null;
            return;
        }

        // Attempt to create an EmailValue, which may throw an ArgumentException if invalid.
        _emailValue = EmailValue.Create(value);
    }

    /// <summary>
    /// Returns the current validated email address as a string,
    /// or an empty string if unset or invalid.
    /// </summary>
    public override string GetValue()
    {
        return _emailValue?.Value ?? string.Empty;
    }
}