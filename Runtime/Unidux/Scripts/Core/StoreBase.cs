using System;
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

        private Func<object, object> _dispatcher;
        private SynchronizationContext _synchronizationContext;

        protected abstract IReducer[] reducers { get; set; }
        protected virtual RepositoryBase repository { get; set; }

        public void Initialize()
        {
            _instance = this;
            _synchronizationContext = SynchronizationContext.Current;
            _state = Activator.CreateInstance<TState>();
            _changed = false;
            UniduxTickProvider.Subscribe(this); 
        }

        public static void Dispatch(UniduxAction<TState> action)
        {
            BeginDispatch(action);
        }


        public async static void Subscribe(Component component, Action<TState> action)
        {
            await UntilInstanceNotNull();

            if (_instance._subject == null) _instance._subject = new Subject<TState>();
            _instance._subject.AsObservable().TakeUntilDestroy(component).Subscribe(action).AddTo(component);
        }

        public async static void Subscribe(Action<TState> action)
        {
            await UntilInstanceNotNull();

            if (_instance._subject == null) _instance._subject = new Subject<TState>();
            _instance._subject.AsObservable().Subscribe(action);
        }

        /// <summary>
        /// Returns a clone of state
        /// </summary>
        /// <returns></returns>
        public static TState GetState()
        {
            return _instance._state;
        }

        public static T GetRepository<T>() where T : RepositoryBase
        {
            if (_instance.repository != null)
            {
                return _instance.repository is T ? _instance.repository as T : throw new Exception("Wrong repository cast type");
            }
            else
            {
                throw new Exception("Repo hasn't been initialized");
            }
        }

        public static async Task<T> GetReposytoryAsync<T>() where T : RepositoryBase
        {
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
                fixedState = (TState)this._state.Clone();

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

            //Debug.Log("Tick");
            this.ForceUpdate();
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