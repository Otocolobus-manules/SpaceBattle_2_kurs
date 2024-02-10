public class Vector
{

    private readonly int[] _coords;
    public readonly int _size;

    public Vector(int[] args)
    {
        _coords = args;
        _size = args.Length;
    }

    public Vector(int size)
    {
        _coords = new int[size];
        _size = size;
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1._size != v2._size) throw new System.ArgumentException();
        var x = new Vector(v1._size);
        for (var i = 0; i < v1._size; i++)
        {
            x._coords[i] = v1._coords[i] + v2._coords[i];
        }
        return x;
    }
    
    public static bool operator ==(Vector v1, Vector v2)
    {
        if (v1._size != v2._size) return false;
        for (var i = 0; i < v1._size; i++) if (v1._coords[i] != v2._coords[i]) return false;
        return true;
    }

    public static bool operator !=(Vector v1, Vector v2)
    {
        return !(v1 == v2);
    }

    public int this[int i]
    {
        get => _coords[i];
        set => _coords[i] = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not null)
        {
            var v = (Vector)obj;
            if (v._size != _size) return false;
            for (var i = 0; i < v._size; i++) if (v._coords[i] != _coords[i]) return false;
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return _coords.GetHashCode();
    }
}
