public class MethodsGetStrategy : IStartegy
{
    System.Type _design_class;

    public MethodsGetStrategy(System.Type design_class)
    {
        _design_class = design_class ?? throw new ArgumentNullException(nameof(design_class));
    }

    public object Execute(params object[] args)
    {
        return Hwdtech.IoC.Resolve<System.Object>("Get.Methods.From.Interface", _design_class);
    }
}
