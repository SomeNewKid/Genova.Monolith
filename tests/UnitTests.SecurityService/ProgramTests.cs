using Genova.SecurityService;

namespace UnitTests.SecurityService;

public class ProgramTests
{
    [Fact]
    public void Can_create_an_instance_of_Program()
    {
        // ✅ Ensures the SecurityService project is referenced
        Program program = new Program();
        Assert.NotNull(program);
    }
}