namespace MessagePreprocess;


public class DefaultExceptionHandler: IStartegy
{
    object _exception_container;
    public DefaultExceptionHandler(object exception_container){
        _exception_container = exception_container;
    }

    public object Execute(params object[] args){
        return Hwdtech.IoC.Resolve<System.Object>("Get.Result.FromDefaultExceptionHandler", _exception_container, args[0]);
    }
}
