public class EmptyCommandTest
{
    [Fact]
    public void EmptyCommand_Test()
    {
        var command = new EmptyCommand();
        command.Execute();
    }
}
