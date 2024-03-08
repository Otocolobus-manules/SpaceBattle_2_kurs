using System.Collections.Concurrent;


public class SoftStopCommand : ICommand
{
    private BlockingCollection<ICommand> _queue;
    private readonly ServerThread _thread;

    public SoftStopCommand(ServerThread thread, BlockingCollection<ICommand> queue)
    {
        _thread = thread;
        _queue = queue;
    }

    public void Execute()
    {
        Hwdtech.IoC.Resolve<ICommand>("Commands.ChangeThreadCommand",
            _thread,
            Hwdtech.IoC.Resolve<IStartegy>("Commands.WalkerInQueueWithStopByEmptyQueue",
                Hwdtech.IoC.Resolve<System.Func<object, object>>("Commands.StandartComandPreprocessingFunction"),
                _thread)).Execute();
    }
}
