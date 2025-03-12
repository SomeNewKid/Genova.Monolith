using Genova.ContentService.Fields;

namespace Genova.ContentService.Components;

public class ArticleComponent : Component
{
    public override string ComponentType => "ArticleComponent";

    public ArticleComponent()
    {
        // Title: text field
        var titleField = new TextField();
        titleField.SetKey("title");
        AddField(titleField);

        // Summary: markdown field
        var summaryField = new MarkdownField();
        summaryField.SetKey("summary");
        AddField(summaryField);

        // Content: markdown field
        var contentField = new MarkdownField();
        contentField.SetKey("content");
        AddField(contentField);

        // Image: image field
        var imageField = new ImageField();
        imageField.SetKey("image");
        AddField(imageField);
    }

    public override IEnumerable<string> Validate(ValidationMode validationMode)
    {
        foreach (var error in base.Validate(validationMode))
            yield return error;

        if (validationMode == ValidationMode.Content)
        {
            var titleField = this.Fields.SingleOrDefault(f => f.Key == "title");
            if (titleField != null && string.IsNullOrEmpty(titleField.GetValue()))
            {
                yield return "Title cannot be empty.";
            }
        }
    }

}
