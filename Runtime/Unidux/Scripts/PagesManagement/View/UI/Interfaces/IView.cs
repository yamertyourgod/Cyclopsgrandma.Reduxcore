namespace ViewManager
{
    public interface IView
    {
        ViewName ViewName { get; }
        ViewType ViewType { get; }

        void RegisterSelfOnAwake();
        void SetActive(bool active);
        void OnShow(object options = null);
        void OnHide();
    }
}
