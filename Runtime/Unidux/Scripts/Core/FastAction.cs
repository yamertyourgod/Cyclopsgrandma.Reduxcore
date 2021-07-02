using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidux;

namespace Unidux
{
    public class FastAction<TState> : UniduxAction<TState> where TState: StateBase
    {
        public FastAction()
        {
            SetupAction(this, this.OnInvokeAbstract, this.OnNextAbstract, this.OnUndoAbstract, this.OnRedoAbstarct);
        }

        private void OnRedoAbstarct(TState state, StorageObject storage)
        {
            OnRedo(state, storage);
        }

        protected virtual void OnRedo(TState state, StorageObject storage)
        {

        }

        private void OnUndoAbstract(TState state, StorageObject storage)
        {
            OnUndo(state, storage);
        }

        protected virtual void OnUndo(TState state, StorageObject storage)
        {

        }

        private void OnNextAbstract(TState state, StorageObject storage)
        {
            OnNext(state, storage);
        }

        protected virtual void OnNext(TState state, StorageObject storage)
        {
            state.NotifyObservers = false;
        }

        private void OnInvokeAbstract(TState state, StorageObject storage)
        {
            state.NotifyObservers = true;
            OnInvoke(state, storage);
        }

        protected virtual void OnInvoke(TState state, StorageObject storage)
        {

        }
    }
}
