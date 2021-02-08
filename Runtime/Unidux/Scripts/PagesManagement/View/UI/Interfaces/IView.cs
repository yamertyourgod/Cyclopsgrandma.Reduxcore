using System;

namespace ViewManager
{
    public interface IView
    {
        Enum ViewName { get; }
        ViewType ViewType { get; }

        void RegisterSelfOnAwake();
        void SetActive(bool active, object options = null);
        void OnShow(object options = null);
        void OnHide(object options = null);
    }
}
