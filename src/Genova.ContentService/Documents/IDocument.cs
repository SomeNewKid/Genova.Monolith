using System;
using System.Collections.Generic;

using Genova.ContentService.Templates;

namespace Genova.ContentService.Documents;

public interface IDocument
{
    /// <summary>
    /// A unique identifier for this document (web page, image, PDF, etc.).
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Allows setting the document's unique Guid ID, if not already set.
    /// Once set, it should not be overwritten.
    /// </summary>
    /// <param name="id">The Guid identifier to assign.</param>
    void SetId(Guid id);

    /// <summary>
    /// A string-to-string dictionary representing metadata or content values
    /// for this document. For example, 'article.title' => 'Hello World'.
    /// </summary>
    Dictionary<string, string> Values { get; }

    /// <summary>
    /// Validates this document.
    /// Returns a collection of error messages. If the collection is empty,
    /// this document is valid.
    /// </summary>
    /// <returns>
    /// An IEnumerable of error messages. An empty collection implies no validation errors.
    /// </returns>
    IEnumerable<string> Validate();
}
