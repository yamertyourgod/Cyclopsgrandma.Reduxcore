using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unidux
{
    public class UniduxAction<TState> where TState: StateBase
    {
        public IList<object> Payload = new List<object>();
        public Action<TState, StorageObject> Invoke;
        public Action<TState, StorageObject> Undo;
        public Action<TState, StorageObject> Redo;
        public StorageObject Storage = new StorageObject();
        public string ContextId;
        public Action OnComplete;
        public Action<TState, StorageObject> DoNext;
        public bool IsUndoLastAction;
        public bool IsRedoLastAction;

        public static TState CurrentState;

        protected static TAction CreateAction<TAction>(System.Action<TState, StorageObject> onInvoke, System.Action<TState, StorageObject> doNext = null, Action<TState, StorageObject> undo = null, Action<TState, StorageObject> redo = null) where TAction : UniduxAction<TState>
        {
            var action = Activator.CreateInstance<TAction>();
            SetupAction(action, onInvoke, doNext, undo, redo);
            return action;
        }

        protected static TAction SetupAction<TAction>(TAction action, System.Action<TState, StorageObject> onInvoke, System.Action<TState, StorageObject> doNext = null, Action<TState, StorageObject> undo = null, Action<TState, StorageObject> redo = null) where TAction : UniduxAction<TState>
        {
            action.Invoke = onInvoke;
            action.Undo = undo;
            action.Redo = redo;
            if (doNext != null)
            {
                action.DoNext = doNext;
            }
            return action;
        }

        protected static TAction SetTrigger<TAction>(Enum trigger) where TAction: UniduxAction<TState>
        {
            return TriggerBool<TAction, TState>.CreateTrigger(trigger);
        }

        protected static TAction SetTrigger<TAction>(Enum trigger, Enum state) where TAction : UniduxAction<TState>
        {
            return TriggerState<TAction, TState>.CreateTrigger(trigger, state);
        }

    }
}
