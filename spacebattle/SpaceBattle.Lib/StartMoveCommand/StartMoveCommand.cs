using Hwdtech;

public class StartMoveCommand: ICommand
{
    private IMoveCommandStartable _startable;

    public StartMoveCommand(IMoveCommandStartable obj)
    {
        _startable = obj;
    }

    public void Execute()
    {
        Hwdtech.IoC.Resolve<ICommand>("Commands.SpeedChange", _startable.obj, _startable.velocity);
        var long_cmd = Hwdtech.IoC.Resolve<ICommand>("Commands.LongMove", _startable.obj);
        _startable.queue.Enqueue(long_cmd);
    }
}
