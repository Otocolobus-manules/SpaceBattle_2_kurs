public class CheckCollisionTests
{
    [Fact]
    public void CheckCollision_Tests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GetProperty",
            (object[] args) => new List<int>
            {
                1,
                1,
                1,
                1,
                1
            }).Execute();

        new InitFindVariationsCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CheckCollision",
            (object[] args) => new CheckCollisionCommand((IUObject)args[0], (IUObject)args[1])).Execute();
    }

    [Fact]
    public void CollisionTestPositive()
    {
        CheckCollision_Tests();

        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();

        var mockDictionary = new Mock<IDictionary<int, object>>();
        mockDictionary.SetupGet(d => d[It.IsAny<int>()]).Returns(mockDictionary.Object);

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CollisionTree",
            (object[] args) => mockDictionary.Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Collision",
            (object[] args) => mockCommand.Object).Execute();

        var mockUObject = new Mock<IUObject>();
        var checkCollisionCommand =
            Hwdtech.IoC.Resolve<ICommand>("Commands.CheckCollision", mockUObject.Object, mockUObject.Object);

        checkCollisionCommand.Execute();
    }

    [Fact]
    public void TryGetNewTreeThrowsException()
    {
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(c => c.Execute()).Verifiable();

        var mockDictionary = new Mock<IDictionary<int, object>>();
        mockDictionary.SetupGet(d => d[It.IsAny<int>()]).Throws(new System.Collections.Generic.KeyNotFoundException())
            .Verifiable();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CollisionTree",
            (object[] args) => mockDictionary.Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Collision",
            (object[] args) => mockCommand.Object).Execute();

        var mockUObject = new Mock<IUObject>();

        var checkCollisionCommand =
            Hwdtech.IoC.Resolve<ICommand>("Commands.CheckCollision", mockUObject.Object, mockUObject.Object);

        Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => checkCollisionCommand.Execute());
        mockDictionary.Verify(d => d[It.IsAny<int>()], Times.Once());
        mockCommand.Verify(command => command.Execute(), Times.Never());
    }
}
