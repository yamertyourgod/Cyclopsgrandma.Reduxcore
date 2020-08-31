using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidux;
using UnityEngine;

namespace SampleModule.View
{
    [Serializable]
    public class SampleControls : IViewControls
    {
        private MonoBehaviour _view;

        public void Assign(MonoBehaviour view)
        {
            _view = view;
        }
    }
}
