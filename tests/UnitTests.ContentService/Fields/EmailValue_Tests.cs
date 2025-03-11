using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class EmailValue_Tests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name+tag@example-domain.net")]
    [InlineData("u.s.e.r@example")]
    [InlineData("USERNAME@EXAMPLE.COM")]
    [InlineData("digits123@domain456.org")]
    [InlineData("mixed-chars_+@domain.co")]
    public void Create_with_valid_email_succeeds(string input)
    {
        var emailValue = EmailValue.Create(input);

        Assert.Equal(input, emailValue.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_with_empty_string_throws_ArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => EmailValue.Create(input));
    }

    [Theory]
    [InlineData("  user@example.com  ")]
    [InlineData("\tuser@example.com\n")]
    public void Create_trims_input(string input)
    {
        var emailValue = EmailValue.Create(input);

        Assert.False(emailValue.Value.StartsWith(' ') || emailValue.Value.EndsWith(' '));
        Assert.False(emailValue.Value.Contains('\t') || emailValue.Value.Contains('\n'));
    }

    [Theory]
    [InlineData("userexample.com")]   // no '@'
    [InlineData("user@@example.com")] // two '@'
    public void Create_with_invalid_at_symbols_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => EmailValue.Create(input));
    }

    [Theory]
    [InlineData("@example.com")]  // empty local-part
    [InlineData("user@")]         // empty domain-part
    public void Create_with_empty_local_or_domain_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => EmailValue.Create(input));
    }

    [Theory]
    [InlineData("user @example.com")]
    [InlineData("user @ example . com")]
    [InlineData("user@example .com")]
    public void Create_with_whitespace_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => EmailValue.Create(input));
    }

    [Theory]
    [InlineData("user<test>@example.com")]
    [InlineData("user(example)@example.com")]
    [InlineData("john;doe@example.com")]
    [InlineData("john/doe@example.com")]
    [InlineData("john\"special\"@example.com")]
    public void Create_with_disallowed_characters_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => EmailValue.Create(input));
    }
}
