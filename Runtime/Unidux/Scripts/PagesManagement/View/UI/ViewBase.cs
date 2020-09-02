using System;
using System.Collections;
using Unidux;
using UnityEngine;

namespace ViewManager
{
    public abstract class ViewBase<TState, TControls, TConfiguration> : UniduxView<TState, TControls, TConfiguration>, IView where TState : StateBase where TControls : IViewControls
    {
        public abstract Enum ViewName { get; }
        public abstract ViewType ViewType { get; }

        protected IViewManager viewManager;

        protected override void Awake()
        {
            base.Awake();
            RegisterSelfOnAwake();
            Controls.Assign(this);
        }


        public void RegisterSelfOnAwake()
        {
            viewManager = ViewManager.Instance;
            viewManager.RegisterView(this);
        }

        public virtual void OnShow(object options = null)
        {
            Debug.Log("OnShow");
            gameObject.SetActive(true);
        }

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                OnShow();
            }
            else
            {
                OnHide();
            }
        } 
    }
}