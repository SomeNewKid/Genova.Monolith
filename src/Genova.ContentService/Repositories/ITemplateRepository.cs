using System;
using System.Collections.Generic;

namespace Genova.ContentService.Templates;

/// <summary>
/// A repository interface for performing CRUD-like operations on ITemplate entities.
/// </summary>
public interface ITemplateRepository
{
    /// <summary>
    /// Returns all templates in the repository.
    /// </summary>
    /// <returns>An enumeration of all stored <see cref="ITemplate"/> objects.</returns>
    IEnumerable<ITemplate> GetAll();

    /// <summary>
    /// Retrieves a single template by its unique ID, or returns null if not found.
    /// </summary>
    /// <param name="id">The <see cref="ITemplate.Id"/> to look up.</param>
    /// <returns>The matching template, or null if not found.</returns>
    ITemplate? GetById(Guid id);

    /// <summary>
    /// Adds a new template to the repository.
    /// In many implementations, this is where the template is actually persisted.
    /// </summary>
    /// <param name="template">The template to add.</param>
    void Add(ITemplate template);

    /// <summary>
    /// Updates an existing template in the repository.
    /// Typically, the template must already have an assigned <see cref="ITemplate.Id"/>.
    /// </summary>
    /// <param name="template">The template with updated data.</param>
    void Update(ITemplate template);

    /// <summary>
    /// Removes the specified template from the repository.
    /// </summary>
    /// <param name="template">The template to remove.</param>
    void Remove(ITemplate template);
}