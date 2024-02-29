public class RationalTest
{
    [Fact]
    public void Create_empty_Rational_Test()
    {
        var rat = new Rational(11, 24);
    }
    
    [Fact]
    public void Get_Set_Item_Test()
    {
        var rat = new Rational(12, 22);
        rat._corner = 23;
        rat._separation = 34;
        Assert.Equal(23, rat._corner);
        Assert.Equal(34, rat._separation);
    }

    [Fact]
    public void Sum_Test()
    {
        var rat1 = new Rational(10);
        var rat2 = new Rational(20);
        var rat3 = new Rational(30);
		var rat4 = new Rational(11, 113);
        Assert.True(rat1 + rat2 == rat3);
		Assert.Throws<System.ArgumentException>(() => rat1 + rat4);
    }

    [Fact]
    public void corner_Eq_NotEq_Test()
    {
        var rat1 = new Rational(10);
        var rat2 = new Rational(20);
        var rat3 = new Rational(10);
        Assert.False(rat1 == rat2);
        Assert.True(rat1 != rat2);
        Assert.True(rat1 == rat3);
    }

    [Fact]
    public void Equals_Test()
    {
        var rat1 = new Rational(10);
        var rat2 = new Rational(10);
        var rat3 = new Rational(20);
        Rational? rat4 = null;
            
        Assert.True(rat1.Equals(rat2));
        Assert.False(rat1.Equals(rat3));
        Assert.False(rat1.Equals(rat4));
        Assert.False(rat1.Equals(null));
    }
    
    [Fact]
    public void GetHashCodeTest()
    {
        var rat = new Rational(1);
        rat.GetHashCode();
    }
}
