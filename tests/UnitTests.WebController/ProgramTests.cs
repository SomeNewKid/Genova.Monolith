using Genova.WebController;

namespace UnitTests.WebController;

public class ProgramTests
{
    [Fact]
    public void Can_create_an_instance_of_Program()
    {
        // ✅ Ensures the WebController project is referenced
        Program program = new Program();
        Assert.NotNull(program);
    }
}