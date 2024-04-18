public class HandlerExceptionStrategy: IStartegy
{
    public object Execute(params object[] args)
    {
        var _keyCmd = args[0].ToString();
        var _keyException = args[1].ToString();

        var tree = Hwdtech.IoC.Resolve<Dictionary<string, IExceptionHandler>>("ExceptionHandler.Tree");

        if (tree.ContainsKey(_keyCmd + _keyException))
        {
            var handlerCmdExc = tree[_keyCmd + _keyException];
            return handlerCmdExc;
        }

        else
        {
            return Hwdtech.IoC.Resolve<IExceptionHandler>("DefaultExceptionHandler");
        }
    }
}
