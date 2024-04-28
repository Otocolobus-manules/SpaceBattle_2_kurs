

public class CheckCollisionCommandTests
{
    private static Vector? vec1;
    private static Vector? vec2;
    
    static CheckCollisionCommandTests()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            Hwdtech.IoC.Resolve<object>(
                "Scopes.New", Hwdtech.IoC.Resolve<object>(
                    "Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionTree", (object[] args) =>
        {
            var tree = new Dictionary<int, object>()
            {
                { 0, new Dictionary<int, object>() },
                { 1, new Dictionary<int, object>() },
                { 2, new Dictionary<int, object>() },
                { 3, new Dictionary<int, object>() }
            };
            ((Dictionary<int, object>)tree[0])[0] = new Dictionary<int, object>();
            ((Dictionary<int, object>)((Dictionary<int, object>)tree[0])[0])[0] = new Dictionary<int, object>();
            ((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)tree[0])[0])[0])[0] = new Dictionary<int, object>();
            return tree;
        }).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DifVec", (object[] args) =>
        {
            var position1 = (Vector)args[0];
            var position2 = (Vector)args[1];
            var velocity1 = (Vector)args[2];
            var velocity2 = (Vector)args[3];
            var vec = new List<int>
            {
                position1[0] - position2[0],
                position1[1] - position2[1],
                velocity1[0] - velocity2[0],
                velocity1[1] - velocity2[1],
            };
            return vec;
        }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Object1GetProperty", (object[] args) => vec1).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Object2GetProperty", (object[] args) => vec2).Execute();
    }

    [Fact]
    public void CorrectCollisionCheckCommand()
    {
        var mockCommand = new Mock<ICommand>();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Collision", (object[] args) => mockCommand.Object).Execute();

        vec1 = new Vector(new int[] { 1, 1 });
        vec2 = new Vector(new int[] { 1, 1 });

        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        var check = new CheckCollisionCommand(obj1.Object, obj2.Object);
        check.Execute();
        mockCommand.Verify();
    }

    [Fact]
    public void IncorrectCheckCollisionCommand()
    {
        var mockCommand = new Mock<ICommand>();

        vec1 = new Vector(new int[] { 1, 1 });
        vec2 = new Vector(new int[] { 5, 6 });

        var obj1 = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();

        var check = new CheckCollisionCommand(obj1.Object, obj2.Object);
        check.Execute();
        mockCommand.Verify(mc => mc.Execute(), Times.Never());
    }
}
