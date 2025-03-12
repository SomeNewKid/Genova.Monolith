using System;
using System.Linq;
using Xunit;
using Genova.ContentService.Documents;
using System.Collections.Generic;

namespace UnitTests.ContentService.Documents;

public class Document_Tests
{
    /// <summary>
    /// A test implementation of the abstract Document class,
    /// so we can instantiate and verify its behavior.
    /// </summary>
    private class TestDocument : Document
    {
        // If needed, override Validate() or leave as is:
        // public override IEnumerable<string> Validate()
        // {
        //     foreach (var e in base.Validate()) yield return e;
        //     // Additional custom checks...
        // }
    }

    class TestValidatedDocument : Document
    {
        public override IEnumerable<string> Validate()
        {
            // base check for ID
            foreach (var e in base.Validate()) yield return e;

            // custom rule: must contain 'title'
            if (!Values.ContainsKey("title"))
                yield return "CustomDoc must have a 'title' in Values.";
        }
    }

    [Fact]
    public void New_document_has_empty_Id_and_empty_Values()
    {
        // Arrange
        var doc = new TestDocument();

        // Act
        var docId = doc.Id;
        var valuesCount = doc.Values.Count;

        // Assert
        Assert.Equal(Guid.Empty, docId);
        Assert.Equal(0, valuesCount);
    }

    [Fact]
    public void SetId_only_allows_one_assignment()
    {
        // Arrange
        var doc = new TestDocument();
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        // Act
        doc.SetId(id1);

        // Assert
        Assert.Equal(id1, doc.Id);

        // Attempt to overwrite => exception
        Assert.Throws<InvalidOperationException>(() => doc.SetId(id2));
    }

    [Fact]
    public void Values_dictionary_can_be_modified_and_retrieved()
    {
        // Arrange
        var doc = new TestDocument();

        // Act
        doc.Values["title"] = "My Title";
        doc.Values["owner"] = "admin";

        // Assert
        Assert.Equal("My Title", doc.Values["title"]);
        Assert.Equal("admin", doc.Values["owner"]);
    }

    [Fact]
    public void Validate_returns_error_if_Id_is_empty()
    {
        // Arrange
        var doc = new TestDocument();
        // Id is still Guid.Empty by default

        // Act
        var errors = doc.Validate().ToArray();

        // Assert
        Assert.Single(errors);
        Assert.Equal("Document has no assigned ID.", errors[0]);
    }

    [Fact]
    public void Validate_no_errors_if_Id_is_set()
    {
        // Arrange
        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());

        // Act
        var errors = doc.Validate().ToList();

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Derived_class_can_extend_Validate_if_needed()
    {
        // Here we show an example of overriding Validate in a second derived class
        // that checks for a certain key in Values. We'll define it inline.

        var customDoc = new TestValidatedDocument();
        // No ID => triggers base error
        customDoc.Values["author"] = "someone";

        var errors1 = customDoc.Validate().ToList();
        Assert.Contains("Document has no assigned ID.", errors1);
        Assert.Contains("CustomDoc must have a 'title' in Values.", errors1);

        // Fix the issues
        customDoc.SetId(Guid.NewGuid());
        customDoc.Values["title"] = "Hello";

        var errors2 = customDoc.Validate().ToList();
        Assert.Empty(errors2);
    }
}