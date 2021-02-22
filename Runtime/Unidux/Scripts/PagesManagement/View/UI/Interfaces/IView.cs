using System;

namespace ViewManager
{
    public interface IView
    {
        Enum ViewName { get; }
        ViewType ViewType { get; }

        bool Active { get; }

        void RegisterSelfOnAwake();
        void SetActive(bool active, SetActiveOptions options = null);
        void OnShow(SetActiveOptions options = null);
        void OnLateShow();
        void OnHide(SetActiveOptions options = null);
    }
}
