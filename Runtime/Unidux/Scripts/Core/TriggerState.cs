using System;

namespace Unidux
{
    public class TriggerState<TAction, TState> where TAction : UniduxAction<TState> where TState : StateBase
    {
        private Enum _trigger;
        private Enum _state;

        public static TAction CreateTrigger(Enum trigger, Enum state)
        {
            var triggerState = new TriggerState<TAction, TState>();
            var action = Activator.CreateInstance<TAction>();

            triggerState._trigger = trigger;
            triggerState._state = state;
            action.Invoke = triggerState.OnInvoke;
            action.DoNext = triggerState.DoNext;
            action.Type = $"Trigger state {trigger} {state}";
            return action;
        }

        private Action<TState, StorageObject> OnInvoke => (state, storage) =>
        {
            state.StateTriggers[_trigger] = _state;
        };

        private Action<TState, StorageObject> DoNext => (state, storage) =>
        {
            state.StateTriggers[_trigger] = DefaultState.None;
        };

        public enum DefaultState
        {
            None
        }
    }
}
