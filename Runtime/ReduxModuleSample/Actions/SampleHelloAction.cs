using SampleModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidux;

namespace SampleModule
{
    class SampleHelloAction: SampleAction
    {
        public static SampleHelloAction Create()
        {
            var action = new SampleHelloAction();
            return SetupAction(action, action.OnInvoke, action.OnNext, action.OnUndo, action.OnRedo);
        }

        private void OnRedo(SampleState state, StorageObject storage)
        {
            var lastValue = state.HelloString;
            state.HelloString = (string)storage.Get("hello");
            storage.Set("hello", lastValue);
        }

        private void OnUndo(SampleState state, StorageObject storage)
        {
            var lastValue = state.HelloString;
            state.HelloString = (string)storage.Get("hello");
            storage.Set("hello", lastValue);

        }

        private void OnInvoke(SampleState state, StorageObject storage)
        {
            state.HelloString = "Hello World!";
            storage.Set("hello", state.HelloString);
        }

        private void OnNext(SampleState state, StorageObject storage)
        {
            state.HelloString = "Hello Universe!";
            storage.Set("hello", state.HelloString);
        }
    }
}
