using CoreWCF;


[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class GameCommandMessageGetter: IGameCommandEndPoint
{
    public object get_message(GameCommandMessage param)
    {
        try{
            return Hwdtech.IoC.Resolve<IStartegy>("GameCommandMessagePreprocessing").Execute(param);
        }
        catch (System.Exception e){
            return Hwdtech.IoC.Resolve<IStartegy>("GameCommandMassegeGetterExceptionHandler").Execute(e, param);
        }
        
    }
}
