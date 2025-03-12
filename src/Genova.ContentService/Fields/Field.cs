namespace Genova.ContentService.Fields;

public abstract class Field : IField
{
    public string Key { get; private set; } = "";

    public abstract string FieldType { get; }

    public abstract void SetValue(string? value);

    public abstract string GetValue();

    public void SetKey(string key)
    {
        if (!string.IsNullOrEmpty(Key))
            throw new InvalidOperationException("Key is already set; cannot overwrite it.");

        Key = key;
    }
}