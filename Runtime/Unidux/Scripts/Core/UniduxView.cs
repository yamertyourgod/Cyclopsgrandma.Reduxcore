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

        [SerializeField]
        protected bool reactIfDisabled = false;

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
            StoreBase<TState>.Subscribe(this, OnRenderRequest);
        }

        protected virtual void Unsubscribe()
        {
            StoreBase<TState>.Unsubscribe(OnRenderRequest);

        }

        private void OnRenderRequest(TState state)
        { 
            if (reactIfDisabled || gameObject.activeInHierarchy)
            {
                OnStateChanged(state);
            }
        }

    }
}
