using Genova.ContentService.Repositories;

namespace Genova.ContentService.Fields;

/// <summary>
/// A basic implementation of <see cref="IFieldRepository"/> that returns
/// instances of known field classes by their type name (e.g., "TextField").
/// Uses a dictionary-based approach for easy extension.
/// </summary>
public class FieldRepository : IFieldRepository
{
    // Dictionary from type name -> factory that creates the IField
    // Using case-insensitive lookup for convenience.
    private readonly Dictionary<string, Func<IField>> _fieldConstructors;

    public FieldRepository()
    {
        _fieldConstructors = new Dictionary<string, Func<IField>>(StringComparer.OrdinalIgnoreCase)
        {
            { "BooleanField",   () => new BooleanField() },
            { "DateField",      () => new DateField() },
            { "DateTimeField",  () => new DateTimeField() },
            { "TimeField",      () => new TimeField() },
            { "HtmlField",      () => new HtmlField() },
            { "ImageField",     () => new ImageField() },
            { "IntegerField",   () => new IntegerField() },
            { "ListField",      () => new ListField() },
            { "MarkdownField",  () => new MarkdownField() },
            { "NumberField",    () => new NumberField() },
            { "PhoneField",     () => new PhoneField() },
            { "TextField",      () => new TextField() },
            { "TimeSpanField",  () => new TimeSpanField() },
            { "UrlField",       () => new UrlField() },
            { "EmailField",     () => new EmailField() }
        };
    }

    /// <summary>
    /// Returns an instance of <see cref="IField"/> matching the given
    /// type name (e.g., "TextField", "ImageField"), or null if unrecognized.
    /// </summary>
    public IField? GetByType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            return null;

        // Try to find a factory in the dictionary
        if (_fieldConstructors.TryGetValue(typeName, out var factory))
        {
            return factory.Invoke();
        }

        // Unknown type => return null or throw, depending on your design
        return null;
    }
}
