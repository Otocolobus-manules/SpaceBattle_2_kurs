public class AttributesGetStrategy: IStartegy
{
    System.Type _design_class;
    
    public AttributesGetStrategy(System.Type design_class)
    {
        _design_class = design_class;
    }
    
    public object Execute(params object[] args)
    {
        return Hwdtech.IoC.Resolve<IEnumerable<System.String>>("Get.Atributes.From.Interface", _design_class);
    }
}
