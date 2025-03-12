using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class EmailField_Tests
{
    [Fact]
    public void FieldType_returns_Email()
    {
        // Arrange
        var field = new EmailField();

        // Act
        var fieldType = field.FieldType;

        // Assert
        Assert.Equal("Email", fieldType);
    }

    [Fact]
    public void GetValue_returns_empty_if_not_set()
    {
        // Arrange
        var field = new EmailField();

        // Act
        var result = field.GetValue();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("someone@example.com", "someone@example.com")]
    [InlineData("USER@EXAMPLE.COM", "USER@EXAMPLE.COM")]
    [InlineData("  user.name+tag@sub.domain.co.uk  ", "user.name+tag@sub.domain.co.uk")]
    [InlineData("john.smith@example.org", "john.smith@example.org")]
    public void SetValue_with_valid_email_stores_it(string input, string expected)
    {
        // Arrange
        var field = new EmailField();

        // Act
        field.SetValue(input);
        var actual = field.GetValue();

        // Assert
        // The final stored email should match the normalized address from EmailValue
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetValue_with_null_or_empty_clears_the_email(string? input)
    {
        // Arrange
        var field = new EmailField();
        field.SetValue("initial@domain.com");

        // Act
        field.SetValue(input);
        var result = field.GetValue();

        // Assert
        // We allow an empty or null input => no email set
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("invalid-email")]           // no '@'
    [InlineData("some@@somewhere.com")]     // double '@'
    [InlineData("some@some where.com")]     // whitespace in domain
    [InlineData("@no-local-part.com")]      // empty local part
    [InlineData("local@.domain.com")]       // empty domain-part chunk
    [InlineData("john@@domain.com")]        // 2 '@' chars
    [InlineData("some@<bad-chars>.com")]
    public void SetValue_with_invalid_strings_throws_ArgumentException(string input)
    {
        // Arrange
        var field = new EmailField();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => field.SetValue(input));
    }
}