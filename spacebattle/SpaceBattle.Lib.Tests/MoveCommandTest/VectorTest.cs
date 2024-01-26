using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Diagnostics;

public class VectorTest
{
    [Fact]
    public void Create_empty_Vector_Test()
    {
        Vector v = new Vector(It.IsAny<int>());
    }

    [Fact]
    public void Create_fulled_Vector_Test()
    {
        Vector v = new Vector(new int[] { It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>() });
    }

    [Fact]
    public void GetSetItemTest()
    {
        Vector v = new Vector(new int[] { 3, -2, 1, 4 });
        v[1] = 10;
        Assert.Equal(v[1], 10);
    }
    
    [Fact]
    public void Sum_Vector()
    {
        Vector v1 = new Vector(new int[] { 1, 2, 1 });
        Vector v2 = new Vector(new int[] { 3, -2, 1 });
        Vector expected = new Vector(new int[] { 4, 0, 2 });
        Assert.True(v1 + v2 == expected);
    }

    [Fact]
    public void Sum_Vector_with_different_len()
    {
        Vector v1 = new Vector(new int[] { 1, 2, 1 });
        Vector v2 = new Vector(new int[] { 3, -2, 1, 2 });
        Assert.Throws<ArgumentException>(() => v1 + v2);
    }

    [Fact]
    public void Eq_NotEq_VectorTest()
    {
        Vector v1 = new Vector(new int[] { 3, -2, 1 });
        Vector v2 = new Vector(new int[] { 9, -6, 3 });
        Vector v3 = new Vector(new int[] { 3, -2, 1 });
        Assert.False(v1 == v2);
        Assert.True(v1 != v2);
        Assert.True(v1 == v3);
    }

    [Fact]
    public void Eq_different_len()
    {
        Vector v1 = new Vector(new int[] { 3, -2, 1 });
        Vector v2 = new Vector(new int[] { 3, -2, 1, 1 });
        Assert.False(v1 == v2);
    }

    [Fact]
    public void GetHashCodeTest()
    {
        Vector v = new Vector(It.IsAny<int>());
        v.GetHashCode();
    }

    [Fact]
    public void EqualTest()
    {
        Vector v = new Vector(new int[] { 3, -2, 1, 4});
        Vector v1 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.True(v.Equals(v1));
    }

    [Fact]
    public void UnEqualSizeEqualTest()
    {
        Vector v = new Vector(new int[] { 3, -2, 1 });
        Vector v1 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.True(!v.Equals(v1));
    }

    [Fact]
    public void EqualSizeUnequalTest()
    {
        Vector v = new Vector(new int[] { 3, 2, 1, 4 });
        Vector v1 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.True(!v.Equals(v1));
    }

    [Fact]
    public void SumUnequalTest()
    {
        Vector v2 = new Vector(new int[] { 9, -6, 3 });
        Vector v3 = new Vector(new int[] { 3, -2, 1, 4 });
        Assert.Throws<ArgumentException>(() => v2 + v3);
    }
    
    [Fact]
    public void EqualNullTest()
    {
        Vector v = new Vector(new int[] { 3, -2, 1 });
        Vector? v1 = null;
        Assert.False(v.Equals(v1));
        Assert.False(v.Equals(null));
    }
}
