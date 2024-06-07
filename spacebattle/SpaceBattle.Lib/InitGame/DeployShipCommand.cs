public class DeployShipCommand : ICommand  
{
    private IUObject _gameobj;
    private IEnumerator<object> _shipPositionEnum;

    public DeployShipCommand(IUObject gameobj, IEnumerator<object> shipPositionEnum)
    {
        _gameobj = gameobj;
        _shipPositionEnum = shipPositionEnum;
    }

    public void Execute()
    {
        Hwdtech.IoC.Resolve<ICommand>("UObjectSetProperty", _gameobj, "Position", _shipPositionEnum.Current).Execute();
        _shipPositionEnum.MoveNext();
    }
}
