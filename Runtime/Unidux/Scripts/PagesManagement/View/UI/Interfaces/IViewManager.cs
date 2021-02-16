using System;
using System.Collections.Generic;

namespace ViewManager
{ 
    public interface IViewManager: System.IDisposable
    {
        void RegisterView(IView view);
        void SwitchTo(Enum viewName, ShowOptions options = null);
        void ShowPopup(Enum viewName, ShowOptions options = null);
        void HidePopup(Enum viewName, HideOptions options = null);
        void ShowPanel(Enum viewName, ShowOptions options = null);
        void HidePanel(Enum viewName, HideOptions options = null);
        List<Enum> GetOpenedPanels { get; }
        List<Enum> GetOpenedPopups { get; }
        IView GetView(Enum name);
        Enum GetCurrentViewName();
    }
}