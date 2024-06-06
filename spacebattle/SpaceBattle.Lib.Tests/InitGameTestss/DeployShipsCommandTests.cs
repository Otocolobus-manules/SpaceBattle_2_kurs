public class DeployShipsCommandTests
{
    [Fact]
    public void DeployShipsCommandTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var MockGameObject = Enumerable.Repeat((new Mock<IUObject>()).Object, 3).ToList();

        var MockPositionIterator = new Mock<IEnumerator<object>>();
        MockPositionIterator.Setup(x => x.Reset()).Verifiable();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ShipPositionIterator", (object[] args) => MockPositionIterator.Object).Execute();

        var MockCommand = new Mock<ICommand>();
        MockCommand.Setup(x => x.Execute()).Verifiable();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeployShip", (object[] args) => MockCommand.Object).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeployShips", (object[] args) => new DeployShipsCommand((IEnumerable<IUObject>)args[0])).Execute();

        MockPositionIterator.Verify(x => x.Reset(), Times.Never());
        MockCommand.Verify(x => x.Execute(), Times.Never());
        Hwdtech.IoC.Resolve<ICommand>("DeployShips", MockGameObject).Execute();
        MockPositionIterator.Verify(x => x.Reset(), Times.Once());
        MockCommand.Verify(x => x.Execute(), Times.Exactly(3));
    }
}
