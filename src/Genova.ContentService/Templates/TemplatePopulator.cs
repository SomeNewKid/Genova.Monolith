using Genova.ContentService.Components;
using Genova.ContentService.Documents;

namespace Genova.ContentService.Templates;

/// <summary>
/// Populates an ITemplate with data from an IDocument’s Values dictionary.
/// Each key in the dictionary is parsed to locate the target component
/// (by 'componentKey') and field (by 'fieldKey'), then SetValue is called
/// on the matching field. Finally, MarkPopulated is called on the template.
/// </summary>
public class TemplatePopulator
{
    /// <summary>
    /// Reads the doc.Values dictionary, for each entry:
    ///  - If the key has no dot, it references a field on the root template.
    ///  - If the key is like "component.field", it references a named child component's field.
    ///  - If the key has multiple dots, only the first dot is used (i.e. "component.field.more" is partial).
    /// Then sets the field's value. Skips missing components or fields silently (or throws if desired).
    /// Finally calls template.MarkPopulated().
    /// </summary>
    /// <param name="template">The root ITemplate to populate.</param>
    /// <param name="doc">The IDocument holding the key/value data.</param>
    public void Populate(ITemplate template, IDocument doc)
    {
        foreach (var kvp in doc.Values)
        {
            var key = kvp.Key;    // e.g. "article.title"
            var value = kvp.Value;

            // Parse the key to separate component vs. field
            var (componentKey, fieldKey) = ParseKey(key);

            // If componentKey is null => we set the field on the root template
            // else => we find the child component by that key
            IComponent targetComponent;
            if (string.IsNullOrEmpty(componentKey))
            {
                // It's a field of the template itself
                targetComponent = template;
            }
            else
            {
                // Find a child component (recursively) by name
                targetComponent = FindComponent(template, componentKey);
                if (targetComponent == null)
                {
                    // If you prefer to skip silently, remove the throw or log a warning
                    // For demonstration, let's just skip in this example.
                    continue;
                }
            }

            // Now find the field in targetComponent
            var field = targetComponent.Fields
                .SingleOrDefault(f => f.Key.Equals(fieldKey, StringComparison.OrdinalIgnoreCase));

            if (field == null)
            {
                // skip or throw
                continue;
            }

            // Set the field's value
            field.SetValue(value);
        }

        // Now that we’ve populated all known fields, switch the template to Populated mode
        template.MarkPopulated();
    }

    /// <summary>
    /// Splits a dictionary key into (componentKey, fieldKey).
    /// If no dot is found, returns (null, originalKey).
    /// If exactly one dot, returns (leftPart, rightPart).
    /// If multiple dots, only the first dot is used (rest is appended to the 'fieldKey' or ignored).
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

        // If you want to handle multiple dots, you can modify logic or parse further
        var left = rawKey.Substring(0, idx);          // e.g. "article"
        var right = rawKey.Substring(idx + 1);        // e.g. "title"
        return (left, right);
    }

    /// <summary>
    /// Recursively searches the template’s child components for a component
    /// whose Key matches 'componentKey' (case-insensitive). Returns null if not found.
    /// </summary>
    private IComponent? FindComponent(IComponent root, string componentKey)
    {
        // if root has the desired key, return root
        if (root.Key.Equals(componentKey, StringComparison.OrdinalIgnoreCase))
        {
            return root;
        }

        // else search children recursively
        foreach (var child in root.Children)
        {
            var match = FindComponent(child, componentKey);
            if (match != null)
            {
                return match;
            }
        }

        // no match
        return null;
    }
}
