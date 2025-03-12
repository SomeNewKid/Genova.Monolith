namespace Genova.ContentService.Fields;

public class ListField : Field
{
    // Internally store values in a HashSet so that:
    // 1) There's no guaranteed order
    // 2) No duplicates
    // 3) Case sensitivity depends on your design; here we do case-sensitive matching
    private readonly HashSet<string> _values = new();

    public override string FieldType => "List";

    /// <summary>
    /// Exposes the collection of values (unordered, no duplicates).
    /// </summary>
    public IReadOnlyCollection<string> Values => _values;

    /// <summary>
    /// Clears and resets the set of values from the provided string.
    /// If the input is null/whitespace, we leave the set empty.
    /// 
    /// This default approach splits on commas. 
    /// Example: "tag1, tag2,tag3" => { "tag1", "tag2", "tag3" }.
    /// </summary>
    public override void SetValue(string? value)
    {
        _values.Clear();
        if (string.IsNullOrWhiteSpace(value))
            return;

        // Split on commas; trim each token
        var tokens = value.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var t in tokens)
        {
            _values.Add(t.Trim());
        }
    }

    /// <summary>
    /// Joins the current set of values into a single comma+space delimited string
    /// (Order is not guaranteed, as we use a HashSet).
    /// </summary>
    public override string GetValue()
    {
        return string.Join(", ", _values);
    }
}