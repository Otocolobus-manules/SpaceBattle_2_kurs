public class StartThread : ICommand
{
    readonly ServerThread _server_thread;

    public StartThread(ServerThread thread)
    {
        _server_thread = thread;
    }

    public void Execute()
    {
        ((Thread)_server_thread.Execute()).Start();
    }
}
