using System.Text.RegularExpressions;


public class CodeGenerationStrategys_test
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
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "TemplateBuilderStrategy", (object[] args) =>
        {
            var template = args[0]?.ToString();
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template), "Template cannot be null");
            }
            return new ScribanTemplateBuilderStrategy(template);
        }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CodeGenerationStrategy", (object[] args) => new CodeGenerationStrategy(args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateCode.Modules.Preprocessing", (object[] args) => args[0]).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetDefinitionInCodeGenerationStrategy", (object[] args) => new SetDefinitionInCodeGenerationStrategy(args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Add.Definition.To.Modules", (object[] args) => ((List<String>)args[0]).Zip(((List<String>)args[1]), (a, b) => a + b).ToList()).Execute();

        var test_template = @"
public class {{classname}}{
{{ for attributes_name in attributes  }}
public {{attributes_name}};
{{ end }}
{{ for method_name in methods  }}
public {{method_name}};
{{ end }}
}
";
        var expected_results = @"
public class TestClass{

public System.Collections.Generic.Dictionary<System.Object,System.Object> nexts {get; set;};


public System.Object step_forward() => {return 2 + 2};

}
";
        var methods_get_strategy_test_object = Hwdtech.IoC.Resolve<MethodsGetStrategy>("MethodsGetStrategy", typeof(ISource));
        var attributes_get_strategy_test_object = Hwdtech.IoC.Resolve<AttributesGetStrategy>("AttributesGetStrategy", typeof(ISource));
        var definition_methods_add_strategy_test_object = Hwdtech.IoC.Resolve<SetDefinitionInCodeGenerationStrategy>("SetDefinitionInCodeGenerationStrategy", methods_get_strategy_test_object.Execute());
        var definition_attributes_add_strategy_test_object = Hwdtech.IoC.Resolve<SetDefinitionInCodeGenerationStrategy>("SetDefinitionInCodeGenerationStrategy", attributes_get_strategy_test_object.Execute());
        var test_dictionary_of_attributes_and_methods = new Dictionary<String, System.Object>()
        {
            {"classname", "TestClass"},
            { "attributes", (List<String>)definition_attributes_add_strategy_test_object.Execute(new List<String>() { " {get; set;}" }) },
            { "methods", (List<String>)definition_methods_add_strategy_test_object.Execute(new List<String>() { "() => {return 2 + 2}" }) }
        };
        var code_builder = Hwdtech.IoC.Resolve<CodeGenerationStrategy>("CodeGenerationStrategy", test_template);

        Assert.Equal(expected_results, code_builder.Execute(test_dictionary_of_attributes_and_methods));
    }

    [Fact]
    public void code_generation_null_check()
    {
        Assert.Throws<ArgumentNullException>(() => new CodeGenerationStrategy(null!));
    }
}
