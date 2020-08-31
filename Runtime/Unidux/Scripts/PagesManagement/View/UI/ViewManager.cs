using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unidux;

namespace ViewManager
{
    public class ViewManager: SingletonMonoBehaviour<ViewManager>,  IViewManager
    {
        public List<IView> Views = new List<IView>();

        private ViewName _lastView;

        public void Initialize()
        {
            
        }

        public void RegisterView(IView view)
        {
            Views.Add(view);
            view.SetActive(false);
        }

        public static ViewName CurrentView;

        public void SwitchTo(ViewName name, object options = null)
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

        public void ShowPopup(ViewName name, object options = null)
        {
            GetView(name)?.OnShow(options);
        }

        public IView GetView(ViewName name)
        {
            return Views.FirstOrDefault(v => v.ViewName.Equals(name));
        }

        public ViewName GetCurrentViewName()
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
