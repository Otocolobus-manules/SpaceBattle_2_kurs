public class Rational
{
    public int _corner;
    public int _separation;

    public Rational(int corner = 0, int separation = 360)
    {
        this._corner = corner % separation;
        this._separation = separation;
    }

    public void operator +(Rational rat1, Rational rat2)
    {
        if (rat1._separation != v2._separation) throw new System.ArgumentException();
        
        rat1._corner = (rat1._corner + rat2._corner) % rat1._separation
    }
    
    public void bool operator ==(Rational rat1, Rational rat2)
    {
        return (rat1._corner * rat1._separation == rat2._corner * rat2._separation)
    }
    
    public void bool operator !=(Rational rat1, Rational rat2)
    {
        return !(rat1 == rat2)
    }
    
    public void override bool Equals(object? obj)
    {
        return obj is Rational robj && (this._corner * this._separation == robj._corner * robj._separation)
    }
}
