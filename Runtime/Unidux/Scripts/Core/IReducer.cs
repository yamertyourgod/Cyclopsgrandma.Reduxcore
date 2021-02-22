using System;
using System.Threading.Tasks;

namespace Unidux
{
    public interface IReducer: IDisposable
    {
        bool IsMatchedAction(object action);
        object ReduceAny(object state, object action);
    }
}