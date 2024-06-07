public class CreateObjectCollectionTests
{
    [Fact]
    public void CreateObjectCollectionTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var gameUObjectMap = new Dictionary<int, IUObject>();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObjectMap", (object[] args) => gameUObjectMap).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObjectCreate", (object[] args) => new Mock<IUObject>().Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateUObjectCollection", (object[] args) => new CreateObjectCollection((int)args[0])).Execute();

        Assert.Empty(gameUObjectMap);
        Hwdtech.IoC.Resolve<ICommand>("CreateUObjectCollection", 10).Execute();
        Assert.Equal(10, gameUObjectMap.Count);
    }
}
