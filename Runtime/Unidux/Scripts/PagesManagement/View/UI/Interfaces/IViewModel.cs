namespace ViewManager
{
    public interface IView<T> : IView
    {
        T Model { get; set; }
    }
}
