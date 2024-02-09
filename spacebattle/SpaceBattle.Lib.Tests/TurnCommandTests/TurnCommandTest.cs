public class TurnCommandTest
{
    [Fact]
    public void Rotate_Test()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        
        turnable.SetupGet(t => t.corner).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(t => t.delta).Returns(new Rational(90)).Verifiable();
        
        TurnCommand turn_command = new TurnCommand(turnable.Object);
        turn_command.Execute();

        turnable.VerifySet(m => m.corner = new Rational(135), Times.Once);
    }

    [Fact]
    public void Corner_Error_Test()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        
        turnable.SetupGet(t => t.corner).Throws(new Exception()).Verifiable();
        turnable.SetupGet(t => t.delta).Returns(new Rational(45)).Verifiable();

        TurnCommand turn_command = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turn_command.Execute());
    }
    
    [Fact]
    public void Delta_Error_Test()
    {
        Mock<ITurnable> turnable = new Mock<ITurnable>();
        turnable.SetupGet(t => t.corner).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(t => t.delta).Throws(new Exception()).Verifiable();

        TurnCommand turn_command = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turn_command.Execute());
    }
    
    [Fact]
    public void Error_Set_Corner_Test()
    {
        var turnable = new Mock<ITurnable>();

        turnable.SetupGet(t => t.corner).Returns(new Rational(45)).Verifiable();
        turnable.SetupGet(t => t.delta).Returns(new Rational(45)).Verifiable();
        turnable.SetupSet(t => t.corner = It.IsAny<Rational>()).Throws(new Exception()).Verifiable();

        var turn_command = new TurnCommand(turnable.Object);
        Assert.Throws<Exception>(() => turn_command.Execute());
    }
}
