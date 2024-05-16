public class IterGenTest
{
    [Fact]
    public void get_id_IUObject_fueld_pairs_test()
    {
        var game_scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope).Execute();
        
        var empty_command = new Mock<ICommand>();
        empty_command.Setup(p => p.Execute());
        var test_list = new List<Mock<IUObject>>() { };
        
        var generate_id_iuobject_fueled_paires = (object x) =>
        {
            var test_iuobject = new Mock<IUObject>();
            test_iuobject.Setup(p => p.SetProperty("fuel", It.IsAny<Object>())).Verifiable();
            test_iuobject.Setup(p => p.SetProperty("position", It.IsAny<Object>())).Verifiable();
            test_list.Add(test_iuobject);
            test_iuobject.Object.SetProperty("fuel", It.IsAny<Object>());
            test_iuobject.Object.SetProperty("position", It.IsAny<Object>());
            ((Dictionary<Object, IUObject>)x).Add(test_iuobject.Object.GetHashCode(), test_iuobject.Object);
            return x;
        };
        
        var stop_criterion = (object iter, object gen) =>
        {
            if (((Dictionary<Object, IUObject>)(((Iterator)iter)._position)).Count() < 3)
            {
                ((Iterator)iter).next();
                return gen;
            }
            else return empty_command.Object;
        };
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Iterator.Step", (object[] args) => ((Func<System.Object, System.Object>)args[0])(args[1])).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "IterGeneratorCommand.Run", (object[] args) => ((Func<System.Object, System.Object, System.Object>)args[1])(args[0], args[2])).Execute();
        
        var test_iterator = new Iterator(new Dictionary<Object, IUObject>() { }, generate_id_iuobject_fueled_paires);
        var test_iter_generator_command = new IterGeneratorCommand(test_iterator, stop_criterion);
        test_iter_generator_command.Execute();
        Assert.Equal(3, ((Dictionary<Object, IUObject>)test_iterator._position).Count());
        test_list.ForEach(i => i.Verify(p => p.SetProperty("fuel", It.IsAny<Object>())));
        test_list.ForEach(i => i.Verify(p => p.SetProperty("position", It.IsAny<Object>())));
    }
}
