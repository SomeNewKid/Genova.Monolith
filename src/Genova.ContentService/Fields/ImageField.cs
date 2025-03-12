namespace Genova.ContentService.Fields;

/// <summary>
/// A concrete field that references an "Image" type document.
/// </summary>
public class ImageField : DocumentField
{
    /// <summary>
    /// Indicates this field references an IDocument of type "Image".
    /// </summary>
    public override string DocumentType => "Image";
}