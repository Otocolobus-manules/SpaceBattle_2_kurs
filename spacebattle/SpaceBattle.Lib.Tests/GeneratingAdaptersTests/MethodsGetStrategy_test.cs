using System.Text.RegularExpressions;

public class MethodsGetStrategy_test
{
    [Fact]
    public void standart_get_methods() 
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Methods.From.Interface", (object[] args) => ((System.Type)args[0]).GetMethods().Where(m => !m.IsSpecialName).Select(i => 
        {
            var methodString = i.ToString();
            if (methodString == null)
            {
                throw new ArgumentNullException(nameof(methodString), "Method string cannot be null");
            }
            var result = Regex.Replace(methodString, @"`\d\[([^\[\]]+)\]", "<$1>");
            result = Regex.Replace(result, "\\(.*\\)", String.Empty);
            return result;
        }).ToList<System.String>()).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MethodsGetStrategy", (object[] args) => new MethodsGetStrategy((System.Type)args[0])).Execute();

        var attributes_get_strategy_test_object = Hwdtech.IoC.Resolve<MethodsGetStrategy>("MethodsGetStrategy", typeof(ISource));
        var expected_results = new List<System.String>() { "System.Object step_forward" };

        Assert.Equal(expected_results, attributes_get_strategy_test_object.Execute());
    }
}
