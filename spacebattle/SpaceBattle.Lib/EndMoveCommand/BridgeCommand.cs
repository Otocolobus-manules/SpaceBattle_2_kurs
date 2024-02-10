public class BridgeCommand: ICommand, IInjectable
{
    private ICommand _command;

    public BridgeCommand(ICommand command)
    {
        _command = command;
    }

    public void Execute()
    {
        _command.Execute();
    }

    public void Inject(ICommand command)
    {
        _command = command;
    }
}
