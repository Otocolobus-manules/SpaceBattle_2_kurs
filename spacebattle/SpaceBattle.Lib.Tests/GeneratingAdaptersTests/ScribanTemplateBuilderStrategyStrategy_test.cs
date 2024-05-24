using System.Text.RegularExpressions;


public class ScribanTemplateBuilderStrategy_test
{
    [Fact]
    public void standart_get_code_generation()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Atributes.From.Interface", (object[] args) =>
        {
            var properties = ((System.Type)args[0]).GetProperties().Where(m => !m.IsSpecialName);
            return properties.Select(i =>
            {
                var propertyString = i.ToString();
                if (propertyString == null)
                {
                    throw new ArgumentNullException(nameof(propertyString), "Property string cannot be null");
                }
                var result = Regex.Replace(propertyString, @"`\d\[([^\[\]]+)\]", "<$1>");
                return result;
            }).ToList<System.String>();
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AttributesGetStrategy", (object[] args) => new AttributesGetStrategy((System.Type)args[0])).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Methods.From.Interface", (object[] args) =>
        {
            var methods = ((System.Type)args[0]).GetMethods().Where(m => !m.IsSpecialName);
            return methods.Select(i =>
            {
                var methodString = i.ToString();
                if (methodString == null)
                {
                    throw new ArgumentNullException(nameof(methodString), "Method string cannot be null");
                }
                var result = Regex.Replace(Regex.Replace(methodString, @"`\d\[([^\[\]]+)\]", "<$1>"), "\\(.*\\)", String.Empty);
                return result;
            }).ToList<System.String>();
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MethodsGetStrategy", (object[] args) => new MethodsGetStrategy((System.Type)args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.TypeOf.ModulesMass", (object[] args) => args[0]).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.AttributesAndMethods", (object[] args) => args[0]).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ScribanTemplateBuilderStrategy", (object[] args) => new ScribanTemplateBuilderStrategy(args[0]?.ToString() ?? throw new ArgumentNullException(nameof(args), "Template cannot be null"))).Execute();

        var test_template = @"
public class TestClassName{
{{ for attributes_name in attributes  }}
public {{attributes_name}};
{{ end }}
{{ for method_name in methods  }}
public {{method_name}};
{{ end }}
}
";
        var expected_results = @"
public class TestClassName{

public System.Object step_forward;


public System.Collections.Generic.Dictionary<System.Object,System.Object> nexts;

}
";
        var methods_get_strategy_test_object = Hwdtech.IoC.Resolve<MethodsGetStrategy>("MethodsGetStrategy", typeof(ISource));
        var attributes_get_strategy_test_object = Hwdtech.IoC.Resolve<AttributesGetStrategy>("AttributesGetStrategy", typeof(ISource));
        var test_dictionary_of_attributes_and_methods = new Dictionary<String, IEnumerable<String>>()
        {
            {"attributes", (List<String>)methods_get_strategy_test_object.Execute()},
            { "methods", (List<String>)attributes_get_strategy_test_object.Execute() }
        };
        var scriban_builder = Hwdtech.IoC.Resolve<ScribanTemplateBuilderStrategy>("ScribanTemplateBuilderStrategy", test_template);

        Assert.Equal(expected_results, scriban_builder.Execute(test_dictionary_of_attributes_and_methods));
    }

    [Fact]
    public void scriban_template_null_check()
    {
        Assert.Throws<ArgumentNullException>(() => new ScribanTemplateBuilderStrategy((string)null!));
    }

    [Fact]
    public void execute_with_null_args_check()
    {
        var template = @"
public class TestClassName{
{{ for attributes_name in attributes  }}
public {{attributes_name}};
{{ end }}
{{ for method_name in methods  }}
public {{method_name}};
{{ end }}
}
";
        var scriban_builder = new ScribanTemplateBuilderStrategy(template);
        Assert.Throws<ArgumentException>(() => scriban_builder.Execute(null!));
    }
}
