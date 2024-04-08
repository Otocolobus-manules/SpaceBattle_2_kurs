using Hwdtech;

public class CheckCollisionCommand: ICommand
{
    private readonly IUObject _obj1, _obj2;

    public CheckCollisionCommand(IUObject obj1, IUObject obj2)
    {
        _obj1 = obj1;
        _obj2 = obj2;
    }

    public void Execute()
    {
        var cool = true;
        var position1 = Hwdtech.IoC.Resolve<Vector>("Object1GetProperty", _obj1, "position");
        var position2 = Hwdtech.IoC.Resolve<Vector>("Object2GetProperty", _obj2, "position");
        var velocity1 = Hwdtech.IoC.Resolve<Vector>("Object1GetProperty", _obj1, "velocity");
        var velocity2 = Hwdtech.IoC.Resolve<Vector>("Object2GetProperty", _obj2, "velocity");

        var vec = Hwdtech.IoC.Resolve<List<int>>("DifVec", position1, position2, velocity1, velocity2);
        var node = Hwdtech.IoC.Resolve<Dictionary<int, object>>("CollisionTree");

        vec.ForEach(n => {
            if ( node.ContainsKey(n) ) { node = (Dictionary<int, object>)node[n]; }
            
            else { cool = false; }
        });

        if (cool) {Hwdtech.IoC.Resolve<ICommand>("Commands.Collision", _obj1, _obj2).Execute(); }
    }
}
