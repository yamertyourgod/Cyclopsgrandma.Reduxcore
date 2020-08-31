using System;
using System.Collections.Generic;

namespace Unidux
{
    public class StorageObject: IDisposable
    {
        private Dictionary<string, object> _args = new Dictionary<string, object>();

        public void Set(string name, object obj)
        {
            if (_args.ContainsKey(name))
            {
                _args[name] = obj;
            }
            else
            {
                _args.Add(name, obj);
            }
        }
        public object Get(string name)
        {
            return _args[name];
        }

        public void Dispose()
        {
            _args = null;
        }

    }
}