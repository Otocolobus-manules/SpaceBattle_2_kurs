public class WalkerInQueue : IStartegy
{
    Func<object, object> _f_mod;

    public WalkerInQueue(Func<object, object> f_mod)
    {
        _f_mod = f_mod;
    }

    public object Execute(params object[] args)
    {
        return _f_mod(args[0]);
    }
}
