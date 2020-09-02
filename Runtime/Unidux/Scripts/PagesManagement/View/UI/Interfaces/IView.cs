using System;

namespace ViewManager
{
    public interface IView
    {
        Enum ViewName { get; }
        ViewType ViewType { get; }

        void RegisterSelfOnAwake();
        void SetActive(bool active);
        void OnShow(object options = null);
        void OnHide();
    }
}
