public class SetFuelCommand: ICommand  
{
    private IEnumerable<IUObject> _gameobj;
    private double _fuel;

    public SetFuelCommand(IEnumerable<IUObject> gameobj, double fuel)
    {
        _gameobj = gameobj;
        _fuel = fuel;
    }

    public void Execute()
    {
        _gameobj.ToList().ForEach(
            x => Hwdtech.IoC.Resolve<ICommand>("UObjectSetProperty", x, "Fuel", _fuel).Execute()
        );
    }
}
