public class SetDefinitionInCodeGenerationStrategy: IStartegy
{
    object _definition;
    
    public SetDefinitionInCodeGenerationStrategy(object definition)
    {
        _definition = definition;
    }
    
    public object Execute(params object[] args)
    {
        return Hwdtech.IoC.Resolve<System.Object>("Add.Definition.To.Modules", _definition, args[0]);
    }
}
