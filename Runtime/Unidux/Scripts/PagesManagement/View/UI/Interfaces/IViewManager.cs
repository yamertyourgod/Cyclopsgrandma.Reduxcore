namespace ViewManager
{ 
    public interface IViewManager: System.IDisposable
    {
        void RegisterView(IView view);
        void SwitchTo(ViewName viewName, object options = null);
        void ShowPopup(ViewName viewName, object options = null);
        IView GetView(ViewName name);
        ViewName GetCurrentViewName();
    }
}