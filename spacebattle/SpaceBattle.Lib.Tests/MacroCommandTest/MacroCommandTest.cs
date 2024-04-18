public class MacroCommandTests
{
    [Fact]
    public void MacroCommandTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MacroCommandStrategy", (object[] args) => { return new MacroCommandStrategy().Execute((string[])args); }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MacroCommand", (object[] args) => { return new MacroCommand((ICommand[])args); }).Execute();

        var moveCommand = new Mock<ICommand>();
        moveCommand.Setup(mc => mc.Execute()).Callback(() => { }).Verifiable();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Move", (object[] args) => moveCommand.Object).Execute();

        var rotateCommand = new Mock<ICommand>();
        moveCommand.Setup(rc => rc.Execute()).Callback(() => { }).Verifiable();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Rotate", (object[] args) => rotateCommand.Object).Execute();

        var commands = new string[] { "Commands.Move", "Commands.Rotate" };
        var macroCommand = Hwdtech.IoC.Resolve<ICommand>("Commands.MacroCommandStrategy", commands);
        macroCommand.Execute();

        moveCommand.Verify(mc => mc.Execute(), Times.Once);
        rotateCommand.Verify(rc => rc.Execute(), Times.Once);
    }
}
