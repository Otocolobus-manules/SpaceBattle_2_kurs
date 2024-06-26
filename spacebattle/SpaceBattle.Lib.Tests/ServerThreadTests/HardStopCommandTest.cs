using System.Collections.Concurrent;


public class HardStopCommandTest
{
    [Fact]
    public void HardStopCommand_Test()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ServerThread",
            (object[] args) => new ServerThread((IStartegy)args[0], (BlockingCollection<ICommand>)args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.BlockingQueueOfICommand",
            (object[] args) => new BlockingCollection<ICommand>()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.StartThread",
            (object[] args) => new StartThread((ServerThread)args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.HardStopCommand",
            (object[] args) => new HardStopCommand((ServerThread)args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WalkerInQueue",
            (object[] args) => new WalkerInQueue((Func<object, object>)args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SoftStopCommand",
                (object[] args) => new SoftStopCommand((ServerThread)args[0], (BlockingCollection<ICommand>)args[1]))
            .Execute();

        var mre = new ManualResetEvent(false);

        var TestCommand = new Mock<ICommand>();
        TestCommand.Setup(x => x.Execute()).Callback(() => mre.Set()).Verifiable();

        var TestCommand_that_should_not_be_action = new Mock<ICommand>();
        TestCommand_that_should_not_be_action.Setup(x => x.Execute()).Verifiable();

        var queue = Hwdtech.IoC.Resolve<BlockingCollection<ICommand>>("Commands.BlockingQueueOfICommand");

        Func<object, object> f = (object x) =>
        {
            return ((BlockingCollection<ICommand>)x).Take();
        };

        var thread_test = Hwdtech.IoC.Resolve<IStartegy>("Commands.ServerThread",
            Hwdtech.IoC.Resolve<IStartegy>("Commands.WalkerInQueue", f), queue);
        var hard_stop_cmd = Hwdtech.IoC.Resolve<ICommand>("Commands.HardStopCommand", thread_test);

        queue.Add(TestCommand.Object);
        queue.Add(hard_stop_cmd);
        queue.Add(TestCommand_that_should_not_be_action.Object);

        Hwdtech.IoC.Resolve<ICommand>("Commands.StartThread", thread_test).Execute();
        Assert.True(mre.WaitOne(10000));

        TestCommand.Verify(x => x.Execute(), Times.Once());
        TestCommand_that_should_not_be_action.Verify(x => x.Execute(), Times.Never());

        Assert.True(1 == queue.Count());
        Assert.Equal(queue.Take(), TestCommand_that_should_not_be_action.Object);
    }
}
