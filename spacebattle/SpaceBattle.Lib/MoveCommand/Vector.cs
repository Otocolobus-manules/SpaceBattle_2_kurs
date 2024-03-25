public class Vector
{

    private int[] coords;
    public int size;

    public Vector(int[] args)
    {
        coords = args;
        size = args.Length;
    }

    public Vector(int Size)
    {
        coords = new int[Size];
        size = Size;
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.size != v2.size) throw new System.ArgumentException();
        
        return new Vector(v1.coords.Zip(v2.coords, (first, second) => first + second).ToArray());;
    }
    
    public static bool operator ==(Vector v1, Vector v2)
    {
        if (v1.size != v2.size) return false;
        
        return v1.coords.Intersect(v2.coords).Count() == v1.size;
    }

    public static bool operator !=(Vector v1, Vector v2)
    {
        return !(v1 == v2);
    }

    public int this[int i]
    {
        get => coords[i];
        set => coords[i] = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not null)
        {
            var v = (Vector)obj;
            if (v.size != size) return false;
            return v.coords.Intersect(coords).Count() == v.size;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return coords.GetHashCode();
    }
}
