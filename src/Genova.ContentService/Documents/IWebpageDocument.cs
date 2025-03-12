namespace Genova.ContentService.Documents;

public interface IWebpageDocument : IDocument
{
    Guid? TemplateId { get; }
}
