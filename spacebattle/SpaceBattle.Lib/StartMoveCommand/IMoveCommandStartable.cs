public interface IMoveCommandStartable
{
    IUObject obj { get; }
    Vector velocity { get; }
    Queue<ICommand> queue { get; }
}
