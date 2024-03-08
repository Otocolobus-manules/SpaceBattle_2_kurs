using System.Collections.Concurrent;


public class SoftStopCommandTest
{
    [Fact]
    public void SoftStopCommand_Test()
    {
        Func<object, object> standart_command_preprocessing_function = (object x) =>
        {
            return ((BlockingCollection<ICommand>)x).Take();
        };

        Func<BlockingCollection<ICommand>, System.Boolean> is_queue_empty_checker = (BlockingCollection<ICommand> x) =>
        {
            return x.Count() == 0;
        };

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
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.StandartComandPreprocessingFunction",
            (object[] args) => standart_command_preprocessing_function).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WalkerInQueueWithStopByEmptyQueue",
            (object[] args) =>
                new WalkerInQueueWithStopByEmptyQueue((Func<object, object>)args[0], (ServerThread)args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ChangeThreadCommand",
            (object[] args) => new StandartChangeThreadCommand((ServerThread)args[0], (IStartegy)args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.IsQueueEmpty",
            (object[] args) => (object)is_queue_empty_checker((BlockingCollection<ICommand>)args[0])).Execute();


        var mre = new ManualResetEvent(false);
        var mre1 = new ManualResetEvent(false);

        var InitScopeInThreadCommand = new Mock<ICommand>();

        InitScopeInThreadCommand.Setup(x => x.Execute()).Callback(() =>
        {
            new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.HardStopCommand",
                (object[] args) => new HardStopCommand((ServerThread)args[0])).Execute();
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SoftStopCommand",
                    (object[] args) =>
                        new SoftStopCommand((ServerThread)args[0], (BlockingCollection<ICommand>)args[1]))
                .Execute();
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.StandartComandPreprocessingFunction",
                (object[] args) => standart_command_preprocessing_function).Execute();
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.WalkerInQueueWithStopByEmptyQueue",
                    (object[] args) =>
                        new WalkerInQueueWithStopByEmptyQueue((Func<object, object>)args[0], (ServerThread)args[1]))
                .Execute();
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.IsQueueEmpty",
                (object[] args) => (object)is_queue_empty_checker((BlockingCollection<ICommand>)args[0])).Execute();
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.ChangeThreadCommand",
                    (object[] args) => new StandartChangeThreadCommand((ServerThread)args[0], (IStartegy)args[1]))
                .Execute();
        });

        var TestCommand = new Mock<ICommand>();
        var TestCommand_after_soft = new Mock<ICommand>();
        var TestCommand_that_should_be_action2 = new Mock<ICommand>();

        var queue = Hwdtech.IoC.Resolve<BlockingCollection<ICommand>>("Commands.BlockingQueueOfICommand");

        var thread_test = Hwdtech.IoC.Resolve<IStartegy>("Commands.ServerThread",
            Hwdtech.IoC.Resolve<IStartegy>("Commands.WalkerInQueue",
                Hwdtech.IoC.Resolve<System.Func<object, object>>("Commands.StandartComandPreprocessingFunction")),
            queue);
        var soft_stop_cmd = Hwdtech.IoC.Resolve<ICommand>("Commands.SoftStopCommand", thread_test, queue);

        TestCommand.Setup(x => x.Execute()).Callback(() => mre.Set()).Verifiable();
        TestCommand_that_should_be_action2.Setup(x => x.Execute()).Verifiable();
        TestCommand_after_soft.Setup(x => x.Execute()).Callback(() =>
        {
            queue.Add(TestCommand_that_should_be_action2.Object);
            mre1.Set();
        }).Verifiable();

        queue.Add(InitScopeInThreadCommand.Object);
        queue.Add(TestCommand.Object);
        queue.Add(soft_stop_cmd);
        queue.Add(TestCommand_after_soft.Object);

        Hwdtech.IoC.Resolve<ICommand>("Commands.StartThread", thread_test).Execute();
        Assert.True(mre.WaitOne(1000));
        Assert.True(mre1.WaitOne(1000));
        TestCommand.Verify(x => x.Execute(), Times.Once());
        TestCommand_after_soft.Verify(x => x.Execute(), Times.Once());
        TestCommand_that_should_be_action2.Verify(x => x.Execute(), Times.Once());
        Assert.True(0 == queue.Count());
    }
}
