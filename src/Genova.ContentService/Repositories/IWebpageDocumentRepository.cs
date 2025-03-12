using Genova.ContentService.Documents;

namespace Genova.ContentService.Repositories;

/// <summary>
/// A repository interface for performing typical CRUD-like operations
/// on WebpageDocument entities, each implementing <see cref="IWebpageDocument"/>.
/// </summary>
public interface IWebpageDocumentRepository
{
    /// <summary>
    /// Returns all webpage documents in the repository.
    /// </summary>
    /// <returns>An enumeration of all stored <see cref="IWebpageDocument"/> objects.</returns>
    IEnumerable<IWebpageDocument> GetAll();

    /// <summary>
    /// Retrieves a single webpage document by its unique ID, or returns null if not found.
    /// </summary>
    /// <param name="id">The <see cref="IWebpageDocument.Id"/> to look up.</param>
    /// <returns>The matching webpage document, or null if not found.</returns>
    IWebpageDocument? GetById(Guid id);

    /// <summary>
    /// Adds a new webpage document to the repository.
    /// </summary>
    /// <param name="document">The webpage document to add.</param>
    void Add(IWebpageDocument document);

    /// <summary>
    /// Updates an existing webpage document in the repository.
    /// </summary>
    /// <param name="document">The webpage document with updated data.</param>
    void Update(IWebpageDocument document);

    /// <summary>
    /// Removes the specified webpage document from the repository.
    /// </summary>
    /// <param name="document">The webpage document to remove.</param>
    void Remove(IWebpageDocument document);
}