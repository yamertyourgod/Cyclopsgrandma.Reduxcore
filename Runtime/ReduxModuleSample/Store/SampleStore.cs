using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidux;

namespace SampleModule
{
    public class SampleStore : Unidux.Util.StoreWithRepo<SampleState, SampleRepository>
    {
        protected override IReducer[] reducers { get; set; } = { new SampleReducer() };

        public new static SampleState GetState()
        {
            var state = StoreBase<SampleState>.GetState();

            return state;
        }
    }
}
