public class MacroCommandStrategy: IStartegy
{
    public object Execute(params object[] args)
    {
        var commands = new List<ICommand>();
        args.ToList().ForEach(cmd =>
        {
            commands.Add(Hwdtech.IoC.Resolve<ICommand>((string)cmd));
        });
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MacroCommand", (object[] args) => { return new MacroCommand((ICommand[])args); }).Execute();
        return Hwdtech.IoC.Resolve<ICommand>("Commands.MacroCommand", commands.ToArray());
    }
}
