using System;
using System.Collections.Generic;

namespace Unidux
{
    [Serializable]
    public class TriggersWrapper<TValue>
    {
        private Dictionary<Enum, TValue> _triggers = new Dictionary<Enum, TValue>(); 

        public TValue this[Enum trigger]
        {
            get => Get(trigger);
            set => Set(trigger, value);
        }

        private void Set(Enum trigger, TValue value)
        {
            _triggers[trigger] = value;
        }

        private TValue Get(Enum trigger)
        {
            if(_triggers.TryGetValue(trigger, out var value))
            {
                return value;
            }

            _triggers[trigger] = default;
            return default;
        }
    }
}