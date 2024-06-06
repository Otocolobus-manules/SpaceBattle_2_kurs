public class DeployShipsCommand: ICommand  
{
    private IEnumerable<IUObject> _gameobj;

    public DeployShipsCommand(IEnumerable<IUObject> gameobj) { _gameobj = gameobj; }

    public void Execute()
    {
        var shipPositionIterator = Hwdtech.IoC.Resolve<IEnumerator<object>>("ShipPositionIterator");
        _gameobj.ToList().ForEach(ship => Hwdtech.IoC.Resolve<ICommand>("DeployShip", ship, shipPositionIterator).Execute());
        shipPositionIterator.Reset();
    }
}
