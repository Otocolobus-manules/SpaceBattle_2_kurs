public interface ISource
{
    Dictionary<object, object?> nexts { get; set; }
    object? step_forward(object args);
}
