using System.Collections.Generic;
using Genova.ContentService.Fields;

namespace Genova.ContentService.Components;

/// <summary>
/// Represents a reusable building block that holds fields (pieces of data)
/// and an ordered list of named child components.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// A unique key for this component within its parent context,
    /// used in dotted paths (e.g., "article.title").
    /// </summary>
    string Key { get; }

    /// <summary>
    /// A string identifier that specifies the kind of component this is,
    /// such as "ArticleComponent", "GalleryComponent", etc.
    /// </summary>
    string ComponentType { get; }

    /// <summary>
    /// A collection of fields (e.g., TextField, DateField) that store
    /// individual data points, such as "Title" or "Summary".
    /// </summary>
    IReadOnlyList<IField> Fields { get; }

    /// <summary>
    /// An ordered list of child components, each of which also has a name and fields.
    /// </summary>
    IReadOnlyList<IComponent> Children { get; }

    void SetKey(string key);

    /// <summary>
    /// Validates this component and all of its children.
    /// Returns a collection of error messages. If the collection is empty,
    /// this component (and its descendants) are valid.
    /// </summary>
    /// <returns>
    /// An IEnumerable of error messages. An empty collection implies no validation errors.
    /// </returns>
    IEnumerable<string> Validate(ValidationMode validationMode);
}
