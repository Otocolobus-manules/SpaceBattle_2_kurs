public interface IMoveCommandEndable
{
    IUObject obj { get; }
    ICommand command { get; }
    Queue<ICommand> queue { get; }
}
