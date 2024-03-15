using Hwdtech;

public class CheckCollisionCommand : ICommand
{
    private readonly IUObject _obj1, _obj2;

    public CheckCollisionCommand(IUObject obj1, IUObject obj2)
    {
        _obj1 = obj1;
        _obj2 = obj2;
    }

    public void Execute()
    {
        var position1 = Hwdtech.IoC.Resolve<List<int>>("Commands.GetProperty", _obj1, "position");
        var velocity1 = Hwdtech.IoC.Resolve<List<int>>("Commands.GetProperty", _obj1, "velocity");
        var position2 = Hwdtech.IoC.Resolve<List<int>>("Commands.GetProperty", _obj2, "position");
        var velocity2 = Hwdtech.IoC.Resolve<List<int>>("Commands.GetProperty", _obj2, "velocity");

        var variations = Hwdtech.IoC.Resolve<List<int>>("Commands.DfVec", position1, position2, velocity1, velocity2);

        var collisionTree = Hwdtech.IoC.Resolve<IDictionary<int, object>>("Commands.CollisionTree");

        variations.ForEach(variation => collisionTree = (IDictionary<int, object>)collisionTree[variation]);

        Hwdtech.IoC.Resolve<ICommand>("Commands.Collision", _obj1, _obj2).Execute();
    }
}
