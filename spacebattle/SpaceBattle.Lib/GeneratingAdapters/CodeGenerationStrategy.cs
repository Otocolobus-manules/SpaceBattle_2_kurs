public class CodeGenerationStrategy: IStartegy
{
    object _template;
    
    public CodeGenerationStrategy(object template)
    {
        _template = template;
    }
    
    public object Execute(params object[] args)
    {
        return Hwdtech.IoC.Resolve<IStartegy>("TemplateBuilderStrategy", _template).Execute(Hwdtech.IoC.Resolve<System.Object>("GenerateCode.Modules.Preprocessing", args));
    }
}
