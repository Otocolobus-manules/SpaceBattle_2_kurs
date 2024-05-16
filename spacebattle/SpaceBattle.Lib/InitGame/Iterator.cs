public class Iterator
{
    public object _position { get; protected set; }
    object _step_function;
    
    public Iterator(object start, object step_function){
        _position = start;
        _step_function = step_function;
    }
    
    public object next(){

        _position = Hwdtech.IoC.Resolve<System.Object>("Iterator.Step", _step_function, _position);
        return _position;
    }
}
