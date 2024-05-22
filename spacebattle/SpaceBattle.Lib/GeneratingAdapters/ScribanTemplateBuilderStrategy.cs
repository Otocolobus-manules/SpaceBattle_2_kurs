using Scriban;
public class ScribanTemplateBuilderStrategy: IStartegy
{
    Scriban.Template _template_compiled;
    
    public ScribanTemplateBuilderStrategy(string template)
    {
        _template_compiled = Template.Parse(template);
    }
    
    public object Execute(params object[] args)
    {
        return _template_compiled.Render(Hwdtech.IoC.Resolve<System.Object>("Get.AttributesAndMethods", args[0]));
    }
}
