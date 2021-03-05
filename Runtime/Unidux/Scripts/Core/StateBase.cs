using System;
using System.Collections.Generic;
using Unidux.Util;

namespace Unidux
{
    [Serializable]
    public abstract class StateBase : IState, IStateChanged, ICloneable
    {
        public abstract string Id { get; set; }
        public bool IsStateChanged { get; private set; }

        public BoolTrigger Triggers { get; set; } = new BoolTrigger();
        public EnumTrigger StateTriggers { get; set; } = new EnumTrigger();

        public StateBase()
        {

        }

        public virtual void SetStateChanged(bool changed = true)
        {
            this.IsStateChanged = changed;
        }
        
        object ICloneable.Clone()
        {
            // It's slow. So in case of requiring performance, override this deep clone method by your code.  
            var clone = CloneUtil.MemoryClone(this) as StateBase;
            clone.Id = this.Id;
            return clone;
        }

        public T Clone<T>() where T: StateBase
        {
            return CloneUtil.MemoryClone(this) as T;
        }

        public override bool Equals(object obj)
        {
            // It's slow. So in case of requiring performance, override this equality method by your code.
            return EqualityUtil.EntityEquals(this, obj);
        }

        public override int GetHashCode()
        {
            // Default implmeentation for supress warnings
            return base.GetHashCode();
        }
    }
}