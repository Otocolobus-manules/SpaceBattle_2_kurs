public class IterGeneratorCommand: ICommand
{
    object _iterator;
    object _stop;
    
    public IterGeneratorCommand(object iterator, object stop)
    {
        _iterator = iterator;
        _stop = stop;
    }
    
    public void Execute()
    {
        Hwdtech.IoC.Resolve<ICommand>("IterGeneratorCommand.Run", _iterator, _stop, this).Execute();
    }
}
