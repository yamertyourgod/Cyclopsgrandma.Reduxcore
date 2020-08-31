using System;
using System.Collections.Generic;
using System.Threading;

namespace Unidux
{
    public class UniduxTickProvider: SingletonMonoBehaviour<UniduxTickProvider>
    {
        private List<ITicker> _tikers = new List<ITicker>();
        public static void Subscribe(ITicker ticker)
        {
            if (Instance != null)
            {
                _instance._tikers.Add(ticker);
            }
            //else
            //{
            //    InternalTicker.Subscribe(tickAction);
            //}
        }

        private void Update()
        {
            foreach(var ticker in _tikers)
            {
                ticker.Tick();
            }
        }
    }

    public class InternalTicker
    {
        private static InternalTicker _instance;
        private event Action _doTick;
        private SynchronizationContext _context;

        private InternalTicker()
        {
            _context = SynchronizationContext.Current;
            StartTicker();
        }

        private void StartTicker()
        {
            new Thread(() =>
            {
                while (this != null)
                {
                    Thread.Sleep(1);
                    _context.Post(Tick, null);
                }
            }).Start();
        }

        private void Tick(object state)
        {
            if(_doTick != null)
            {
                _doTick();
            }
        }

        public static void Subscribe(Action tickAction)
        {
            if(_instance == null)
            {
                _instance = new InternalTicker();
            }
            _instance._doTick += tickAction;
        }
    }
}