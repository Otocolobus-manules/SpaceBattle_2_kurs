public class StartMoveCommand : ICommand
{
    private readonly IMoveStartable _order;

    public StartMoveCommand(IMoveStartable order)
    {
        _order = order;
    }

    public void Execute()
    {
        Hwdtech.IoC.Resolve<ICommand>("OrderTargetSetProperty", _order.UObject, "Velocity", _order.initialVelocity).Execute();

        var movable = Hwdtech.IoC.Resolve<IMovable>("Commands.Movable.Create", _order);
        var moveCommand = Hwdtech.IoC.Resolve<ICommand>("Commands.MoveCommand.Create", movable);

        var injectCommand = Hwdtech.IoC.Resolve<InjectCommand>("Inject.Create", moveCommand);
        Hwdtech.IoC.Resolve<ICommand>("Queue.Add", _order.Queue, injectCommand).Execute();
    }
}
