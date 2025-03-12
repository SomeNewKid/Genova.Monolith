namespace Genova.ContentService.Fields; 

public interface IField 
{
    string Key { get; }

    string FieldType { get; }

    void SetValue(string? value);

    string GetValue();

    void SetKey(string key);
}