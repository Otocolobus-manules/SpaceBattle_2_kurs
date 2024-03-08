public class WalkerInQueueWithStopByEmptyQueue : IStartegy
{
    Func<object, object> _f_mod;
    ServerThread _thread;

    public WalkerInQueueWithStopByEmptyQueue(Func<object, object> f_mod, ServerThread thread)
    {
        _f_mod = f_mod;
        _thread = thread;
    }

    public object Execute(params object[] args)
    {
        if (Hwdtech.IoC.Resolve<System.Boolean>("Commands.IsQueueEmpty",
                args[0])) return Hwdtech.IoC.Resolve<ICommand>("Commands.HardStopCommand", _thread);
        else return _f_mod(args[0]);
    }
}
