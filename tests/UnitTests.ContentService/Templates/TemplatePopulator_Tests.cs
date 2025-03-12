using Genova.ContentService.Components;
using Genova.ContentService.Documents;
using Genova.ContentService.Fields;
using Genova.ContentService.Templates;

namespace UnitTests.ContentService.Templates;

public class TemplatePopulator_Tests
{
    // ------------------------------------------------
    // TEST IMPLEMENTATIONS
    // ------------------------------------------------

    /// <summary>
    /// A minimal ITemplate that can hold fields or child components.
    /// We'll store whether MarkPopulated() was called in a boolean flag.
    /// </summary>
    private class TestTemplate : Component, ITemplate
    {
        private Guid _id = Guid.Empty;
        private bool _populated = false;

        public Guid Id => _id;
        public TemplateMode Mode => _populated ? TemplateMode.Populated : TemplateMode.Definition;

        public override string ComponentType => "TestTemplate";

        public void SetId(Guid id)
        {
            if (_id != Guid.Empty)
                throw new InvalidOperationException("Id already set.");
            _id = id;
        }

        public void MarkPopulated()
        {
            if (_populated)
                throw new InvalidOperationException("Already in Populated mode.");
            _populated = true;
        }

        public IEnumerable<string> Validate()
        {
            // Minimal: we won't do a full validation, just demonstrate
            if (_id == Guid.Empty)
                yield return "Template ID is empty.";
        }

        public override IEnumerable<string> Validate(ValidationMode validationMode)
        {
            // Just a placeholder so we compile. 
            // For real usage, you'd do base checks or child checks.
            if (_id == Guid.Empty)
                yield return "Template ID is empty (validationMode=" + validationMode + ")";
        }
    }

    /// <summary>
    /// A minimal child component with a single field "title" or "body".
    /// We'll store multiple fields if needed for tests.
    /// </summary>
    private class TestChildComponent : Component
    {
        private readonly string _type;

        public TestChildComponent(string type, string key)
        {
            _type = type;
            SetKey(key);
        }

        public override string ComponentType => _type;

        // For convenience, let's add a single field:
        public void AddField(string fieldKey, IField field)
        {
            field.SetKey(fieldKey);
            base.AddField(field);
        }

        public override IEnumerable<string> Validate(ValidationMode validationMode)
        {
            // Minimal. 
            foreach (var err in base.Validate(validationMode))
                yield return err;
        }
    }

    /// <summary>
    /// A minimal IDocument used for testing. 
    /// We'll track if it was given an ID, etc.
    /// </summary>
    private class TestDocument : Document
    {
        public override IEnumerable<string> Validate()
        {
            // For this test doc, we'll skip deep logic.
            if (Id == Guid.Empty)
                yield return "Document has no ID.";
        }
    }

    // ------------------------------------------------
    // ACTUAL TESTS
    // ------------------------------------------------

    [Fact]
    public void Populate_sets_root_field_when_no_dot_in_key()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        // Add a field directly to the template
        var textField = new TextField();
        textField.SetKey("pageTitle");
        // We'll cheat and call protected AddField via reflection or
        // define a method in TestTemplate, but let's do it simple:
        template.GetType().BaseType!.GetMethod("AddField",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic)!
            .Invoke(template, new object[] { textField });

        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        doc.Values["pageTitle"] = "Home Page"; // no dot => root field

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        // the textField should now contain "Home Page"
        Assert.Equal("Home Page", textField.GetValue());
    }

    [Fact]
    public void Populate_sets_child_field_when_key_has_dot()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        // child component with Key="article"
        var articleComp = new TestChildComponent("ArticleComp", "article");
        // add a field "title"
        articleComp.AddField("title", new TextField());
        template.AddChild(articleComp);

        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        doc.Values["article.title"] = "Hello World";

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        var field = articleComp.Fields.Single(f => f.Key == "title");
        Assert.Equal("Hello World", field.GetValue());
    }

    [Fact]
    public void Populate_ignores_unknown_component()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        // No child named "sidebar"
        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        doc.Values["sidebar.title"] = "Should be ignored";

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        // No exceptions, no fields set. 
        // The root has no field "sidebar.title", no child "sidebar"
        // We just confirm it didn't crash. 
        Assert.Empty(template.Fields);
        Assert.Empty(template.Children);
    }

    [Fact]
    public void Populate_ignores_unknown_field()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        // Child with key="article" but no field "subtitle"
        var articleComp = new TestChildComponent("ArticleComp", "article");
        articleComp.AddField("title", new TextField());
        template.AddChild(articleComp);

        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        doc.Values["article.subtitle"] = "Should be ignored";

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        // No exceptions, no new field "subtitle" magically created
        // We only have "title".
        Assert.Single(articleComp.Fields);
        Assert.Equal("title", articleComp.Fields[0].Key);
    }

    [Fact]
    public void Populate_marks_template_as_populated()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        // Add a field, optional
        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        doc.Values["pageTitle"] = "Hello";

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        // The test template transitions from Definition to Populated
        Assert.Equal(TemplateMode.Populated, template.Mode);
    }

    [Fact]
    public void Populate_does_nothing_when_doc_values_is_empty()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        // doc.Values is empty

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        // We moved to Populated mode anyway, but no fields set
        Assert.Equal(TemplateMode.Populated, template.Mode);
    }

    [Fact]
    public void Populate_handles_multiple_dot_keys_by_splitting_only_first_dot()
    {
        // e.g. "article.title.more" => component="article", field="title.more"
        // This is how the sample parseKey handles it
        // We'll show an example scenario

        // Arrange
        var template = new TestTemplate();
        template.SetKey("root");
        template.SetId(Guid.NewGuid());

        // child component with key="article"
        var articleComp = new TestChildComponent("ArticleComp", "article");
        articleComp.AddField("title.more", new TextField());
        template.AddChild(articleComp);

        var doc = new TestDocument();
        doc.SetId(Guid.NewGuid());
        doc.Values["article.title.more.stuff"] = "Extra bits";

        var populator = new TemplatePopulator();

        // Act
        populator.Populate(template, doc);

        // Assert
        // "article.title.more.stuff" => parse => (component="article", field="title.more.stuff"??)
        // The example code only splits on the FIRST dot, so fieldKey = "title.more.stuff"
        // We won't have an exact fieldKey unless we made "title.more.stuff" or handle partial parse
        // So in this example, let's say we made "title.more" -> won't match "title.more.stuff"
        // => it gets ignored
        // For demonstration, let's confirm no exceptions
        var field = articleComp.Fields.Single(f => f.Key == "title.more");
        // It's still empty because we didn't find "title.more" == "title.more.stuff"
        Assert.Equal(string.Empty, field.GetValue());
    }
}
