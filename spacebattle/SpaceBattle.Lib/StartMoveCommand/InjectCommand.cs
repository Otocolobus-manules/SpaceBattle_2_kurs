public class InjectCommand: ICommand, IInjectable
{
    private ICommand cmd;

    public InjectCommand(ICommand input_cmd) => cmd = input_cmd;

    public void Execute() => cmd.Execute();

    public void Inject(ICommand obj) => cmd = obj;
}
