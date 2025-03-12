namespace Genova.ContentService.Fields;

/// <summary>
/// An abstract base field for referencing another document (e.g., an image or PDF).
/// The <see cref="DocumentType"/> property indicates which type of document is expected,
/// and the stored value is typically the GUID (or other unique ID) referencing that foreign document.
/// </summary>
public abstract class DocumentField : Field
{
    private string? _documentId;

    public override string FieldType => "Document";

    /// <summary>
    /// Indicates which kind of foreign document this field references,
    /// such as "Image", "PDF", "Webpage", "Stylesheet", etc.
    /// Concrete field classes (e.g., <see cref="ImageField"/>) override this property
    /// to specify their document type.
    /// </summary>
    public abstract string DocumentType { get; }

    /// <summary>
    /// Sets the ID (GUID or other unique identifier) of the referenced document.
    /// If the value is null, the field references no document.
    /// </summary>
    public override void SetValue(string? value)
    {
        _documentId = value; // e.g., "1F3452AB-..." or "12345" or any doc ID format
    }

    /// <summary>
    /// Returns the current document ID.
    /// </summary>
    public override string GetValue()
    {
        return _documentId ?? string.Empty;
    }
}