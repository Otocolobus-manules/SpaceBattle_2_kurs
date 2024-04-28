﻿namespace SpaceBattle.Lib.Tests;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class HandlerExceptionStrategyTest
{
    public HandlerExceptionStrategyTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CommandExceptionHandler()
    {
        var tree = new Dictionary<string, IExceptionHandler>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Tree", (object[] args) => tree).Execute();
        
        var mockHandler = new Mock<IExceptionHandler>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DefaultExceptionHandler", (object[] args) => { return mockHandler.Object; }).Execute();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Find", (object[] args) => new HandlerExceptionStrategy().Execute(args)).Execute();

        var mockCommand = new Mock<ICommand>();
        var mockException = new Mock<Exception>();

        tree.Add(mockCommand.ToString(), mockHandler.Object);

        var handler = IoC.Resolve<IExceptionHandler>("ExceptionHandler.Find", mockCommand, mockException);
        Assert.Equal(mockHandler.Object, handler);
    }

    [Fact]
    public void ExceptionExceptionHandler()
    {
        var tree = new Dictionary<string, IExceptionHandler>();
        var mockHandler = new Mock<IExceptionHandler>();

        var mockCommand = new Mock<ICommand>();
        var mockException = new Mock<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DefaultExceptionHandler", (object[] args) => { return mockHandler.Object; }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Tree", (object[] args) => tree).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Find",
            (object[] args) => new HandlerExceptionStrategy().Execute(args)
        ).Execute();

        tree.Add(mockException.ToString(), mockHandler.Object);

        var handler = IoC.Resolve<IExceptionHandler>("ExceptionHandler.Find", mockCommand, mockException);

        Assert.Equal(mockHandler.Object, handler);
    }

    [Fact]
    public void CommandAndExceptionExceptionHandler()
    {
        var tree = new Dictionary<string, IExceptionHandler>();
        var mockHandler = new Mock<IExceptionHandler>();

        var mockCommand = new Mock<ICommand>();
        var mockException = new Mock<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DefaultExceptionHandler", (object[] args) => { return mockHandler.Object; }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Tree", (object[] args) => tree).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Find", (object[] args) => new HandlerExceptionStrategy().Execute(args)).Execute();

        tree.Add(mockCommand.ToString() + mockException.ToString(), mockHandler.Object);

        var handler = IoC.Resolve<IExceptionHandler>("ExceptionHandler.Find", mockCommand, mockException);

        Assert.Equal(mockHandler.Object, handler);
    }

    [Fact]
    public void DefaultExceptionHandler()
    {
        var tree = new Dictionary<string, IExceptionHandler>();
        var mockHandler = new Mock<IExceptionHandler>();

        var mockCommand = new Mock<ICommand>();
        var mockException = new Mock<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DefaultExceptionHandler", (object[] args) => { return mockHandler.Object; }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Tree", (object[] args) => tree).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Find",
            (object[] args) => new HandlerExceptionStrategy().Execute(args)
        ).Execute();

        tree.Add(mockHandler.ToString(), mockHandler.Object);

        var handler = IoC.Resolve<IExceptionHandler>("ExceptionHandler.Find", mockCommand, mockException);

        handler.Handle();

        mockHandler.Verify(mc => mc.Handle(), Times.Once());

        Assert.Equal(mockHandler.Object, handler);
    }
}
