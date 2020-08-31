using Newtonsoft.Json;
using System.Collections.Generic;
using Unidux;

namespace SampleModule
{
    [System.Serializable]
    public class SampleState : StateBase
    {
        public override string Id { get; set; } = "SampleModule";
        public string HelloString { get; internal set; }
    }
}
