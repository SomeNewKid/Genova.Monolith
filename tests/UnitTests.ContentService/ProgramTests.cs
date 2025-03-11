using Genova.ContentService;

namespace UnitTests.ContentService;

public class ProgramTests
{
    [Fact]
    public void Can_create_an_instance_of_Program()
    {
        // ✅ Ensures the ContentService project is referenced
        Program program = new Program();
        Assert.NotNull(program);
    }
}