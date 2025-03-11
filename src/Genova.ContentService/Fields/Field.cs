namespace Genova.ContentService.Fields;

public abstract class Field : IField
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public abstract string FieldType { get; }

    public abstract void SetValue(string? value);

    public abstract string GetValue();
}