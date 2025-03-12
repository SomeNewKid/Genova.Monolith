using Genova.ContentService.Fields;

namespace UnitTests.ContentService.Fields;

public class ListField_Tests
{
    [Fact]
    public void FieldType_returns_List()
    {
        // Arrange
        var listField = new ListField();

        // Act
        var fieldType = listField.FieldType;

        // Assert
        Assert.Equal("List", fieldType);
    }

    [Fact]
    public void Values_is_empty_by_default()
    {
        // Arrange
        var listField = new ListField();

        // Act
        var values = listField.Values;
        var rawValue = listField.GetValue();

        // Assert
        Assert.Empty(values);
        Assert.Equal(string.Empty, rawValue);
    }

    [Theory]
    [InlineData("tag1, tag2, tag3", 3)]
    [InlineData("  tag1 ,tag2,tag3 ", 3)]
    [InlineData("tag1, tag2, tag1", 2)] // duplicates removed
    [InlineData("", 0)]
    [InlineData(null, 0)]
    public void SetValue_parses_comma_separated_strings_and_stores_them_in_unordered_set(string? input, int expectedCount)
    {
        // Arrange
        var listField = new ListField();

        // Act
        listField.SetValue(input);

        // Assert
        Assert.Equal(expectedCount, listField.Values.Count);
    }

    [Theory]
    [InlineData("tag1, tag2, tag3")]
    [InlineData("tag1, tag1, tag2, tag3")]
    [InlineData("alpha, beta, gamma")]
    public void GetValue_joins_the_values_in_unspecified_order(string input)
    {
        // Arrange
        var listField = new ListField();

        // Act
        listField.SetValue(input);
        var rawString = listField.GetValue();

        // Assert
        // Because we use a HashSet, the exact order is not predictable;
        // we only verify that all distinct items are present in the output.
        var distinctItems = input.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                 .Select(t => t.Trim())
                                 .Distinct()
                                 .ToArray();

        // The joined string should contain each item, but might use a different order or comma spacing.
        foreach (var item in distinctItems)
        {
            Assert.Contains(item, rawString);
        }

        // Additionally, we can verify we have the expected count of items in Values.
        Assert.Equal(distinctItems.Length, listField.Values.Count);
    }

    [Fact]
    public void SetValue_clears_previous_values()
    {
        // Arrange
        var listField = new ListField();
        listField.SetValue("A, B, C");

        // Act
        listField.SetValue("X, Y");

        // Assert
        Assert.Equal(2, listField.Values.Count);
        Assert.Contains("X", listField.Values);
        Assert.Contains("Y", listField.Values);
        Assert.DoesNotContain("A", listField.Values);
    }
}