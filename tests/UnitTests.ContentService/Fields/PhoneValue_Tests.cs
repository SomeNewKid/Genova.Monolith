using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class PhoneValue_Tests
{
    [Theory]
    [InlineData("131456", "131456")]            // all digits
    [InlineData("93321234", "93321234")]        // all digits
    [InlineData("08 9332 1234", "0893321234")]  // remove spaces
    [InlineData("(08) 9332 1234", "0893321234")]// remove parentheses, spaces
    [InlineData("(08) 9332-1234", "0893321234")]// remove parentheses, spaces, hyphen
    [InlineData("+61 8 9332-1234", "+61893321234")] // leading '+', rest are digits
    public void Create_with_valid_formats_succeeds(string input, string expected)
    {
        // Act
        var phoneValue = PhoneValue.Create(input);

        // Assert
        Assert.Equal(expected, phoneValue.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" - ")]
    [InlineData("()")]
    public void Create_with_empty_after_cleaning_throws(string input)
    {
        // All these examples would become "" after removing parentheses/hyphens/spaces
        Assert.Throws<ArgumentException>(() => PhoneValue.Create(input));
    }

    [Theory]
    [InlineData("12345")]               // 5 digits => too few
    [InlineData("123456789012345678901")] // 21 digits => too many
    public void Create_with_out_of_range_digit_count_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => PhoneValue.Create(input));
    }

    [Theory]
    [InlineData("08+9332")]       // plus sign not at first char
    [InlineData("089+3321234")]   // plus sign not at first char
    public void Create_with_plus_not_in_first_position_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => PhoneValue.Create(input));
    }

    [Theory]
    [InlineData("+1(8)abc234")]   // after cleaning => +18abc234 => 'a' is invalid
    [InlineData("08933x1234")]    // 'x' is invalid
    [InlineData("08##9332")]      // '#' is invalid
    [InlineData("12.34")]         // '.' is invalid
    public void Create_with_invalid_characters_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => PhoneValue.Create(input));
    }

    [Theory]
    [InlineData("+")]             // just a plus sign => 0 digits => fail
    [InlineData("+12345")]        // plus sign => only 5 digits => fail
    [InlineData("+123456789012345678901")] // plus + 21 digits => fail
    public void Create_with_plus_but_invalid_digit_count_throws(string input)
    {
        Assert.Throws<ArgumentException>(() => PhoneValue.Create(input));
    }

    [Fact]
    public void Create_with_minimum_and_maximum_digit_counts()
    {
        // 6 digits
        var minOk = PhoneValue.Create("123456");
        Assert.Equal("123456", minOk.Value);

        // 20 digits
        var maxOk = PhoneValue.Create("12345678901234567890");
        Assert.Equal("12345678901234567890", maxOk.Value);

        // Leading plus with 6 digits
        var minWithPlus = PhoneValue.Create("+123456");
        Assert.Equal("+123456", minWithPlus.Value);

        // Leading plus with 20 digits
        var maxWithPlus = PhoneValue.Create("+12345678901234567890");
        Assert.Equal("+12345678901234567890", maxWithPlus.Value);
    }
}
