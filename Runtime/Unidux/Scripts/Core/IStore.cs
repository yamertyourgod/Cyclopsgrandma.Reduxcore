using System;

namespace Unidux
{
    public interface IStore<TState> where TState : StateBase
    {    
        object DispatchObject(object action);
        void Tick();
    }

    public interface IStoreObject
    {
        object ObjectState { get; set; }
        IObservable<object> ObjectSubject { get; }
        Type StateType { get; }
    }
}
