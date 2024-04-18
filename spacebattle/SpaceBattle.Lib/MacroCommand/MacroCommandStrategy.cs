public class MacroCommandStrategy: IStartegy
{
    public object Execute(params object[] args)
    {
        var commands = new List<ICommand>();
        args.ToList().ForEach(cmd =>
        {
            commands.Add(Hwdtech.IoC.Resolve<ICommand>((string)cmd));
        });
        return Hwdtech.IoC.Resolve<ICommand>("Commands.MacroCommand", commands.ToArray());
    }
}
