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

        public bool Active { get; set; }

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
            gameObject.SetActive(true);
        }

        public virtual void OnHide(object options = null)
        {
            gameObject.SetActive(false);
        }

        public void SetActive(bool active, object options = null)
        {
            if (active)
            {
                OnShow(options);
            }
            else
            {
                OnHide(options);
            }
        }

        public void OnShow(ShowOptions options = null)
        {
            throw new NotImplementedException();
        }

        public void OnHide(HideOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}