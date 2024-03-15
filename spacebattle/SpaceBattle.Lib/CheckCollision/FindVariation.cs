public class InitFindVariationsCommand : ICommand
{
    public void Execute()
    {
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.DfVec", (object[] args) =>
        {
            var positionFirst = (List<int>)args[0];
            var velocityFirst = (List<int>)args[1];
            var positionSecond = (List<int>)args[2];
            var velocitySecond = (List<int>)args[3];

            var variation = positionFirst.Select((value, index) => value - positionSecond[index])
                .Concat(velocityFirst.Select((value, index) => value - velocitySecond[index])).ToList();
            return variation;
        }).Execute();
    }
}
