using Genova.ContentService.Components;

namespace Genova.ContentService.Repositories;

/// <summary>
/// A repository for creating or retrieving component objects
/// (ArticleComponent, MetadataComponent, etc.) based on their
/// string <see cref="ComponentType"/> names. This interface
/// behaves similarly to an abstract factory, but is organized
/// as a repository for future extensibility (e.g., if components
/// are stored or configured externally).
/// </summary>
public interface IComponentRepository
{
    /// <summary>
    /// Returns an instance of an <see cref="IComponent"/> matching the
    /// given type name (e.g., "ArticleComponent"). If no match is found,
    /// null may be returned or an exception may be thrown, depending on
    /// your domain rules.
    /// </summary>
    /// <param name="componentType">The <see cref="IComponent.ComponentType"/> identifier.</param>
    /// <returns>
    /// A concrete implementation of <see cref="IComponent"/>, or null
    /// if the component type is not recognized.
    /// </returns>
    IComponent? GetByType(string componentType);
}