using Genova.ContentService.Components;
using Genova.ContentService.Templates;

namespace UnitTests.ContentService.Components;

public class Component_Tests
{
    /// <summary>
    /// A basic parent component with ComponentType = "ParentType"
    /// (not an ITemplate).
    /// </summary>
    private class TestParentComponent : Component
    {
        public override string ComponentType => "ParentType";

        // Example: if Key == "BadParentKey", we produce an error
        public override IEnumerable<string> Validate()
        {
            foreach (var err in base.Validate())
                yield return err;

            if (Key == "BadParentKey")
                yield return "Parent has a forbidden key 'BadParentKey'.";
        }
    }

    /// <summary>
    /// A basic child component with a given component type, for testing AddChild scenarios.
    /// </summary>
    private class TestChildComponent : Component
    {
        private readonly string _type;

        public TestChildComponent(string type)
        {
            _type = type;
        }

        public override string ComponentType => _type;

        /// <summary>
        /// Allows adding its own child to simulate a "child with children" scenario.
        /// </summary>
        public void AddNestedChild(Component nested)
        {
            AddChild(nested);
        }

        // Example: if Key == "InvalidChildKey", we produce an error
        public override IEnumerable<string> Validate()
        {
            foreach (var err in base.Validate())
                yield return err;

            if (Key == "InvalidChildKey")
                yield return "Child key 'InvalidChildKey' is not allowed.";
        }
    }

    /// <summary>
    /// A test implementation of ITemplate, so we can confirm that
    /// an ITemplate can host deeper structures.
    /// </summary>
    private class TestTemplate : Component, ITemplate
    {
        private Guid _id = Guid.Empty;

        public Guid Id => _id;

        public void SetId(Guid id)
        {
            _id = id;
        }

        public override string ComponentType => "TestTemplate";
    }

    [Fact]
    public void AddChild_throws_if_child_same_ComponentType()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("parentA");

        var child = new TestChildComponent("ParentType"); // same type as parent
        child.SetKey("childA");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(
            () => parent.AddChild(child)
        );
        Assert.Contains("Cannot add a child of the same ComponentType 'ParentType'", ex.Message);
    }

    [Fact]
    public void AddChild_throws_if_non_template_parent_and_child_has_children()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("parentA");

        var childWithChildren = new TestChildComponent("ChildType");
        childWithChildren.SetKey("childB");

        // The child itself has a nested child, so childWithChildren.Children is non-empty
        var grandChild = new TestChildComponent("GrandChildType");
        grandChild.SetKey("grandChild1");
        childWithChildren.AddNestedChild(grandChild);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(
            () => parent.AddChild(childWithChildren)
        );
        Assert.Contains("non-template component cannot add a child component which already has children", ex.Message);
    }

    [Fact]
    public void AddChild_succeeds_if_non_template_parent_and_child_has_no_children()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("parentA");

        var child = new TestChildComponent("ChildType");
        child.SetKey("childA");

        // Act
        parent.AddChild(child);

        // Assert
        Assert.Single(parent.Children);
        Assert.Equal(child, parent.Children[0]);
    }

    [Fact]
    public void AddChild_throws_if_child_key_already_exists_regardless_of_case()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("parentA");

        var firstChild = new TestChildComponent("SomeOtherType");
        firstChild.SetKey("child1");

        var secondChild = new TestChildComponent("SomeOtherType");
        secondChild.SetKey("CHILD1"); // same key in different case

        parent.AddChild(firstChild);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(
            () => parent.AddChild(secondChild)
        );
        Assert.Contains("already exists in this component (key comparison is case-insensitive)", ex.Message);
    }

    [Fact]
    public void AddChild_succeeds_for_different_type_and_distinct_key()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("parentA");

        var child1 = new TestChildComponent("ChildType1");
        child1.SetKey("childOne");

        var child2 = new TestChildComponent("ChildType2");
        child2.SetKey("childTwo");

        // Act
        parent.AddChild(child1);
        parent.AddChild(child2);

        // Assert
        Assert.Equal(2, parent.Children.Count);
        Assert.Contains(parent.Children, c => c.Key == "childOne");
        Assert.Contains(parent.Children, c => c.Key == "childTwo");
    }

    [Fact]
    public void Template_can_add_child_that_itself_has_children()
    {
        // Arrange
        var template = new TestTemplate();
        template.SetKey("templateA");
        template.SetId(Guid.NewGuid());

        // Child #1 (intermediate)
        var midChild = new TestChildComponent("MidType");
        midChild.SetKey("midChild");

        // midChild has a nested leaf
        var leafChild = new TestChildComponent("LeafType");
        leafChild.SetKey("leaf1");
        // Leaf has no children, so midChild remains a single-level child
        midChild.AddNestedChild(leafChild);

        // Act
        // Because 'template' IS an ITemplate, adding midChild that has a child is allowed
        template.AddChild(midChild);

        // Assert
        Assert.Single(template.Children);
        Assert.Equal(midChild, template.Children[0]);

        // The midChild has 1 child
        Assert.Single(midChild.Children);
        Assert.Equal(leafChild, midChild.Children[0]);
    }

    [Fact]
    public void Template_still_cannot_add_child_of_same_type()
    {
        // Even though it's a template, we still don't allow the child to have the same component type
        // as the parent.

        // Arrange
        var template = new TestTemplate();
        template.SetKey("templateB");
        template.SetId(Guid.NewGuid());

        var childSameType = new TestChildComponent("TestTemplate");
        childSameType.SetKey("childX");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(
            () => template.AddChild(childSameType)
        );
        Assert.Contains("Cannot add a child of the same ComponentType 'TestTemplate'", ex.Message);
    }

    [Fact]
    public void Template_adds_child_key_conflict_still_throws()
    {
        // The normal key duplication check still applies
        var template = new TestTemplate();
        template.SetKey("templateC");
        template.SetId(Guid.NewGuid());

        var child1 = new TestChildComponent("ChildTypeA");
        child1.SetKey("childKey");

        var child2 = new TestChildComponent("ChildTypeB");
        child2.SetKey("CHILDKEY"); // same key, different case

        template.AddChild(child1);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(
            () => template.AddChild(child2)
        );
        Assert.Contains("A child component with key 'CHILDKEY' already exists", ex.Message);
    }

    [Fact]
    public void Parent_with_BadParentKey_fails_its_own_validation()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("BadParentKey");

        // Act
        var errors = parent.Validate().ToList();

        // Assert
        Assert.Single(errors);
        Assert.Contains("Parent has a forbidden key 'BadParentKey'", errors[0]);
    }

    [Fact]
    public void Child_with_InvalidChildKey_fails_its_own_validation()
    {
        // Arrange
        var child = new TestChildComponent("SomeChildType");
        child.SetKey("InvalidChildKey");

        // Act
        var errors = child.Validate().ToList();

        // Assert
        Assert.Single(errors);
        Assert.Contains("Child key 'InvalidChildKey' is not allowed.", errors[0]);
    }

    [Fact]
    public void AddChild_throws_if_child_is_invalid()
    {
        // Arrange
        var parent = new TestParentComponent();
        parent.SetKey("normalParent");

        // This child has a “forbidden” key
        var invalidChild = new TestChildComponent("ChildType");
        invalidChild.SetKey("InvalidChildKey"); // triggers error in Validate()

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            parent.AddChild(invalidChild)
        );
        Assert.Contains("Cannot add child component 'InvalidChildKey' because it is invalid", ex.Message);

        // Also confirm the child is not added
        Assert.Empty(parent.Children);
    }
}
