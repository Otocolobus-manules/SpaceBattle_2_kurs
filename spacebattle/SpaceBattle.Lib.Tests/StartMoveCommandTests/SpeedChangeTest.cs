public class SpeedChangeTest
{
    [Fact]
    public void Execute_SpeedChange_With_Anothe_With_UIObject()
    {
        var dict = new Dictionary<string, object>();
        var UObject = new Mock<IUObject>();
        UObject.Setup(x => x.set_property("velocity", It.IsAny<Vector>())).Callback<string, object>((string a, object z) => dict["velocity"] = z);
        new SpeedChange(UObject.Object, It.IsAny<Vector>()).Execute();
        UObject.Setup(dict => dict.get_property("velocity")).Returns(dict["velocity"]).Verifiable();
        Assert.Equal(UObject.Object.get_property("velocity"), dict["velocity"]);
    }
    
    [Fact]
    public void Execute_SpeedChange_With_Another()
    {
        int[] vector = { 1, 1 };
        Assert.Throws<InvalidCastException>(() => new SpeedChange(new object(), new Vector(vector)).Execute());
    }
}
