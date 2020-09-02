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
            view.SetActive(false);
        }

        public static Enum CurrentView;

        public void SwitchTo(Enum name, object options = null)
        {
            CurrentView = name;
            //Debug.Log($"Switch to {CurrentView}");
            if (_lastView != name)
            {
                Views.ForEach(v => v.SetActive(v.ViewName.Equals(name)));
                if (Views.Exists(v => v.ViewName.Equals(name)))
                {
                    _lastView = name;
                }
            }

        }

        public void ShowPopup(Enum name, object options = null)
        {
            GetView(name)?.OnShow(options);
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
