﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unidux.Util;
using UniRx;
using UnityEngine;

namespace Unidux
{
    public abstract class StoreBase<TState> : IStore<TState>, IStoreObject, IInitialize, ITicker where TState : StateBase
    {
        private static StoreBase<TState> _instance;
        private TState _state;
        private bool _changed;
        private Subject<TState> _subject;
        private Dictionary<Action<TState>, IDisposable> _subscriptions;
        private Func<object, object> _dispatcher;
        private SynchronizationContext _synchronizationContext;

        protected abstract IReducer[] reducers { get; set; }
        protected virtual RepositoryBase repository { get; set; }
        protected virtual EntitiesBase entities { get; set; }
        protected virtual ServicesBase services { get; set; }

        public virtual bool DisposeOnLoadHub { get; set; }
        public static bool Initialized => _instance != null;

        public void Initialize()
        {
            _instance = this;
            _subscriptions = new Dictionary<Action<TState>, IDisposable>();
            _synchronizationContext = SynchronizationContext.Current;
            _state = Activator.CreateInstance<TState>();
            _changed = false;
        }

        public static void Dispatch(UniduxAction<TState> action)
        {
            if (_instance == null)
                throw new StoreNotInitializedException();

            BeginDispatch(action);
        }

        public static void Subscribe(Component component, Action<TState> action)
        {
            if(_instance is null)
            {
                SubscribeAsync(component, action);
                return;
            }

            EndSubscribe(component, action);
        }

        public static void Subscribe(Action<TState> action)
        {
            if (_instance is null)
            {
                SubscribeAsync(action);
                return;
            }

            EndSubscribe(action);
        }

        private static void EndSubscribe(Component component, Action<TState> action)
        {
            if (_instance._subject == null) _instance._subject = new Subject<TState>();
            if (_instance._subscriptions.ContainsKey(action))
                return;

            _instance._subscriptions.Add(action, _instance._subject.AsObservable().TakeUntilDestroy(component).Subscribe(action).AddTo(component));
        }

        internal static bool HasObservers()
        {
            return _instance?._subject?.HasObservers == true;
        }

        private static void EndSubscribe(Action<TState> action)
        {
            if (_instance._subject == null) _instance._subject = new Subject<TState>();
            if (_instance._subscriptions.ContainsKey(action))
                return;

            _instance._subscriptions.Add(action, _instance._subject.AsObservable().Subscribe(action));
        }

        private static async void SubscribeAsync(Component component, Action<TState> action)
        {
            await UntilInstanceNotNull();
            EndSubscribe(component, action);
        }

        private static async void SubscribeAsync(Action<TState> action)
        {
            await UntilInstanceNotNull();
            EndSubscribe(action);
        }



        public static void Unsubscribe(Action<TState> action)
        {
            if (_instance == null)
                return;

            if (_instance._subscriptions.TryGetValue(action, out var subscription))
            {
                subscription.Dispose();
                _instance._subscriptions.Remove(action);
            }
        }

        /// <summary>
        /// Returns a clone of state
        /// </summary>
        /// <returns></returns>
        public static TState GetState()
        {
            if (_instance == null)
                throw new StoreNotInitializedException();

            return _instance._state;
        }

        public static T GetRepository<T>() where T : RepositoryBase
        {
            if (_instance == null)
                throw new StoreNotInitializedException();


            if (_instance.repository != null)
            {
                return _instance.repository is T ? _instance.repository as T : throw new Exception("Wrong repository cast type");
            }
            else
            {
                throw new Exception("Repo hasn't been initialized");
            }
        }

        public static T GetServices<T>() where T: ServicesBase
        {
            if (_instance == null)
                throw new StoreNotInitializedException();


            if (_instance.services != null)
            {
                return _instance.services is T ? _instance.services as T : throw new Exception("Wrong services cast type");
            }
            else
            {
                throw new Exception("Services hasn't been initialized");
            }
        }

        public static T GetEntities<T>() where T : EntitiesBase
        {
            if (_instance == null)
                throw new StoreNotInitializedException();


            if (_instance.entities != null)
            {
                return _instance.entities is T ? _instance.entities as T : throw new Exception("Wrong entities cast type");
            }
            else
            {
                throw new Exception("Entities hasn't been initialized");
            }
        }


        public static async Task<T> GetReposytoryAsync<T>() where T : RepositoryBase
        {
            if (_instance == null)
                throw new StoreNotInitializedException();

            await UntilInstanceNotNull();
            return _instance.repository as T;
        }

        private static async void BeginDispatch(UniduxAction<TState> action)
        {
            await UntilInstanceNotNull();
            _instance._synchronizationContext.Post(EndDispatch, action);
        }

        private static void EndDispatch(object action)
        {
            _instance.DispatchObject(action);
        }

        private static async Task UntilInstanceNotNull()
        {
            while (_instance == null)
            {
                await Task.Delay(100);
            }
        }

        public object ObjectState
        {
            get { return this._state; }
            set { this._state = (TState)value; }
        }

        public IObservable<object> ObjectSubject
        {
            get { return this._subject.Select(it => (object)it); }
        }

        public Type StateType
        {
            get { return typeof(TState); }
        }

        public void ApplyMiddlewares(params Middleware[] middlewares)
        {
            this._dispatcher = (object _action) => { return this._Dispatch(_action); };

            foreach (var middleware in middlewares.Reverse())
            {
                this._dispatcher = middleware(this)(this._dispatcher);
            }
        }



        public object DispatchObject(object action)
        {
            if (this._dispatcher == null)
            {
                return this._Dispatch(action);
            }
            else
            {
                return this._dispatcher(action);
            }
        }


        private object _Dispatch(object action)
        {
            foreach (var matcher in this.reducers)
            {
                if (matcher.IsMatchedAction(action))
                {
                    this._state = (TState)matcher.ReduceAny(this._state, action);
                    this._state.LastAction = action;
                    this._changed = true;
                }
            }

            if (!this._changed)
            {
                Debug.LogWarning("'Store.Dispatch(" + action + ")' was failed. Maybe you forget to assign reducer.");
            }

            return null;
        }


        public void ForceUpdate()
        {
            this._changed = false;
            TState fixedState;

            lock (this._state)
            {
                // Prevent writing state object
                var action = this._state.LastAction;
                fixedState = (TState)this._state.Clone();
                fixedState.LastAction = action;
                // The function may slow
                StateUtil.ResetStateChanged(this._state);
            }

            if (this._subject == null) this._subject = new Subject<TState>();
            this._subject.OnNext(fixedState);
        }

        public virtual void Tick()
        {
            if (!this._changed)
            {
                return;
            }

            if (_subject != null && _subject.HasObservers)
            {
                this.ForceUpdate();
            }
        }

        public void InitOnLoadHub()
        {
            UniduxTickProvider.Subscribe(this);
        }

        public void Dispose()
        {
            Debug.LogWarning("Disposing...");
            UniduxTickProvider.Unsubscribe(this);
            _instance = null;
            _state = null;
            _subject = null;
            _dispatcher = null;
            _synchronizationContext = null;
            repository.Dispose();
            reducers.ToList().ForEach(r => r.Dispose());
            reducers = null;
        }
    }

    public static class StoreBuilder
    {
        public static void Build(this IStoreObject storeObject)
        {
            var store = storeObject as IInitialize;
            store.Initialize();
        }

        public static T Clone<T>(this T state) where T : StateBase
        {
            return state.Clone<T>();
        }
    }
}