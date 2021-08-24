using System;

namespace Unidux
{
    [Serializable]
    public class EnumTrigger: TriggersWrapper<Enum>
    {
        protected override Enum GetDefault<TValue>(Enum trigger)
        {
            return DefaultEnum.None;
        }

        public enum DefaultEnum
        {
            None
        }
    }
}