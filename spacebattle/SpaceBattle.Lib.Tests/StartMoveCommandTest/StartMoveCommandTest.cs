public class StartMoveCommandTest
{
    [Fact]
    public void Init_Score_Env()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        var spaceship = new Mock<IMovable>();
        
        var SpeedChangeCommand = new Mock<IStartegy>();
        SpeedChangeCommand.Setup(x => x.Execute(It.IsAny<IUObject>(), It.IsAny<Vector>()));

        var LongMoveCommmand = new Mock<IStartegy>();
        LongMoveCommmand.Setup(x => x.Execute(It.IsAny<object[]>())).Returns(new MoveCommand(spaceship.Object));
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.LongMove", (object[] args) => LongMoveCommmand.Object.Execute(args)).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SpeedChange", (object[] args) => SpeedChangeCommand.Object.Execute(args)).Execute();
    }
    
    [Fact]
    public void StartCommand_Exe()
    {
        Init_Score_Env();
        var vector = new Vector(new int[]{1, 1});
        
		var UObject = new Mock<IUObject>();
        UObject.Setup(x => x.set_property("velocity", It.IsAny<Vector>()));
        
        var speed_changeable = new Mock<ISpeedChangeable>();
        speed_changeable.SetupGet(x => x.obj).Returns(UObject.Object).Verifiable();
        speed_changeable.SetupGet(x => x.speed_change).Returns(vector).Verifiable();
        
        var MoveComStart = new Mock<IMoveCommandStartable>();
        MoveComStart.SetupGet(x => x.velocity).Returns(It.IsAny<Vector>()).Verifiable();
        MoveComStart.SetupGet(x => x.obj).Returns(UObject.Object).Verifiable();
        MoveComStart.SetupGet(x => x.queue).Returns(new Queue<ICommand>()).Verifiable();

        var go_round = new StartMoveCommand(MoveComStart.Object);
        go_round.Execute();
    }
    
    [Fact]
    public void StartCommand_not_Velocity()
    {
        Init_Score_Env();
        
        var MoveComStart = new Mock<IMoveCommandStartable>();
        MoveComStart.SetupGet(x => x.velocity).Throws(new Exception()).Verifiable();
        MoveComStart.SetupGet(x => x.obj).Returns(It.IsAny<IUObject>).Verifiable();
        MoveComStart.SetupGet(x => x.queue).Returns(It.IsAny<Queue<ICommand>>).Verifiable();

        Assert.Throws<Exception>(() => new StartMoveCommand(MoveComStart.Object).Execute());
    }
    
    [Fact]
    public void StartCommand_not_Object()
    {
        Init_Score_Env();
        
        var MoveComStart = new Mock<IMoveCommandStartable>();
        MoveComStart.SetupGet(x => x.velocity).Returns(It.IsAny<Vector>()).Verifiable();
        MoveComStart.SetupGet(x => x.obj).Throws(new Exception()).Verifiable();
        MoveComStart.SetupGet(x => x.queue).Returns(It.IsAny<Queue<ICommand>>).Verifiable();

        Assert.Throws<Exception>(() => new StartMoveCommand(MoveComStart.Object).Execute());
    }
    
    [Fact]
    public void StartCommand_not_Queue()
    {
        Init_Score_Env();
        
        var MoveComStart = new Mock<IMoveCommandStartable>();
        MoveComStart.SetupGet(x => x.velocity).Returns(It.IsAny<Vector>()).Verifiable();
        MoveComStart.SetupGet(x => x.obj).Returns(It.IsAny<IUObject>).Verifiable();
        MoveComStart.SetupGet(x => x.queue).Throws(new Exception()).Verifiable();

        Assert.Throws<Exception>(() => new StartMoveCommand(MoveComStart.Object).Execute());
    }
}
