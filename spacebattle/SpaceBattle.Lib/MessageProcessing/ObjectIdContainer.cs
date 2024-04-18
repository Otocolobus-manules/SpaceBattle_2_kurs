public class ObjectIdContainer: IStartegy
{
    private Func<object, object, object> func_of_search;
    private object container;
    
    public ObjectIdContainer(Func<object, object, object> func_of_search, object container)
    {
        this.container = container;
        this.func_of_search = func_of_search;
    }
    
    public object Execute(params object[] arg)
    {
        return func_of_search(arg[0], container);
    }
    
}
