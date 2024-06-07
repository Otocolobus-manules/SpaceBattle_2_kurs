public class ShipPositionIteratorTests
{
    [Fact]
    public void SuccessfulTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var positions = new List<Vector>{ new Vector(new int[] {0, 0}), new Vector(new int[] {1, 0}) };
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ShipPositions", (object[] args) => positions).Execute();
        
        var iterator = new ShipPositionIterator();

        Assert.Equal(positions[0], iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(positions[1], iterator.Current);
        Assert.False(iterator.MoveNext());

        iterator.Reset();
        Assert.Equal(positions[0], iterator.Current);
        Assert.Throws<NotImplementedException>(() => iterator.Dispose());
    }

    [Fact]
    public void ThrowsOutOfRangeExceptionTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var positions = new List<Vector>{ new Vector(new int[] {0, 0}) };

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ShipPositions", (object[] args) => positions).Execute();
        var iterator = new ShipPositionIterator();

        Assert.Equal(positions[0], iterator.Current);
        Assert.False(iterator.MoveNext());
        Assert.Throws<ArgumentOutOfRangeException>(() => iterator.Current);
    }
}
