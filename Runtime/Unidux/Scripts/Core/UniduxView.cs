using System;
using UnityEngine;
using UniRx;

namespace Unidux
{
    public abstract class UniduxView<TState, TControls, TConfiguration> : MonoBehaviour where TState : StateBase where TControls : IViewControls
    {
        public TControls Controls;
        public TConfiguration Configuration;

        public abstract void OnStateChanged(TState state);


        protected bool renderIfDisabled = false;

        protected virtual void Awake()
        {
            Subscribe();
            OnAwake();
        }

        protected virtual void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnStart() { }

        protected virtual void Subscribe()
        {
            StoreBase<TState>.Subscribe(this, (state) => OnRenderRequest(state));
        }

        private void OnRenderRequest(TState state)
        { 
            if (renderIfDisabled || gameObject.activeInHierarchy)
            {
                OnStateChanged(state);
            }
        }

    }
}
