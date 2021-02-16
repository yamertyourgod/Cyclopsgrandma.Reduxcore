
using System;
using System.Collections.Generic;
using Unidux;
using UnityEngine;

namespace SampleModule
{
    public class SampleAction : UniduxAction<SampleState>
    {
        public static SampleAction Hello => SampleHelloAction.Create();
        public static SampleAction UndoLast = new SampleAction() { IsUndoLastAction = true };
        public static SampleAction RedoLast = new SampleAction() { IsRedoLastAction = true };

        public static SampleAction SetTrigger(SampleState.TriggerBool @enum) => SetTrigger<SampleAction>(@enum);
        public static SampleAction SetTriggerState(SampleState.TriggerState @enum, Enum state) => SetTrigger<SampleAction>(@enum, state);
    }
}