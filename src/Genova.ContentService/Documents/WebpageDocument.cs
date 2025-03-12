using System;
using System.Collections.Generic;

namespace Genova.ContentService.Documents;

public class WebpageDocument : Document
{
    private Guid? _templateId = null;

    /// <summary>
    /// Optional identifier referencing a template (root component).
    /// If null, there's no associated template.
    /// </summary>
    public Guid? TemplateId
    {
        get => _templateId;
        set => _templateId = value;
    }

    /// <summary>
    /// A dictionary storing this webpage's specific content,
    /// such as "article.title" => "Hello World",
    /// which is distinct from the base <see cref="Values"/> dictionary
    /// if you want to separate general metadata from page-specific fields.
    /// </summary>
    public Dictionary<string, string> Content { get; } = new();

    /// <summary>
    /// Validates that the document's ID is not empty and that TemplateId is non-null and non-empty.
    /// </summary>
    /// <returns>An IEnumerable of error messages. Empty if valid.</returns>
    public override IEnumerable<string> Validate()
    {
        // 1) Let the base Document do its checks (e.g., Id must not be Guid.Empty).
        foreach (var baseError in base.Validate())
        {
            yield return baseError;
        }

        // 2) Check TemplateId is non-null and not Guid.Empty.
        if (TemplateId == null || TemplateId.Value == Guid.Empty)
        {
            yield return "WebpageDocument requires a non-empty TemplateId.";
        }
    }
}