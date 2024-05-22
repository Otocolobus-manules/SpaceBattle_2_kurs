namespace Game_operation;


public class DefaultExceptionHandler: IStartegy
{
    object _exception_container;
    public DefaultExceptionHandler(object exception_container){
        _exception_container = exception_container;
    }
    public object Execute(params object[] args){
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => ((Dictionary<Object, ICommand>)args[0])[((System.Exception)args[1]).Message]).Execute();
        return Hwdtech.IoC.Resolve<System.Object>("Get.Result.FromDefaultExceptionHandler", _exception_container, args[0]);
    }
}
