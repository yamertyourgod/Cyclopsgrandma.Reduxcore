using System;

namespace Unidux
{
    public class TriggerBool<TAction, TState> where TAction : UniduxAction<TState> where TState : StateBase
    {
        private Enum _trigger;

        public static TAction CreateTrigger(Enum trigger)
        {
            var triggerBool = new TriggerBool<TAction, TState>();
            var action = Activator.CreateInstance<TAction>();           
            triggerBool._trigger = trigger;
            
            action.Invoke = triggerBool.OnInvoke;
            action.DoNext = triggerBool.DoNext;
            action.Type = $"Trigger bool {trigger}";
            return action;
        }

        private Action<TState, StorageObject> OnInvoke => (state, storage) =>
        {
            state.Triggers[_trigger] = true;
        };

        private Action<TState, StorageObject> DoNext => (state, storage) =>
        {
            state.Triggers[_trigger] = false;
        };
    }
}
