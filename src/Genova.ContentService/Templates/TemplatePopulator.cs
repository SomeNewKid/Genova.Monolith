using Genova.ContentService.Components;
using Genova.ContentService.Documents;

namespace Genova.ContentService.Templates;

/// <summary>
/// Populates an ITemplate with data from an IDocument’s Values dictionary.
/// This version ensures a "MetadataComponent" with key "__metadata"
/// exists in the template for fallback assignment when no dot is found.
/// </summary>
public class TemplatePopulator
{
    public void Populate(ITemplate template, IDocument doc)
    {
        // 1) Ensure we have a MetadataComponent with key="__metadata"
        var metadataComp = FindOrCreateMetadataComponent(template, "__metadata");

        // 2) For each KV in doc.Values, parse and set the appropriate field.
        foreach (var kvp in doc.Values)
        {
            var (componentKey, fieldKey) = ParseKey(kvp.Key);
            var rawValue = kvp.Value;

            if (string.IsNullOrEmpty(componentKey))
            {
                // No dot => root-level field or fallback to metadata
                // Attempt to find in the root template first
                var field = template.Fields
                    .SingleOrDefault(f => f.Key.Equals(fieldKey, StringComparison.OrdinalIgnoreCase));

                if (field != null)
                {
                    field.SetValue(rawValue);
                }
                else
                {
                    // fallback to metadata component
                    var metaField = metadataComp.Fields
                        .SingleOrDefault(f => f.Key.Equals(fieldKey, StringComparison.OrdinalIgnoreCase));
                    if (metaField != null)
                    {
                        metaField.SetValue(rawValue);
                    }
                    else
                    {
                        // If not found, we skip or log a warning
                        continue;
                    }
                }
            }
            else
            {
                // we have componentKey + fieldKey
                IComponent? targetComp = FindComponent(template, componentKey);
                if (targetComp == null)
                {
                    // skip if not found
                    continue;
                }

                var field = targetComp.Fields
                    .SingleOrDefault(f => f.Key.Equals(fieldKey, StringComparison.OrdinalIgnoreCase));

                if (field == null)
                {
                    // skip if not found
                    continue;
                }

                field.SetValue(rawValue);
            }
        }

        // 3) Once complete, mark the template as populated
        template.MarkPopulated();
    }

    /// <summary>
    /// Splits a dictionary key into (componentKey, fieldKey).
    /// If no dot is found, returns (null, key).
    /// If exactly one dot, returns (leftPart, rightPart).
    /// If multiple dots, only the first dot is used in this example.
    /// </summary>
    private (string? componentKey, string fieldKey) ParseKey(string rawKey)
    {
        if (string.IsNullOrEmpty(rawKey))
            return (null, string.Empty);

        var idx = rawKey.IndexOf('.');
        if (idx < 0)
        {
            // no dot => treat entire key as a field on the root template
            return (null, rawKey);
        }

        var left = rawKey.Substring(0, idx);
        var right = rawKey.Substring(idx + 1);
        return (left, right);
    }

    /// <summary>
    /// Recursively searches the template’s child components for one
    /// whose Key matches 'componentKey' (case-insensitive). Returns null if not found.
    /// Includes the template root if it matches.
    /// </summary>
    private IComponent? FindComponent(IComponent root, string componentKey)
    {
        // if root matches
        if (root.Key.Equals(componentKey, StringComparison.OrdinalIgnoreCase))
            return root;

        // search children
        foreach (var child in root.Children)
        {
            var match = FindComponent(child, componentKey);
            if (match != null)
                return match;
        }

        return null;
    }

    /// <summary>
    /// Finds or creates a MetadataComponent with the specified key (e.g., "__metadata")
    /// as a child of the root template, if it doesn’t already exist.
    /// </summary>
    private IComponent FindOrCreateMetadataComponent(ITemplate template, string metadataKey)
    {
        // Check if there's an existing child with that key
        var existing = FindComponent(template, metadataKey);
        if (existing != null)
            return existing;

        // else create a new MetadataComponent
        var meta = new MetadataComponent();
        meta.SetKey(metadataKey);
        template.AddChild(meta);

        return meta;
    }
}
