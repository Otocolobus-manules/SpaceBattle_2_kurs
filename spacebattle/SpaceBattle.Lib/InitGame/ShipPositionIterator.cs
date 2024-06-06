public class ShipPositionIterator: IEnumerator<object>  
{
    private IList<Vector> _coord;
    private int _coordind;

    public ShipPositionIterator()
    {
        _coordind = 0;
        _coord = Hwdtech.IoC.Resolve<List<Vector>>("ShipPositions");
    }

    public object Current => _coord[_coordind];

    public bool MoveNext() => ++_coordind < _coord.Count;

    public void Reset() => _coordind = 0;

    public void Dispose() => throw new System.NotImplementedException();                                          

}
