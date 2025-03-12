using Genova.ContentService.Components;
using Genova.ContentService.Templates;

namespace UnitTests.ContentService.Components;

public class Template_Tests
{
    [Fact]
    public void New_template_has_empty_id()
    {
        // Arrange
        var template = new Template();
        template.SetKey("myTemplate");

        // Act
        var id = template.Id;

        // Assert
        Assert.Equal(Guid.Empty, id);
    }

    [Fact]
    public void SetId_assigns_guid_once()
    {
        // Arrange
        var template = new Template();
        var newGuid = Guid.NewGuid();

        // Act
        template.SetId(newGuid);
        var idAfterSet = template.Id;

        // Assert
        Assert.Equal(newGuid, idAfterSet);
    }

    [Fact]
    public void SetId_throws_if_already_set()
    {
        // Arrange
        var template = new Template();
        template.SetId(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            template.SetId(Guid.NewGuid())
        );
    }

    [Fact]
    public void ComponentType_is_Template()
    {
        // Arrange
        var template = new Template();

        // Act
        var type = template.ComponentType;

        // Assert
        Assert.Equal("Template", type);
    }

    [Fact]
    public void ComponentType_is_an_ITemplate()
    {
        // Arrange
        var template = new Template();

        // Assert
        Assert.True(template is ITemplate);
    }

    [Fact]
    public void Can_set_key_once()
    {
        // Arrange
        var template = new Template();

        // Act
        template.SetKey("myTemplate");

        // Assert
        Assert.Equal("myTemplate", template.Key);

        // Trying to overwrite should fail
        Assert.Throws<InvalidOperationException>(() =>
            template.SetKey("anotherKey")
        );
    }

    [Fact]
    public void Allows_children_with_children_since_its_an_ITemplate()
    {
        // Arrange
        var template = new Template();
        template.SetKey("rootTemplate");

        // A child that has its own child
        var midChild = new FakeComponent("MidType");
        midChild.SetKey("midChild");

        var leafChild = new FakeComponent("LeafType");
        leafChild.SetKey("leaf1");
        // leaf has no children, so midChild can add it
        midChild.AddChild(leafChild);

        // Act
        // Because 'template' is an ITemplate, it can add a child that already has children.
        template.AddChild(midChild);

        // Assert
        Assert.Single(template.Children);
        Assert.Equal(midChild, template.Children[0]);
        Assert.Single(midChild.Children);
        Assert.Equal(leafChild, midChild.Children[0]);
    }

    // A basic component used to simulate children of the template
    private class FakeComponent : Component
    {
        private readonly string _type;

        public FakeComponent(string type)
        {
            _type = type;
        }

        public override string ComponentType => _type;
    }
}
