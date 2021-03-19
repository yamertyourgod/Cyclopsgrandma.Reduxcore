using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Unidux
{
    public class ReducerBase<TState, TAction> : IReducer where TAction: UniduxAction<TState> where TState: StateBase
    {
        private UndoApplier<TState> _undoApplier;
        private SynchronizationContext _context = SynchronizationContext.Current;
        private CoroutineHolder _coroutineHolder;
        public ReducerBase()
        {
            _undoApplier = new UndoApplier<TState>(30, AllowUndoCondition);
            if (CoroutineHolder.Instance == null)
            {
                _coroutineHolder = new GameObject("CoroutinesHolder").AddComponent<CoroutineHolder>();
            }
            else
            {
                _coroutineHolder = CoroutineHolder.Instance;
            }
        }

        protected virtual bool AllowUndoCondition(UniduxAction<TState> action) => true;


        public virtual TState Reduce(TState state, TAction action)
        {
            UniduxAction<TState>.CurrentState = state;

            if(action.IsUndoLastAction)
            {
                _undoApplier.Undo(state);
            }

            if (action.IsRedoLastAction)
            {
                _undoApplier.Redo(state);
            }

            action.Invoke?.Invoke(state, action.Storage);
            if (action.DoNext != null)
            {
                try
                {
                    _context.Post((d) => _coroutineHolder.StartCoroutine(ExecuteCoroutine(action.DoNext)), null);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            if (action.Undo != null)
            {
                action.ContextId = SetActionContextId(state);
                _undoApplier.Write(action);
            }
            return state;
        }

        protected virtual string SetActionContextId(TState state)
        {
            return "Default";
        }

        public virtual object ReduceAny(object state, object action)
        {
            return this.Reduce((TState) state, (TAction) action);
        }

        public virtual bool IsMatchedAction(object action)
        {
            return action is TAction;
        }

        private IEnumerator ExecuteCoroutine(Action<TState, StorageObject> action)
        {
            yield return new WaitWhile(StoreBase<TState>.HasObservers);
            yield return new WaitFor().Frames(1);
            var nextAction = Activator.CreateInstance<TAction>();
            nextAction.Invoke = action;
            StoreBase<TState>.Dispatch(nextAction);
        }

        public void Dispose()
        {
            _undoApplier = null;
            _context = null;
            _coroutineHolder = null;
        }

        public class WaitFor
        {
            public IEnumerator Frames(int frameCount)
            {
                if (frameCount <= 0)
                {
                    throw new ArgumentOutOfRangeException("frameCount", "Cannot wait for less that 1 frame");
                }

                while (frameCount > 0)
                {
                    frameCount--;
                    yield return null;
                }
            }
        }
    }
}