public class BridgeCommandTest
{
    [Fact]
    public void BridgeCommand_Test()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
        
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        var bridgeCommand = new BridgeCommand(mockCommand.Object);
        bridgeCommand.Inject(Hwdtech.IoC.Resolve<ICommand>("Commands.EmptyCommand"));
        bridgeCommand.Execute();

        mockCommand.Verify(m => m.Execute(), Times.Never());
    }
}
