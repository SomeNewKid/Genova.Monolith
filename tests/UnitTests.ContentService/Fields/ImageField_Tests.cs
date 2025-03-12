using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class ImageField_Tests
{
    [Fact]
    public void FieldType_is_Document()
    {
        // Arrange
        var imageField = new ImageField();

        // Act
        var fieldType = imageField.FieldType;

        // Assert
        Assert.Equal("Document", fieldType);
    }

    [Fact]
    public void DocumentType_is_Image()
    {
        // Arrange
        var imageField = new ImageField();

        // Act
        var docType = imageField.DocumentType;

        // Assert
        Assert.Equal("Image", docType);
    }

    [Fact]
    public void SetValue_stores_the_document_id()
    {
        // Arrange
        var imageField = new ImageField();
        const string docId = "ABC-123-GUID";

        // Act
        imageField.SetValue(docId);

        // Assert
        Assert.Equal(docId, imageField.GetValue());
    }

    [Fact]
    public void GetValue_returns_empty_if_not_set()
    {
        // Arrange
        var imageField = new ImageField();

        // Act
        var value = imageField.GetValue();

        // Assert
        Assert.Equal(string.Empty, value);
    }

    [Fact]
    public void SetKey_assigns_the_field_key()
    {
        // Arrange
        var imageField = new ImageField();

        // Act
        imageField.SetKey("featuredImage");

        // Assert
        Assert.Equal("featuredImage", imageField.Key);
    }
}
