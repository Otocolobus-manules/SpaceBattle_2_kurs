public class StandartChangeThreadCommand : ICommand
{
    private readonly ServerThread _thread;
    readonly IStartegy _new_strategy;

    public StandartChangeThreadCommand(ServerThread thread, IStartegy new_strategy)
    {
        _thread = thread;
        _new_strategy = new_strategy;
    }

    public void Execute()
    {
        _thread.ChangeMethod(_new_strategy);
    }
}
