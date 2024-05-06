public class GameExeCommand: ICommand
{
    public object _queue { get; }
    object _scope;
    public GameExeCommand(object queue, object scope)
    {
        _queue = queue;
        _scope = scope;
    }
    public void Execute()
    {
        try{
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetScope", (object[] args) => new StandartSetScopeCommand(args[0])).Execute();
            Hwdtech.IoC.Resolve<ICommand>("SetScope", _scope).Execute();
            
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.GetCommand", (object[] args) => ((Queue<ICommand>)args[0]).Dequeue()).Execute();
            Hwdtech.IoC.Resolve<ICommand>("Queue.GetCommand", _queue).Execute();
            
            Hwdtech.IoC.Resolve<ICommand>("Game.Run", this).Execute();
        }
        catch (System.Exception e){
            Hwdtech.IoC.Resolve<ICommand>("Game.ExceptionHandler", e).Execute();
        }
    }
}
