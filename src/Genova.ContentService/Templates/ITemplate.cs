using Genova.ContentService.Components;

namespace Genova.ContentService.Templates;

public interface ITemplate : IComponent
{
    /// <summary>
    /// A unique identifier for this template. This is primarily used by IDocument
    /// to specify which template it references. 
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Allows setting the template's unique ID (for construction, deserialization, etc.).
    /// Once set, it shouldn't be changed again.
    /// </summary>
    /// <param name="id">The new GUID identifier.</param>
    void SetId(Guid id);
}