namespace Genova.ContentService.Templates;

/// <summary>
/// Specifies whether an ITemplate is operating in a "definition" mode (structure-only)
/// or a "populated" mode (with actual field data).
/// </summary>
public enum TemplateMode
{
    /// <summary>
    /// The template is a reusable definition. Fields/components might be empty or
    /// partially specified. Typically used for storing structural info in a data store.
    /// This is the default mode.
    /// </summary>
    Definition = 0,

    /// <summary>
    /// The template is fully populated with actual data (e.g., from a WebpageDocument).
    /// Used at runtime to render or generate final output.
    /// </summary>
    Populated = 1
}