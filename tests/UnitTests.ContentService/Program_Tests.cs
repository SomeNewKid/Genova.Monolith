using Genova.ContentService;

namespace UnitTests.ContentService;

public class Program_Tests
{
    [Fact]
    public void Can_create_an_instance_of_Program()
    {
        Program program = new();
        Assert.NotNull(program);
    }
}