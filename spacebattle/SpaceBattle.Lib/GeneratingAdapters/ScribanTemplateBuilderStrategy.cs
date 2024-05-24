using Scriban;


public class ScribanTemplateBuilderStrategy : IStartegy
{
    Scriban.Template _template_compiled;

    public ScribanTemplateBuilderStrategy(string template)
    {
        if (string.IsNullOrEmpty(template))
        {
            throw new ArgumentNullException(nameof(template), "Template cannot be null or empty");
        }
        _template_compiled = Template.Parse(template);
    }

    public object Execute(params object[] args)
    {
        if (args == null || args.Length == 0 || args[0] == null)
        {
            throw new ArgumentException("Arguments cannot be null or empty");
        }

        return _template_compiled.Render(Hwdtech.IoC.Resolve<System.Object>("Get.AttributesAndMethods", args[0]));
    }
}
