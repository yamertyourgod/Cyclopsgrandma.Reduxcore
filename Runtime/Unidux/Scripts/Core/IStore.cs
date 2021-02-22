using System;

namespace Unidux
{
    public interface IStore<TState> where TState : StateBase
    {    
        object DispatchObject(object action);
        void Tick();
    }

    public interface IStoreObject: IDisposable
    {
        object ObjectState { get; set; }
        IObservable<object> ObjectSubject { get; }
        Type StateType { get; }

        bool DisposeOnLoadHub { get; }

        void InitOnLoadHub();
    }
}
