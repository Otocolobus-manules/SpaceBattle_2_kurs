using System.Text.RegularExpressions;


public class SetDefinitionInCodeGenerationStrategy_test
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
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SetDefinitionInCodeGenerationStrategy", (object[] args) => new SetDefinitionInCodeGenerationStrategy(args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Add.Definition.To.Modules", (object[] args) => ((List<String>)args[0]).Zip(((List<String>)args[1]), (a, b) => a + b).ToList()).Execute();

        var methods_get_strategy_test_object = Hwdtech.IoC.Resolve<MethodsGetStrategy>("MethodsGetStrategy", typeof(ISource));
        var definition_add_strategy_test_object = Hwdtech.IoC.Resolve<SetDefinitionInCodeGenerationStrategy>("SetDefinitionInCodeGenerationStrategy", methods_get_strategy_test_object.Execute());
        var expected_results = new List<String>() { "System.Object step_forward() => {return 2 + 2}" };

        definition_add_strategy_test_object.Execute((List<String>)methods_get_strategy_test_object.Execute());
        Assert.Equal(expected_results, definition_add_strategy_test_object.Execute(new List<String>() { "() => {return 2 + 2}" }));
    }

    [Fact]
    public void set_definition_null_check()
    {
        Assert.Throws<ArgumentNullException>(() => new SetDefinitionInCodeGenerationStrategy(null!));
    }

    [Fact]
    public void execute_with_null_args_check()
    {
        var definition = "Some definition";
        var strategy = new SetDefinitionInCodeGenerationStrategy(definition);
        Assert.Throws<ArgumentException>(() => strategy.Execute(null!));
    }

    [Fact]
    public void execute_with_empty_args_check()
    {
        var definition = "Some definition";
        var strategy = new SetDefinitionInCodeGenerationStrategy(definition);
        Assert.Throws<ArgumentException>(() => strategy.Execute(new object[] { }));
    }
}
