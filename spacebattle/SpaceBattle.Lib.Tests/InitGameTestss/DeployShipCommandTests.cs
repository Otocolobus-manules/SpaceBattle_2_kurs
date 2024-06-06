public class DeployShipCommandTests
{
    [Fact]
    public void DeployShipCommandTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObjectSetProperty", (object[] args) => mockCommand.Object).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeployShip", (object[] args) => new DeployShipCommand((IUObject)args[0], (IEnumerator<object>)args[1])).Execute();
        
        var mockobj = new Mock<IUObject>();

        var MockPositionIter = new Mock<IEnumerator<object>>();
        MockPositionIter.SetupGet(x => x.Current).Verifiable();
        MockPositionIter.Setup(x => x.MoveNext()).Verifiable();

        mockCommand.Verify(x => x.Execute(), Times.Never());
        Hwdtech.IoC.Resolve<ICommand>("DeployShip", mockobj.Object, MockPositionIter.Object).Execute();
        mockCommand.Verify(x => x.Execute(), Times.Once());
        MockPositionIter.VerifyAll();
    }
}
