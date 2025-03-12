using Genova.ContentService.Fields;

namespace Genova.ContentService.Components;

public class MetadataComponent : Component
{
    public override string ComponentType => "MetadataComponent";

    public MetadataComponent()
    {
        // Title field (text)
        var titleField = new TextField();
        titleField.SetKey("title");
        AddField(titleField);

        // Description field (text).
        // We do not require it to be non-empty.
        var descriptionField = new TextField();
        descriptionField.SetKey("description");
        AddField(descriptionField);
    }

    public override IEnumerable<string> Validate(ValidationMode validationMode)
    {
        // Always do the base validations (e.g. ensuring Key is set, child validations).
        foreach (var error in base.Validate(validationMode))
            yield return error;

        // If we are in "content" mode, we apply stricter checks (like requiring a title).
        if (validationMode == ValidationMode.Content)
        {
            var titleField = this.Fields.SingleOrDefault(f => f.Key == "title");
            if (titleField != null && string.IsNullOrEmpty(titleField.GetValue()))
            {
                yield return "Title cannot be empty in MetadataComponent.";
            }
        }
    }

}