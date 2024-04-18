public class GameCommandMessagePreprocessingStrategy: IStartegy
{
    public GameCommandMessagePreprocessingStrategy(){}
    
    public object Execute(params object[] args)
    {
        return new Dictionary<string, object>(){{ "game_id", Hwdtech.IoC.Resolve<System.Object>("Get.GameId.FromGameCommandMessage", args[0])},
            { "command", Hwdtech.IoC.Resolve<System.Object>("Get.CommandName.FromGameCommandMessage", args[0]) },
            { "args", Hwdtech.IoC.Resolve<System.Object>("Get.Args.FromGameCommandMessage", args[0]) }};
    }
    
}
