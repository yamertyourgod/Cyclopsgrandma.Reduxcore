using System;
using Unidux;
using System.Linq;
using UnityEngine;
using ViewManager;
using UnityEngine.UI;
using System.Collections;


namespace SampleModule.View
{
    public class SampleView : ViewBase<SampleState, SampleControls, SampleConfiguration>
    {
        public override Enum ViewName => DefaultViewName.Default;

        public override ViewType ViewType => ViewType.Window;

        public override void OnStateChanged(SampleState state)
        {
            Debug.Log(state.HelloString);
        }
    }
}
