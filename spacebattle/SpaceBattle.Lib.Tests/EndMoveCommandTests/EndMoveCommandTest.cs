public class EndMoveCommandTest
{
    [Fact]
    public void Init_Score()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        var InjectCom = new Mock<IInjectStrategy>();
        InjectCom.Setup(x => x.Inject(It.IsAny<ICommand>()));
        
        var SpeedChangeCommand = new Mock<IStartegy>();
        SpeedChangeCommand.Setup(x => x.Execute(It.IsAny<IUObject>(), It.IsAny<Vector>()));

        var EmptyCom = new Mock<IStartegy>();
        EmptyCom.Setup(x => x.Execute());
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.EmptyCommand", (object[] args) => EmptyCom.Object.Execute()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Inject", (object[] args) => InjectCom.Object.Inject(args)).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SpeedChange", (object[] args) => SpeedChangeCommand.Object.Execute(args)).Execute();

    }

    [Fact]
    public void EndMoveCommand_Test()
    {
        Init_Score();
        
        var vector = new Vector(new int[]{1, 1});
        
        var UObject = new Mock<IUObject>();
        UObject.Setup(x => x.set_property("velocity", It.IsAny<Vector>()));
    
        var spaceship = new Mock<IMovable>();
        var command = new MoveCommand(spaceship.Object);
        var queue = new Queue<ICommand>();
        queue.Enqueue(command);
        
        var speed_changeable = new Mock<ISpeedChangeable>();
        speed_changeable.SetupGet(x => x.obj).Returns(UObject.Object).Verifiable();
        speed_changeable.SetupGet(x => x.speed_change).Returns(vector).Verifiable();
        
        var MoveComEnd = new Mock<IMoveCommandEndable>();
        MoveComEnd.SetupGet(x => x.command).Returns(It.IsAny<ICommand>()).Verifiable();
        MoveComEnd.SetupGet(x => x.obj).Returns(UObject.Object).Verifiable();
        MoveComEnd.SetupGet(x => x.queue).Returns(queue).Verifiable();
        
        var go_round = new EndMoveCommand(MoveComEnd.Object);
        go_round.Execute();
    }
    
    [Fact]
    public void StartCommand_not_command()
    {
        Init_Score();
        
        var MoveComEnd = new Mock<IMoveCommandEndable>();
        MoveComEnd.SetupGet(x => x.command).Throws(new Exception()).Verifiable();
        MoveComEnd.SetupGet(x => x.obj).Returns(It.IsAny<IUObject>()).Verifiable();
        MoveComEnd.SetupGet(x => x.queue).Returns(It.IsAny<Queue<ICommand>>()).Verifiable();

        Assert.Throws<Exception>(() => new EndMoveCommand(MoveComEnd.Object).Execute());
    }
    
    [Fact]
    public void StartCommand_not_obj()
    {
        Init_Score();
        
        var MoveComEnd = new Mock<IMoveCommandEndable>();
        MoveComEnd.SetupGet(x => x.command).Returns(It.IsAny<ICommand>()).Verifiable();
        MoveComEnd.SetupGet(x => x.obj).Throws(new Exception()).Verifiable();
        MoveComEnd.SetupGet(x => x.queue).Returns(It.IsAny<Queue<ICommand>>()).Verifiable();

        Assert.Throws<Exception>(() => new EndMoveCommand(MoveComEnd.Object).Execute());
    }
    
    [Fact]
    public void StartCommand_not_queue()
    {
        Init_Score();
        
        var MoveComEnd = new Mock<IMoveCommandEndable>();
        MoveComEnd.SetupGet(x => x.command).Returns(It.IsAny<ICommand>()).Verifiable();
        MoveComEnd.SetupGet(x => x.obj).Returns(It.IsAny<IUObject>()).Verifiable();
        MoveComEnd.SetupGet(x => x.queue).Throws(new Exception()).Verifiable();

        Assert.Throws<Exception>(() => new EndMoveCommand(MoveComEnd.Object).Execute());
    }
}
