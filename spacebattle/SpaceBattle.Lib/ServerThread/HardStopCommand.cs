public class HardStopCommand : ICommand
{
    readonly IStopable _thread;

    public HardStopCommand(ServerThread thread)
    {
        _thread = thread;
    }

    public void Execute()
    {
        _thread.Stop();
    }
}
