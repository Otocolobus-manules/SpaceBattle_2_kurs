public class CreateObjectCollection: ICommand  
{
    private int _gameObjCount;

    public CreateObjectCollection(int gameObjCount)
    {
        _gameObjCount = gameObjCount;
    }

    public void Execute()
    {
        var gameUObjectMap = Hwdtech.IoC.Resolve<IDictionary<int, IUObject>>("UObjectMap");
        Enumerable.Range(0, _gameObjCount).ToList().ForEach(i => gameUObjectMap.Add(i, Hwdtech.IoC.Resolve<IUObject>("UObjectCreate")));
    }
}
