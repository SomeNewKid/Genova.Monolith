using Genova.ContentService.Components;

namespace Genova.ContentService.Templates;

public class Template : Component, ITemplate
{
    private Guid _id = Guid.Empty;
    private TemplateMode _mode = TemplateMode.Definition;

    /// <summary>
    /// A unique identifier for this template. 
    /// </summary>
    public Guid Id => _id;

    /// <summary>
    /// A string identifier that specifies the kind of component this is.
    /// For a generic template, we can simply say "Template".
    /// </summary>
    public override string ComponentType => "Template";

    /// <summary>
    /// The current mode (Definition or Populated).
    /// Defaults to Definition and can only switch to Populated once.
    /// </summary>
    public TemplateMode Mode => _mode;

    /// <summary>
    /// Allows setting the template's unique Guid ID, if not already set.
    /// Throws an exception if the ID was previously assigned.
    /// </summary>
    /// <param name="id">The new Guid identifier.</param>
    /// <exception cref="InvalidOperationException">If the ID was previously set.</exception>
    public void SetId(Guid id)
    {
        if (_id != Guid.Empty)
            throw new InvalidOperationException("Id is already set; cannot overwrite it.");

        _id = id;
    }

    /// <summary>
    /// Switches this template from Definition to Populated mode.
    /// Once in Populated mode, it cannot revert to Definition mode.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If the template is already in Populated mode.
    /// </exception>
    public void MarkPopulated()
    {
        if (_mode == TemplateMode.Populated)
        {
            throw new InvalidOperationException(
                "Template is already in Populated mode and cannot revert to Definition mode."
            );
        }

        _mode = TemplateMode.Populated;
    }

    /// <summary>
    /// Validates this template and all of its children.
    /// If Mode = Definition, we perform structure-only checks.
    /// If Mode = Populated, we perform content-level checks.
    /// Returns a collection of error messages. If the collection is empty,
    /// this template is valid.
    /// </summary>
    /// <returns>
    /// An IEnumerable of error messages. An empty collection implies no validation errors.
    /// </returns>
    public IEnumerable<string> Validate()
    {
        // Decide which ValidationMode to use, based on Mode
        var mode = (_mode == TemplateMode.Definition)
            ? ValidationMode.Definition
            : ValidationMode.Content;

        // Now call the base Validate(ValidationMode) method
        return this.Validate(mode);
    }

    /// <summary>
    /// Implementation for the required Validate(ValidationMode) from IComponent,
    /// so the base can do structural checks. If we need specialized logic for a template
    /// in definition vs. content mode, we can add it here.
    /// </summary>
    public override IEnumerable<string> Validate(ValidationMode validationMode)
    {
        // Possibly call base.Validate(validationMode) to do normal checks:
        foreach (var baseError in base.Validate(validationMode))
            yield return baseError;

        if (validationMode == ValidationMode.Definition)
        {
            if (_id == Guid.Empty)
            {
                yield return "Template must have a non-empty ID.";
            }
        }
    }
}
