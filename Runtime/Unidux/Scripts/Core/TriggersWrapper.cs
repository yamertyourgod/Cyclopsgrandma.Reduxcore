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

            return _triggers[trigger] = GetDefault<TValue>(trigger);

            throw new Exception("Attempt to use empty enum for enum triggers!");
        }

        protected virtual TValue GetDefault<TTValue>(Enum trigger) where TTValue : TValue
        {
            return default;
        }
    }


}