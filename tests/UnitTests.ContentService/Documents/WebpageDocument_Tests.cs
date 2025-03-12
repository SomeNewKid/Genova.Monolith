using Genova.ContentService.Documents;

namespace UnitTests.ContentService.Documents;

public class WebpageDocument_Tests
{
    [Fact]
    public void By_default_Id_is_empty_and_TemplateId_is_null()
    {
        // Arrange
        var webpage = new WebpageDocument();

        // Act
        var docId = webpage.Id;
        var templateId = webpage.TemplateId;

        // Assert
        Assert.Equal(Guid.Empty, docId);
        Assert.Null(templateId);
    }

    [Fact]
    public void SetId_only_allows_assignment_once()
    {
        // Arrange
        var webpage = new WebpageDocument();
        var firstGuid = Guid.NewGuid();
        var secondGuid = Guid.NewGuid();

        // Act
        webpage.SetId(firstGuid);

        // Assert
        Assert.Equal(firstGuid, webpage.Id);

        // Attempting to overwrite should fail
        Assert.Throws<InvalidOperationException>(() => webpage.SetId(secondGuid));
    }

    [Fact]
    public void Can_assign_TemplateId_multiple_times()
    {
        // Arrange
        var webpage = new WebpageDocument();

        // Act
        var templateGuid1 = Guid.NewGuid();
        webpage.TemplateId = templateGuid1;

        // Assert
        Assert.Equal(templateGuid1, webpage.TemplateId);

        // Changing the TemplateId is allowed
        var templateGuid2 = Guid.NewGuid();
        webpage.TemplateId = templateGuid2;
        Assert.Equal(templateGuid2, webpage.TemplateId);
    }

    [Fact]
    public void Content_dictionary_is_empty_initially()
    {
        // Arrange
        var webpage = new WebpageDocument();

        // Act
        var contentDict = webpage.Content;

        // Assert
        Assert.Empty(contentDict);
    }

    [Fact]
    public void Can_add_to_Content_dictionary()
    {
        // Arrange
        var webpage = new WebpageDocument();

        // Act
        webpage.Content["article.title"] = "Hello World";
        webpage.Content["article.body"] = "Some body text...";

        // Assert
        Assert.Equal("Hello World", webpage.Content["article.title"]);
        Assert.Equal("Some body text...", webpage.Content["article.body"]);
    }

    [Fact]
    public void Base_Values_dictionary_is_accessible_for_general_metadata()
    {
        // Arrange
        var webpage = new WebpageDocument();

        // Act
        webpage.Values["owner"] = "admin";
        webpage.Values["published"] = "true";

        // Assert
        Assert.Equal("admin", webpage.Values["owner"]);
        Assert.Equal("true", webpage.Values["published"]);
    }

    [Fact]
    public void Validate_returns_error_if_Id_is_empty()
    {
        // Arrange
        var webpage = new WebpageDocument();
        // We do not call SetId, leaving Id = Guid.Empty
        // TemplateId is also null by default

        // Act
        var errors = webpage.Validate().ToArray();

        // Assert
        // Expect at least 2 errors: one for missing document Id, one for missing TemplateId
        Assert.True(errors.Length >= 2);
        Assert.Contains(errors, e => e == "Document has no assigned ID.");
        Assert.Contains(errors, e => e == "WebpageDocument requires a non-empty TemplateId.");
    }

    [Fact]
    public void Validate_returns_error_if_TemplateId_is_null_even_if_Id_is_set()
    {
        // Arrange
        var webpage = new WebpageDocument();
        webpage.SetId(Guid.NewGuid());
        // TemplateId remains null

        // Act
        var errors = webpage.Validate().ToList();

        // Assert
        // We only expect the template error now, because we do have an Id
        Assert.Single(errors);
        Assert.Contains("WebpageDocument requires a non-empty TemplateId.", errors[0]);
    }

    [Fact]
    public void Validate_returns_error_if_TemplateId_is_EmptyGuid()
    {
        // Arrange
        var webpage = new WebpageDocument();
        webpage.SetId(Guid.NewGuid());
        webpage.TemplateId = Guid.Empty;  // explicitly set to empty

        // Act
        var errors = webpage.Validate().ToArray();

        // Assert
        // Expect exactly one error about TemplateId
        Assert.Single(errors);
        Assert.Contains("WebpageDocument requires a non-empty TemplateId.", errors[0]);
    }

    [Fact]
    public void Validate_no_errors_if_Id_and_TemplateId_are_both_populated()
    {
        // Arrange
        var webpage = new WebpageDocument();
        webpage.SetId(Guid.NewGuid());
        webpage.TemplateId = Guid.NewGuid(); // a non-empty, valid Guid

        // Act
        var errors = webpage.Validate().ToList();

        // Assert
        Assert.Empty(errors);
    }
}
