using Genova.ContentService.Components;

namespace Genova.ContentService.Templates;

public class Template : Component, ITemplate
{
    private Guid _id = Guid.Empty;

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
    /// Allows setting the template's unique Guid ID, if not already set.
    /// Throws an exception if the ID was previously assigned.
    /// </summary>
    /// <param name="id">The new Guid identifier.</param>
    public void SetId(Guid id)
    {
        if (_id != Guid.Empty)
            throw new InvalidOperationException("Id is already set; cannot overwrite it.");

        _id = id;
    }
}
