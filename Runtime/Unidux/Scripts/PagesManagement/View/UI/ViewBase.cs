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
        private Coroutine _lateShowCoroutine;

        protected virtual int FramesSkipToLateShow { get; set; } = 1;

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

        public virtual void OnShow(SetActiveOptions options = null)  { }

        public virtual void OnLateShow() { }

        public virtual void OnHide(SetActiveOptions options = null) { }

        public void SetActive(bool active, SetActiveOptions options = null)
        {
            if (active)
            {
                gameObject.SetActive(true);
                OnShow(options);
                _lateShowCoroutine = StartCoroutine(CallOnShowLate());
            }
            else
            {
                gameObject.SetActive(false);
                OnHide(options);
                if(_lateShowCoroutine != null) StopCoroutine(_lateShowCoroutine);
            }
        }

        private IEnumerator CallOnShowLate()
        {
            //this skips frames
            for(var i = 0; i < FramesSkipToLateShow; i++)
            {
                yield return null;
            }

            OnLateShow();
        }
    }
}