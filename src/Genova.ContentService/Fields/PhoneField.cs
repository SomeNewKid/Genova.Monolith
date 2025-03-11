using Genova.ContentService.Fields;

namespace Genova.ContentService.Fields;

public class PhoneField : Field
{
    private PhoneValue? _phoneValue;

    public override string FieldType => "Phone";

    /// <summary>
    /// Sets the phone field's value by creating a PhoneValue object, which will
    /// handle normalization and validation. If validation fails, an ArgumentException is thrown.
    /// </summary>
    public override void SetValue(string? value)
    {
        // Delegates to PhoneValue's Create method (which can throw ArgumentException).
        // If you want to allow an "unset" phone, you could handle null/empty here
        // and set _phoneValue to null. For now, we rely on the domain logic in PhoneValue.
        _phoneValue = PhoneValue.Create(value);
    }

    /// <summary>
    /// Returns the normalized phone string, or empty if no value is set.
    /// </summary>
    public override string GetValue()
    {
        return _phoneValue?.Value ?? string.Empty;
    }
}
