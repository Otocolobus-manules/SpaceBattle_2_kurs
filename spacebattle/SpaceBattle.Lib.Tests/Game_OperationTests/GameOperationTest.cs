using System.Diagnostics;
using Game_operation;


public class GameOperationTest
{
    [Fact]
    public void StopGameExeCommandByTimeTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var game_time = 1000000;
        var game_timer = new Stopwatch();
        var empty_command = new Mock<ICommand>();
        empty_command.Setup(p => p.Execute());
        var test_command = new Mock<ICommand>();
        test_command.Setup(p => p.Execute()).Verifiable();
        var test_command_error = new Mock<ICommand>();
        test_command_error.Setup(p => p.Execute()).Verifiable();
        var continue_game_command_execution = (object x) =>
        {
            game_timer.Stop();
            if (game_timer.ElapsedTicks < game_time)
            {
                game_timer.Start();
                return x;
            }
            else
            {
                game_timer.Reset();
                return empty_command.Object;
            }
        };
        var main_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", main_scope).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameExeCommand", (object[] args) => new GameExeCommand(args[0], args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQueue", (object[] args) => new Queue<ICommand>()).Execute();
        var game_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", main_scope);
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => test_command_error.Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Run", (object[] args) => {return continue_game_command_execution(args[0]);}).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", main_scope).Execute();
        var sw = new Stopwatch();
        sw.Start();
        var test_queue = Hwdtech.IoC.Resolve<Queue<ICommand>>("GameQueue");
        test_queue.Enqueue(test_command.Object);
        var game_exe_command_test = Hwdtech.IoC.Resolve<ICommand>("Commands.GameExeCommand", test_queue, game_scope);
        game_exe_command_test.Execute();
        test_command.Verify(p => p.Execute());
    }
    
    [Fact]
    public void GetErrorGameExeCommandWhileRunningTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var game_time = 1000000;
        var game_timer = new Stopwatch();
        var empty_command = new Mock<ICommand>();
        empty_command.Setup(p => p.Execute());
        var test_command = new Mock<ICommand>();
        test_command.Setup(p => p.Execute()).Verifiable();
        var test_command_error = new Mock<ICommand>();
        test_command_error.Setup(p => p.Execute()).Verifiable();
        var continue_game_command_execution = (object x) =>
        {
            game_timer.Stop();
            if (game_timer.ElapsedTicks < game_time)
            {
                game_timer.Start();
                return x;
            }
            else
            {
                game_timer.Reset();
                return empty_command.Object;
            }
        };
        var main_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", main_scope).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameExeCommand", (object[] args) => new GameExeCommand(args[0], args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQueue", (object[] args) => new Queue<ICommand>()).Execute();
        var game_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", main_scope);
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => test_command_error.Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Run", (object[] args) => { return continue_game_command_execution(args[0]); }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", main_scope).Execute();
        var sw = new Stopwatch();
        sw.Start();
        var test_queue = Hwdtech.IoC.Resolve<object>("GameQueue");
        var game_exe_command_test = Hwdtech.IoC.Resolve<ICommand>("Commands.GameExeCommand", test_queue, game_scope);
        game_exe_command_test.Execute();
        test_command_error.Verify(p => p.Execute());
    }
    
    [Fact]
    public void GetErrorGameExeCommandWhileRunningFromeScopeTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        var game_time = 1000000;
        var game_timer = new Stopwatch();
        var empty_command = new Mock<ICommand>();
        empty_command.Setup(p => p.Execute());
        var test_command = new Mock<ICommand>();
        test_command.Setup(p => p.Execute()).Verifiable();
        var test_command_error = new Mock<ICommand>();
        test_command_error.Setup(p => p.Execute()).Verifiable();
        var continue_game_command_execution = (object x) =>
        {
            game_timer.Stop();
            if (game_timer.ElapsedTicks < game_time)
            {
                game_timer.Start();
                return x;
            }
            else
            {
                game_timer.Reset();
                return empty_command.Object;
            }
        };
        var scope_error_key = (new ArgumentException("Unknown IoC dependency key ${key}. Make sure that ${key} has been registered before try to resolve the dependnecy")).Message;
        var error_container = new Dictionary<Object, ICommand>() { {scope_error_key, test_command_error.Object } };
        var exception_hendler_with_scope_error = new DefaultExceptionHandler(error_container);
        var main_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", main_scope).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.GameExeCommand", (object[] args) => new GameExeCommand(args[0], args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQueue", (object[] args) => new Queue<ICommand>()).Execute();
        var game_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", main_scope);
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exception_hendler_with_scope_error.Execute(args[0])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", main_scope).Execute();
        var sw = new Stopwatch();
        sw.Start();
        var test_queue = Hwdtech.IoC.Resolve<Queue<ICommand>>("GameQueue");
        test_queue.Enqueue(test_command.Object);
        var game_exe_command_test = Hwdtech.IoC.Resolve<ICommand>("Commands.GameExeCommand", test_queue, game_scope);
        game_exe_command_test.Execute();
    }
}
