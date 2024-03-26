using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Diagnostics;

public class VectorTest
{
    [Fact]
    public void Create_empty_Vector_Test()
    {
        var v = new Vector(It.IsAny<int>());
    }

    [Fact]
    public void Create_fulled_Vector_Test()
    {
        var v = new Vector(new int[] { It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>() });
    }

    [Fact]
    public void GetSetItemTest()
    {
        var v = new Vector(new int[] { 3, -2, 1, 4 });
        v[1] = 10;
        Assert.Equal(10, v[1]);
    }

    [Fact]
    public void Sum_Vector()
    {
        var v1 = new Vector(new int[] { 1, 2, 1 });
        var v2 = new Vector(new int[] { 3, -2, 1 });
        var expected = new Vector(new int[] { 4, 0, 2 });
        Assert.True(v1 + v2 == expected);
    }

    [Fact]
    public void Sum_Vector_with_different_len()
    {
        var v1 = new Vector(new int[] { 1, 2, 1 });
        var v2 = new Vector(new int[] { 3, -2, 1, 2 });
        Assert.Throws<ArgumentException>(() => v1 + v2);
    }

    [Fact]
    public void Eq_NotEq_VectorTest()
    {
        var v1 = new Vector(new int[] { 3, -2, 1 });
        var v2 = new Vector(new int[] { 9, -6, 3 });
        var v3 = new Vector(new int[] { 3, -2, 1 });
        Assert.False(v1 == v2);
        Assert.True(v1 != v2);
        Assert.True(v1 == v3);
    }

    [Fact]
    public void Eq_different_len()
    {
        var v1 = new Vector(new int[] { 3, -2, 1 });
        var v2 = new Vector(new int[] { 3, -2, 1, 1 });
        Assert.False(v1 == v2);
    }

    [Fact]
    public void GetHashCodeTest()
    {
        var v = new Vector(It.IsAny<int>());
        v.GetHashCode();
    }

    [Fact]
    public void EqualTest()
    {
        var v = new Vector(new int[] { 3, -2, 1, 4});
        var v1 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.True(v.Equals(v1));
    }

    [Fact]
    public void UnEqualSizeEqualTest()
    {
        var v = new Vector(new int[] { 3, -2, 1 });
        var v1 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.True(!v.Equals(v1));
    }

    [Fact]
    public void EqualSizeUnequalTest()
    {
        var v = new Vector(new int[] { 3, 2, 1, 4 });
        var v1 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.True(!v.Equals(v1));
    }

    [Fact]
    public void SumUnequalTest()
    {
        var v2 = new Vector(new int[] { 9, -6, 3 });
        var v3 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.Throws<ArgumentException>(() => v2 + v3);
    }
    

    [Fact]
    public void EqualNullTest()
    {
        var v = new Vector(new int[] { 3, -2, 1 });
        Vector? v1 = null;
        Assert.False(v.Equals(v1));
        Assert.False(v.Equals(null));
    }
}
