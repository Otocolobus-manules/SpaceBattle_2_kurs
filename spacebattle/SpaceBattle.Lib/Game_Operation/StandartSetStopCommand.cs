public class StandartSetScopeCommand: ICommand
{
    object _scope;
    public StandartSetScopeCommand(object scope)
    {
        _scope = scope;
    }

    public void Execute()
    {
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
    }
}
