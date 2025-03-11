public interface IField 
{
    string Id { get; set; }
    string Name { get; set; }

    string FieldType { get; }

    void SetValue(string? value);

    string GetValue();
}