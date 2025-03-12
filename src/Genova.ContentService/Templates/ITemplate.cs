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

    TemplateMode Mode { get; }

    void MarkPopulated();

    /// <summary>
    /// Validates this template and all of its children.
    /// Returns a collection of error messages. If the collection is empty,
    /// this template is valid.
    /// </summary>
    /// <returns>
    /// An IEnumerable of error messages. An empty collection implies no validation errors.
    /// </returns>
    IEnumerable<string> Validate();
}