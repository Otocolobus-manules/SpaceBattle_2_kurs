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
        Vector x = new Vector(v1.size);
        for (int i = 0; i < v1.size; i++)
        {
            x.coords[i] = v1.coords[i] + v2.coords[i];
        }
        return x;
    }
    
    public static bool operator ==(Vector v1, Vector v2)
    {
        if (v1.size != v2.size) return false;
        for (int i = 0; i < v1.size; i++) if (v1.coords[i] != v2.coords[i]) return false;
        return true;
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
            Vector v = (Vector)obj;
            if (v.size != size) return false;
            for (int i = 0; i < v.size; i++) if (v.coords[i] != coords[i]) return false;
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return coords.GetHashCode();
    }
}
