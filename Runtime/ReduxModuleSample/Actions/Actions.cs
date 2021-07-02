
using System;
using System.Collections.Generic;
using Unidux;
using UnityEngine;

namespace SampleModule
{
    public class SampleAction : FastAction<SampleState>
    {
        public static SampleAction Hello => SampleHelloAction.Create();
        public static SampleAction UndoLast = new SampleAction() { IsUndoLastAction = true };
        public static SampleAction RedoLast = new SampleAction() { IsRedoLastAction = true };

        public static SampleAction SetTriggerBool(SampleState.TriggerBool enumTrigger) => SetTrigger<SampleAction>(enumTrigger);
        public static SampleAction SetTriggerEnum(SampleState.TriggerEnum enumTrigger, Enum enumState) => SetTrigger<SampleAction>(enumTrigger, enumState);
    }
}