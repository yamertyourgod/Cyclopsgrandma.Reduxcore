using System;

namespace ViewManager
{ 
    public interface IViewManager: System.IDisposable
    {
        void RegisterView(IView view);
        void SwitchTo(Enum viewName, object options = null);
        void ShowPopup(Enum viewName, object options = null);
        IView GetView(Enum name);
        Enum GetCurrentViewName();
    }
}