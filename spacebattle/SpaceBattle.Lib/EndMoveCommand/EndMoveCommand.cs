public class EndMoveCommand: ICommand
{
    private readonly IMoveCommandEndable _endable;

    public EndMoveCommand(IMoveCommandEndable endable)
    {
        _endable = endable;
    }

    public void Execute()
    {
        var emptyCommand = Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Commands.EmptyCommand");
        Hwdtech.IoC.Resolve<IInjectable>("Commands.Inject", _endable.command, emptyCommand);
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Commands.SpeedChange", _endable.obj, new Vector(new int[] {1, 1}));
        _endable.queue.Dequeue();
    }
}
