public class SpeedChange: ICommand
{
    private IUObject _obj;
    private Vector _velocity;

    public SpeedChange(object obj, Vector velocity)
    {
        _obj = (IUObject)obj;
        _velocity = velocity;
    }

    public void Execute()
    {
        _obj.set_property("velocity", _velocity);
    }
}
