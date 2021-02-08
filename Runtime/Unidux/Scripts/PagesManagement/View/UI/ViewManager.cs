using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unidux;
using System;

namespace ViewManager
{
    public class ViewManager: SingletonMonoBehaviour<ViewManager>,  IViewManager
    {
        public List<IView> Views = new List<IView>();

        private Enum _lastView;

        public void Initialize()
        {
            
        }

        public void RegisterView(IView view)
        {
            Views.Add(view);
            if (view.ViewType == ViewType.Window)
            {
                view.SetActive(false);
            }
        }

        public static Enum CurrentView;

        public void SwitchTo(Enum name, object options = null)
        {
            CurrentView = name;
            //Debug.Log($"Switch to {CurrentView}");
            if (_lastView != name)
            {
                Views.Where(v => v.ViewType == ViewType.Window).ToList().ForEach(v => v.SetActive(v.ViewName.Equals(name)));
                if (Views.Exists(v => v.ViewName.Equals(name)))
                {
                    _lastView = name;
                }
            }

        }

        public void ShowPopup(Enum name, object options = null)
        {
            var view = GetView(name);
            view.SetActive(true, options);
        }

        public void HidePopup(Enum name, object options = null)
        {
            var view = GetView(name);
            view.SetActive(false, options);
        }

        public void ShowPanel(Enum name, object options = null)
        {
            var view = GetView(name);
            view.SetActive(true, options);
        }

        public void HidePanel(Enum name, object options = null)
        {
            var view = GetView(name);
            view.SetActive(false, options);
        }

        public IView GetView(Enum name)
        {
            return Views.FirstOrDefault(v => v.ViewName.Equals(name));
        }

        public Enum GetCurrentViewName()
        {
            return CurrentView;
        }

        public void Update()
        {
        }

        public void Dispose()
        {

        }

       
    }


}
