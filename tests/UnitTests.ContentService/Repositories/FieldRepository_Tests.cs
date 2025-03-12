using System;
using Xunit;
using Genova.ContentService.Fields;
using Genova.ContentService.Repositories;

namespace UnitTests.ContentService.Repositories;

public class FieldRepository_Tests
{
    private readonly IFieldRepository _fieldRepo;

    public FieldRepository_Tests()
    {
        // Create an instance of your FieldRepository implementation
        _fieldRepo = new FieldRepository();
    }

    // We can use a [Theory] + [InlineData] approach to test multiple known types.
    [Theory]
    [InlineData("BooleanField", typeof(BooleanField))]
    [InlineData("DateField", typeof(DateField))]
    [InlineData("DateTimeField", typeof(DateTimeField))]
    [InlineData("TimeField", typeof(TimeField))]
    [InlineData("HtmlField", typeof(HtmlField))]
    [InlineData("ImageField", typeof(ImageField))]
    [InlineData("IntegerField", typeof(IntegerField))]
    [InlineData("ListField", typeof(ListField))]
    [InlineData("MarkdownField", typeof(MarkdownField))]
    [InlineData("NumberField", typeof(NumberField))]
    [InlineData("PhoneField", typeof(PhoneField))]
    [InlineData("TextField", typeof(TextField))]
    [InlineData("TimeSpanField", typeof(TimeSpanField))]
    [InlineData("UrlField", typeof(UrlField))]
    [InlineData("EmailField", typeof(EmailField))]
    public void GetByType_returns_correct_concrete_field(string typeName, Type expectedType)
    {
        // Act
        var field = _fieldRepo.GetByType(typeName);

        // Assert
        Assert.NotNull(field);
        Assert.IsType(expectedType, field);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("UnknownField")]
    [InlineData("NotARealField")]
    public void GetByType_returns_null_for_unrecognized_or_empty_types(string? typeName)
    {
        // Act
        var field = _fieldRepo.GetByType(typeName);

        // Assert
        Assert.Null(field);
    }

    [Fact]
    public void GetByType_is_case_insensitive()
    {
        // Act
        var fieldLowercase = _fieldRepo.GetByType("textfield");
        var fieldMixedCase = _fieldRepo.GetByType("TeXtFiElD");

        // Assert
        Assert.NotNull(fieldLowercase);
        Assert.IsType<TextField>(fieldLowercase);

        Assert.NotNull(fieldMixedCase);
        Assert.IsType<TextField>(fieldMixedCase);
    }
}