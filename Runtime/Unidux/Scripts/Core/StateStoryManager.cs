using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidux
{
    public class StateStoryManager
    {
        private Dictionary<string, StateStory> _stories;

        public StateStoryManager()
        {
            _stories = new Dictionary<string, StateStory>();
        }

        public void RegisterState(StateBase state, int storyItemsAmount)
        {
            if (_stories.ContainsKey(state.Id)) return;

            _stories.Add(state.Id, new StateStory(state, storyItemsAmount));
        }

        public void OnChange(StateBase state)
        {
            if (_stories.TryGetValue(state.Id, out var stateStory))
            {
                stateStory.Add(state);
            }
        }

        public void Clear(StateBase state)
        {
            if (_stories.TryGetValue(state.Id, out var stateStory))
            {
                stateStory.Clear();
                stateStory.Add(state);
            }
        }

        public StateBase GetPrevious(StateBase state)
        {
            if (_stories.TryGetValue(state.Id, out var stateStory))
            {
                return stateStory.Get(state);
            }
            else return null;
        }
    }

    public class StateStory
    {
        private int _storyItemAmount;
        private Stack<StateBase> _states;
        private bool _addedRecently;
        public StateStory(StateBase state, int storyItemAmount)
        {
            _storyItemAmount = storyItemAmount;
            _states = new Stack<StateBase>(storyItemAmount);
            //_states.Push(state);
        }

        public void Add(StateBase state)
        {
            if (_states.Count == _storyItemAmount)
            {
                var first = _states.First();
                if (first != null)
                    (_states as ICollection<StateBase>)?.Remove(first);
            }

            _states.Push(state.Clone());
            _addedRecently = true;
        }

        public void Clear()
        {
            _states.Clear();
            _addedRecently = false;
        }

        public StateBase Get(StateBase state)
        {
            lock (_states)
            {                
                if (_states.Count == 2)
                {
                    var result = _states.Peek();
                    var pop = _states.Pop();
                    _states.Push(pop);
                    return result;
                }
                if (_addedRecently && _states.Count > 3) _states.Pop();
                else if(_addedRecently && _states.Count == 3)
                {
                    _states.Pop();
                    return _states.Last();
                }
                
                _addedRecently = false;
                //UnityEngine.Debug.LogError($"WriteState; count = {_states.Count - 1}");
                return _states.Count > 0 ? _states.Pop() : null;
            }
        }
    }
}
