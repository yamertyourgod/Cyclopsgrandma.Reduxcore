using System;

namespace ViewManager
{
    public interface IView
    {
        Enum ViewName { get; }
        ViewType ViewType { get; }

        bool Active { get; }

        void RegisterSelfOnAwake();
        void SetActive(bool active, object options = null);
        void OnShow(ShowOptions options = null);
        void OnHide(HideOptions options = null);
    }
}
