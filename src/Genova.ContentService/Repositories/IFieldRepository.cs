using Genova.ContentService.Fields;

namespace Genova.ContentService.Repositories;

/// <summary>
/// A repository for creating or retrieving field objects (TextField, ImageField, etc.)
/// based on their string type names. In many cases, this acts similarly to an
/// abstract factory, but is organized as a repository for consistency and future
/// extensibility (e.g., storing field definitions in a data store).
/// </summary>
public interface IFieldRepository
{
    /// <summary>
    /// Returns an instance of an <see cref="IField"/> matching the given
    /// type name (e.g., "TextField", "ImageField"). If no match is found,
    /// null may be returned or an exception may be thrown, depending
    /// on your business rules.
    /// </summary>
    /// <param name="typeName">The identifier for the field type.</param>
    /// <returns>
    /// A concrete implementation of <see cref="IField"/>, or null if the
    /// type name is not recognized.
    /// </returns>
    IField? GetByType(string typeName);
}