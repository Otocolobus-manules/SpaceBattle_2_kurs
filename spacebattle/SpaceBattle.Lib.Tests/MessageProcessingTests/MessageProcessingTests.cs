﻿using System.Collections.Concurrent;

public class MessageProcessingTests
{
    [Fact]
    public void EndPoint_test_get_set()
    {

        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) =>  ((IGameIdContainer)args[0]).game_id).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => ((IArgContainer)args[0]).args).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.CommandName.FromGameCommandMessage", (object[] args) =>  ((ICommandNameContainer)args[0]).command_name).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        game_command_message_for_test.command_name = "cmd1";
        game_command_message_for_test.game_id = "game1";
        game_command_message_for_test.args = new List<string>(){ "game_object", "another_parametr" };
        
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = (Dictionary<string, object>)end_point_for_test.get_message(game_command_message_for_test);
        
        Assert.Equal(game_command_message_for_test.command_name, results["command"]);
        Assert.Equal(game_command_message_for_test.game_id, results["game_id"]);
        Assert.Equal(game_command_message_for_test.args, results["args"]);
    }

    [Fact]
    public void EndPoint_test_error_on_game_id_check()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) => {throw new System.Exception("test exception thrown"); return new object();}).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => ((IArgContainer)args[0]).args).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.CommandName.FromGameCommandMessage", (object[] args) => ((ICommandNameContainer)args[0]).command_name).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        game_command_message_for_test.command_name = "cmd1";
        game_command_message_for_test.game_id = "game1";
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = end_point_for_test.get_message(game_command_message_for_test);
        
        Assert.Equal("It\'s error", results);
    }

    [Fact]
    public void EndPoint_test_error_on_command_name_check()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) => ((IGameIdContainer)args[0]).game_id).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.CommandName.FromGameCommandMessage", (object[] args) => { throw new System.Exception("test exception thrown"); return new object(); }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => ((IArgContainer)args[0]).args).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        game_command_message_for_test.command_name = "cmd1";
        game_command_message_for_test.game_id = "game1";
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = end_point_for_test.get_message(game_command_message_for_test);
        
        Assert.Equal("It\'s error", results);
    }

    [Fact]
    public void EndPoint_test_error_on_args_check()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) => ((IGameIdContainer)args[0]).game_id).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => { throw new System.Exception("test exception thrown"); return new object(); }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        game_command_message_for_test.command_name = "cmd1";
        game_command_message_for_test.game_id = "game1";
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = end_point_for_test.get_message(game_command_message_for_test);
        
        Assert.Equal("It\'s error", results);
    }

    [Fact]
    public void EndPoint_test_get_set_if_null()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) => ((IGameIdContainer)args[0]).game_id).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => ((IArgContainer)args[0]).args).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.CommandName.FromGameCommandMessage", (object[] args) => ((ICommandNameContainer)args[0]).command_name).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = (Dictionary<string, object>)end_point_for_test.get_message(game_command_message_for_test);
        
        Assert.Equal(game_command_message_for_test.command_name, results["command"]);
        Assert.Equal(game_command_message_for_test.game_id, results["game_id"]);
        Assert.Equal(game_command_message_for_test.args, results["args"]);
    }

    [Fact]
    public void EndPoint_test_set_to_queue_with_classes()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) => ((IGameIdContainer)args[0]).game_id).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => ((IArgContainer)args[0]).args).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.CommandName.FromGameCommandMessage", (object[] args) => ((ICommandNameContainer)args[0]).command_name).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.ObjectIdContainer", (object[] args) => new ObjectIdContainer((Func<object, object, object>)args[0], (object)args[1])).Execute();
        
        var test_command = new Mock<ICommand>();
        test_command.Setup(p => p.Execute()).Verifiable();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "TestCMD", (object[] args) => test_command.Object).Execute();
        Func<object, object, object> func_gamequeue_search_in_idlist = (object id, object container) => ((Dictionary<string, BlockingCollection<ICommand>>)container)[(string)id];
        var game1_queue = new BlockingCollection<ICommand>() { };
        var id_queuegame_dictionary = new Dictionary<string, BlockingCollection<ICommand>>() { { "game1", game1_queue } };
        var id_game_container = Hwdtech.IoC.Resolve<ObjectIdContainer>("Get.ObjectIdContainer", func_gamequeue_search_in_idlist, id_queuegame_dictionary);
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        game_command_message_for_test.command_name = "TestCMD";
        game_command_message_for_test.game_id = "game1";
        game_command_message_for_test.args = new List<string>() { };
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = (Dictionary<string, object>)end_point_for_test.get_message(game_command_message_for_test);
        var test_queue_get = (BlockingCollection<ICommand>)id_game_container.Execute(results["game_id"]);
        var test_cmd_get = Hwdtech.IoC.Resolve<ICommand>((string)results["command"]);
        
        Assert.Equal(test_cmd_get, test_command.Object);
        Assert.Equal(test_queue_get, game1_queue);
        test_queue_get.TryAdd(test_cmd_get);
        test_queue_get.Take().Execute();
        test_command.Verify(p => p.Execute(), Times.Once());
    }

    [Fact]
    public void EndPoint_test_set_to_queue()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessageGetter", (object[] args) => new GameCommandMessageGetter()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessage", (object[] args) => new GameCommandMessage()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMessagePreprocessing", (object[] args) => new GameCommandMessagePreprocessingStrategy()).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommandMassegeGetterExceptionHandler", (object[] args) => new DefaultExceptionHandler(new object())).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.GameId.FromGameCommandMessage", (object[] args) => ((IGameIdContainer)args[0]).game_id).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Args.FromGameCommandMessage", (object[] args) => ((IArgContainer)args[0]).args).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.CommandName.FromGameCommandMessage", (object[] args) => ((ICommandNameContainer)args[0]).command_name).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Result.FromDefaultExceptionHandler", (object[] args) => "It\'s error").Execute();
        
        var test_command = new Mock<ICommand>();
        test_command.Setup(p => p.Execute()).Verifiable();
        var func_gamequeue_search = (object z) => z;
        var game1_queue = new BlockingCollection<ICommand>() { };
        var id_queuegame_container = new Dictionary<string, BlockingCollection<ICommand>>(){{"game1", game1_queue }};
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "TestCMD", (object[] args) => test_command.Object).Execute();
        var game_command_message_for_test = Hwdtech.IoC.Resolve<GameCommandMessage>("GameCommandMessage");
        game_command_message_for_test.command_name = "TestCMD";
        game_command_message_for_test.game_id = "game1";
        game_command_message_for_test.args = new List<string>() {};
        var end_point_for_test = Hwdtech.IoC.Resolve<GameCommandMessageGetter>("GameCommandMessageGetter");
        var results = (Dictionary<string, object>)end_point_for_test.get_message(game_command_message_for_test);
        var test_queue_get = id_queuegame_container[(string)results["game_id"]];
        var test_cmd_get = Hwdtech.IoC.Resolve<ICommand>((string)results["command"]);
        
        Assert.Equal(test_cmd_get, test_command.Object);
        Assert.Equal(test_queue_get, game1_queue);
        test_queue_get.TryAdd(test_cmd_get);
        test_queue_get.Take().Execute();
        test_command.Verify(p => p.Execute(), Times.Once());
    }
}
