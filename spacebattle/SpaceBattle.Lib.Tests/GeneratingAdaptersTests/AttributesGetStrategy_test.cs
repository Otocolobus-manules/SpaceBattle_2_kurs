using System.Text.RegularExpressions;


public class AttributesGetStrategy_test
{
    [Fact]
    public void standart_get_atributes()
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
                result = Regex.Replace(result, "\\(.*\\)", String.Empty);
                return result;
            }).ToList<System.String>();
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "AttributesGetStrategy", (object[] args) => new AttributesGetStrategy((System.Type)args[0])).Execute();

        var attributes_get_strategy_test_object = Hwdtech.IoC.Resolve<AttributesGetStrategy>("AttributesGetStrategy", typeof(ISource));
        var expected_results = new List<System.String>() { "System.Collections.Generic.Dictionary<System.Object,System.Object> nexts" };

        Assert.Equal(expected_results, attributes_get_strategy_test_object.Execute());
    }

    [Fact]
    public void get_attributes_null_check()
    {
        Assert.Throws<ArgumentNullException>(() => new AttributesGetStrategy(null!));
    }
}
