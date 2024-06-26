public class ReplaceCommand: ICommand
{
    IMethodChangeable _thread;
    IStartegy _f;
    public ReplaceCommand(IMethodChangeable thread, IStartegy f)
    {
        _thread = thread;
        _f = f;
    }
    public void Execute()
    {
        _thread.ChangeMethod(_f);
    }
}
