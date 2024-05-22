using System.Collections.Concurrent;

public class ReplaceCommandTest
{
    [Fact]
    public void test_replace_command()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ServerThreadStrategy", (object[] args) => new ServerThread((IStartegy)args[0], (BlockingCollection<ICommand>)args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.BlockingQueueOfICommand", (object[] args) => new BlockingCollection<ICommand>()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WalkerInQueueStrategy", (object[] args) => new WalkerInQueue((Func<object, object>)args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.StartServerThreadCommand", (object[] args) => new StartThread((ServerThread)args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SoftStopServerThreadCommand", (object[] args) => new SoftStopCommand((ServerThread)args[0], (BlockingCollection<ICommand>)args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ReplaceCommand", (object[] args) => new ReplaceCommand((IMethodChangeable)args[0], (IStartegy)args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.HardStopServerThreadCommand", (object[] args) => new HardStopCommand((ServerThread)args[0])).Execute();

        var check1 = new Mock<ICommand>();
        var check2 = new Mock<ICommand>();
        check1.Setup(p => p.Execute()).Verifiable();
        check2.Setup(p => p.Execute()).Verifiable();

        Func<object, object> f1 = (object x) =>
        {
            check1.Object.Execute();
            return ((BlockingCollection<ICommand>)x).Take();
        };

        Func<object, object> f2 = (object x) =>
        {
            check2.Object.Execute();
            return ((BlockingCollection<ICommand>)x).Take();
        };

        var mre = new ManualResetEvent(false);

        var TestCommand1 = new Mock<ICommand>();
        var TestCommand2 = new Mock<ICommand>();

        var queue = Hwdtech.IoC.Resolve<BlockingCollection<ICommand>>("Commands.BlockingQueueOfICommand");
        var exe1 = Hwdtech.IoC.Resolve<IStartegy>("Commands.WalkerInQueueStrategy", f1);
        var exe2 = Hwdtech.IoC.Resolve<IStartegy>("Commands.WalkerInQueueStrategy", f2);
        var thread_test = Hwdtech.IoC.Resolve<IStartegy>("Commands.ServerThreadStrategy", exe1, queue);
        var replacer = Hwdtech.IoC.Resolve<ICommand>("Commands.ReplaceCommand", thread_test, exe2);
        var hard_stop_cmd = Hwdtech.IoC.Resolve<ICommand>("Commands.HardStopServerThreadCommand", thread_test);

        TestCommand1.Setup(p => p.Execute()).Verifiable();
        TestCommand2.Setup(p => p.Execute()).Callback(() => mre.Set()).Verifiable();

        queue.Add(TestCommand1.Object);
        queue.Add(replacer);
        queue.Add(TestCommand2.Object);
        queue.Add(hard_stop_cmd);

        Hwdtech.IoC.Resolve<ICommand>("Commands.StartServerThreadCommand", thread_test).Execute();

        Assert.True(mre.WaitOne(10000));
        TestCommand1.Verify(p => p.Execute(), Times.Once());
        TestCommand2.Verify(p => p.Execute(), Times.Once());
        check1.Verify(p => p.Execute(), Times.Exactly(2));
        check2.Verify(p => p.Execute(), Times.Exactly(2));
        Assert.True(0 == queue.Count());
    }
}
