public class SetFuelCommandTests
{
    [Fact]
    public void SetFuelCommandTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObjectSetProperty", (object[] args) => new ActionCommand(() => ((IUObject)args[0]).SetProperty((string)args[1], (object)args[2]))).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetFuel", (object[] args) => new SetFuelCommand((IEnumerable<IUObject>)args[0], (double)args[1])).Execute();

        var mockUObjects = Enumerable.Range(0, 3).Select(x =>
        {
            var mock = new Mock<IUObject>();
            mock.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
            return mock;
        }).ToList();

        mockUObjects.ForEach(mock => mock.Verify(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Never()));
        Hwdtech.IoC.Resolve<ICommand>("SetFuel", mockUObjects.Select(x => x.Object), 100.0).Execute();
        mockUObjects.ForEach(mock => mock.Verify(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()), Times.Once()));
    }
}
