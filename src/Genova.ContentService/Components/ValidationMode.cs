namespace Genova.ContentService.Components;

/// <summary>
/// Specifies how a component or template should be validated.
/// </summary>
public enum ValidationMode
{
    /// <summary>
    /// Validation checks only structure-level rules (e.g., component keys, presence of fields).
    /// This is the default mode.
    /// </summary>
    Definition = 0,

    /// <summary>
    /// Validation includes content-level checks (e.g., required fields are non-empty).
    /// Typically used when the component/template is populated with final data.
    /// </summary>
    Content = 1
}