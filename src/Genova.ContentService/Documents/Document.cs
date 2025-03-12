namespace Genova.ContentService.Documents;

public abstract class Document : IDocument
{
    private Guid _id = Guid.Empty;

    /// <summary>
    /// A unique identifier for this document (web page, image, PDF, etc.).
    /// </summary>
    public Guid Id => _id;

    /// <summary>
    /// Allows setting the document's unique Guid ID, if not already set.
    /// Once set, it should not be overwritten.
    /// </summary>
    /// <param name="id">The Guid identifier to assign.</param>
    /// <exception cref="InvalidOperationException">If the ID was previously set.</exception>
    public void SetId(Guid id)
    {
        if (_id != Guid.Empty)
            throw new InvalidOperationException("Id is already set; cannot overwrite it.");

        _id = id;
    }

    /// <summary>
    /// A string-to-string dictionary representing metadata or content values
    /// for this document. For example, "title" => "Hello World" or
    /// "article.content" => "Some text".
    /// </summary>
    public Dictionary<string, string> Values { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Validates this document. Returns an error if <see cref="Id"/> is empty.
    /// Derived classes should override this to add further validation checks,
    /// calling <c>base.Validate()</c> to preserve the ID check.
    /// </summary>
    /// <returns>An IEnumerable of error messages. Empty if valid.</returns>
    public virtual IEnumerable<string> Validate()
    {
        if (_id == Guid.Empty)
        {
            yield return "Document has no assigned ID.";
        }
    }
}