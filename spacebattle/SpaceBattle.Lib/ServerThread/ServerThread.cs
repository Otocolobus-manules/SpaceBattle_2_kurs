using System.Collections.Concurrent;


public class ServerThread : IStartegy, IMethodChangeable, IStopable
{
    IStartegy _s;
    bool _run = true;
    readonly Thread _thread;
    BlockingCollection<ICommand> _queue;

    public ServerThread(IStartegy s, BlockingCollection<ICommand> x)
    {
        _s = s;
        _queue = x;

        _thread = new Thread(() =>
        {
            while (_run)
            {
                ((ICommand)_s.Execute(_queue)).Execute();
            }
        });
    }

    public void ChangeMethod(IStartegy s)
    {
        if (_thread == Thread.CurrentThread)
        {
            _s = s;
        }
    }

    public void Stop()
    {
        if (_thread == Thread.CurrentThread)
        {
            _run = false;
        }
    }

    public object Execute(params object[] args)
    {
        return (object)_thread;
    }
}
