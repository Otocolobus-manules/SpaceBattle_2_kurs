public class SetDefinitionInCodeGenerationStrategy : IStartegy
{
    object _definition;

    public SetDefinitionInCodeGenerationStrategy(object definition)
    {
        _definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }

    public object Execute(params object[] args)
    {
        if (args == null || args.Length == 0 || args[0] == null)
        {
            throw new ArgumentException("Arguments cannot be null or empty");
        }

        return Hwdtech.IoC.Resolve<System.Object>("Add.Definition.To.Modules", _definition, args[0]);
    }
}
