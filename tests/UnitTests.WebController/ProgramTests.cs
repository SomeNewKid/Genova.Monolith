using Genova.WebController;

namespace UnitTests.WebController;

public class ProgramTests
{
    [Fact]
    public void Can_create_an_instance_of_Program()
    {
        Program program = new();
        Assert.NotNull(program);
    }
}