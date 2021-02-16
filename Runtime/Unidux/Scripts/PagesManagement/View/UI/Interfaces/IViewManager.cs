using System;
using System.Collections.Generic;

namespace ViewManager
{ 
    public interface IViewManager: System.IDisposable
    {
        void RegisterView(IView view);
        void SwitchTo(Enum viewName, SetActiveOptions options = null);
        void ShowPopup(Enum viewName, SetActiveOptions options = null);
        void HidePopup(Enum viewName, SetActiveOptions options = null);
        void ShowPanel(Enum viewName, SetActiveOptions options = null);
        void HidePanel(Enum viewName, SetActiveOptions options = null);
        List<Enum> GetOpenedPanels { get; }
        List<Enum> GetOpenedPopups { get; }
        IView GetView(Enum name);
        Enum GetCurrentViewName();
    }
}