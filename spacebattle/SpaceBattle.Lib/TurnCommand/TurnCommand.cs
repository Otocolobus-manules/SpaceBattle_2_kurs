public class TurnCommand: ICommand
{
    private readonly ITurnable turnable;

    public TurnCommand(ITurnable turnable)
    {
        this.turnable = turnable;
    }

    public void Execute()
    {
        turnable.corner += turnable.delta
    }
}
