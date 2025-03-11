namespace Genova.ContentService.Fields;

public class TextField : Field
{
    private string _value = string.Empty;

    public override string FieldType => "Text";

    public override void SetValue(string? value)
    {
        _value = value ?? string.Empty;
    }

    public override string GetValue()
    {
        return _value;
    }
}
