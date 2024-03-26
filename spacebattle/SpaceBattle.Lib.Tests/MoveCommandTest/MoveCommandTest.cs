public class MovableTests
{
    [Fact]
    public void MoveCommandStraightLineMovementComplete()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { 12, 5 })).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(new int[] { -7, 3 })).Verifiable();

        ICommand moveCommand = new MoveCommand(movable.Object);

        moveCommand.Execute();

        movable.VerifySet(m => m.Position = new Vector(new int[] { 5, 8 }), Times.Once);
        movable.VerifyAll();
    }

    [Fact]
    public void MoveCommandCantReadPosition()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Throws(new NotImplementedException());

        ICommand moveCommand = new MoveCommand(movable.Object);

        Assert.Throws<NotImplementedException>(moveCommand.Execute);
    }

    [Fact]
    public void MoveCommandCantReadVelocity()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { 1, 4 })).Verifiable();
        movable.SetupGet(m => m.Velocity).Throws(new NotImplementedException());

        ICommand moveCommand = new MoveCommand(movable.Object);

        Assert.Throws<NotImplementedException>(moveCommand.Execute);
    }

    [Fact]
    public void MoveCommandCantSetPosition()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { 1, 4 })).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(new int[] { 1, 2 })).Verifiable();
        movable.SetupSet(m => m.Position = It.IsAny<Vector>()).Throws(new NotImplementedException());

        ICommand moveCommand = new MoveCommand(movable.Object);

        Assert.Throws<NotImplementedException>(moveCommand.Execute);
        movable.VerifyAll();
    }
}
