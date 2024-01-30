public class RationalTest
{
    [Fact]
    public void Create_empty_Rational_Test()
    {
        Rational rat = new Rational(11, 24);
    }
    
    [Fact]
    public void Get_Set_Item_Test()
    {
        Rational rat = new Rational(12, 22);
        rat._corner = 23;
        rat._separation = 34;
        Assert.Equal(rat._corner, 23);
        Assert.Equal(rat._separation, 34);
    }

    [Fact]
    public void Sum_Test()
    {
        Rational rat1 = new Rational(10);
        Rational rat2 = new Rational(20);
        Rational rat3 = new Rational(30);
        Assert.True(rat1 + rat2 == rat3);
    }

    [Fact]
    public void Eq_NotEq_Test()
    {
        Rational rat1 = new Rational(10);
        Rational rat2 = new Rational(20);
        Rational rat3 = new Rational(10);
        Assert.False(rat1 == rat2);
        Assert.True(rat1 != rat2);
        Assert.True(rat1 == rat3);
    }

    [Fact]
    public void Equals_Test()
    {
        Rational rat1 = new Rational(10);
        Rational rat2 = new Rational(10);
        Rational rat3 = new Rational(20);
        Rational? rat4 = null;
            
        Assert.True(rat1.Equals(rat2));
        Assert.False(rat1.Equals(rat3));
        Assert.False(rat1.Equals(rat4));
        Assert.False(rat1.Equals(null));
    }
    
    [Fact]
    public void GetHashCodeTest()
    {
        Rational rat = new Rational(1);
        rat.GetHashCode();
    }
}
