using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class UrlValue_Tests
{
    [Theory]
    [InlineData("path")]               // relative
    [InlineData("../path")]            // relative with ..
    [InlineData("/path")]              // root-relative
    [InlineData("//example.com/path")] // protocol-relative
    [InlineData("http://example.com/path")]
    [InlineData("https://example.com/with?query=param")]
    [InlineData("unknown://example.com/path")]  // no protocol check
    [InlineData("mailto:someone@example.com")]  // unusual protocols allowed
    [InlineData("/some/path#section")]
    [InlineData("//i.dont.think.this.is.valid/path")] // domain not validated
    [InlineData("scheme://some.place/complex?query=param#anchor")]
    [InlineData("ftp://example.com")]   // ftp, ftps, etc. are all allowed
    [InlineData("my-custom-protocol://whatever")]
    public void Create_with_valid_input_succeeds(string input)
    {
        // Arrange & Act
        var urlValue = UrlValue.Create(input);

        // Assert
        Assert.Equal(input, urlValue.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_with_empty_or_whitespace_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => UrlValue.Create(input));
    }

    [Theory]
    [InlineData("  /some/path  ")]  // whitespace around => trimmed => "/some/path"
    [InlineData("\t../stuff\n")]
    public void Create_trims_input(string input)
    {
        // Act
        var urlValue = UrlValue.Create(input);

        // we confirm that the Value is trimmed
        Assert.False(urlValue.Value.StartsWith(' ') || urlValue.Value.EndsWith(' '));
        Assert.False(urlValue.Value.Contains('\t') || urlValue.Value.Contains('\n'));
    }

    [Theory]
    [InlineData("://example.com")]  // must not start with ://
    [InlineData("http://some://weird")]
    public void Create_with_two_sets_of_colon_slash_slash_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => UrlValue.Create(input));
    }

    [Theory]
    [InlineData("  path with space")]
    [InlineData("/some path")]
    [InlineData("/some\tpath")]
    public void Create_with_internal_whitespace_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => UrlValue.Create(input));
    }

    [Theory]
    [InlineData("path?one=1?two=2")]
    [InlineData("/root#anchor#other")]
    public void Create_with_multiple_question_or_hash_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => UrlValue.Create(input));
    }

    [Theory]
    [InlineData("://")]
    [InlineData("http://abc://def")]
    [InlineData("//abc.com??query")]
    public void Create_with_structurally_invalid_colon_slash_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => UrlValue.Create(input));
    }

    [Theory]
    [InlineData("<script>")]
    [InlineData("some|weird|chars")]
    [InlineData("back\\slash")]
    [InlineData("some{something}")]
    [InlineData("some}")]
    public void Create_with_invalid_characters_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => UrlValue.Create(input));
    }

    [Fact]
    public void Create_does_not_validate_domain_or_protocol()
    {
        // Even an obviously weird domain or protocol is accepted
        var url = "strange-scheme://not-real_domain??";
        // This actually has two '?' so we expect that to fail with the multiple-question rule
        Assert.Throws<ArgumentException>(() => UrlValue.Create(url));

        // Let's do a single '?' version:
        var noDomainCheck = "strange-scheme://not-real_domain?param=1";
        var obj = UrlValue.Create(noDomainCheck);

        Assert.Equal(noDomainCheck, obj.Value);
    }
}
