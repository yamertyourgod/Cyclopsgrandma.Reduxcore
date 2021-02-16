using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unidux;
using UnityEngine;

namespace Unidux
{
    public class StoresHub: MonoBehaviour
    {
        [SerializeField]
        [MonoScript]
        private List<string> IncludedStores = new List<string>();

        private static Dictionary<string, IStoreObject> _createdTypes = new Dictionary<string, IStoreObject>();

        private void Awake()
        {
            DisposeStoresNeedDisposed();
            IncludedStores.ForEach(store => ValidateAndAdd(store));
        }

        private void Start()
        {
            InitStoresOnLoad();
        }

        private void ValidateAndAdd(string typeName)
        {        
            var type = Type.GetType(typeName);
            if (type != null)
            {
                var exist = _createdTypes.ContainsKey(typeName);
                if (!exist)
                {
                    var instance = Activator.CreateInstance(type) as IStoreObject;
                    instance.Build();
                    _createdTypes.Add(typeName, instance);
                }
                else
                {
                    Debug.LogWarning("Store already exists");
                }
            }
            else
            {
                Debug.LogWarning($"Some store has not been assigned!");
            }
        }

        private void DisposeStoresNeedDisposed()
        {
            var removeList = new List<string>();
            foreach(var storePair in _createdTypes)
            {
                var storeObject = storePair.Value;
                if(storeObject.DisposeOnLoadHub)
                {
                    storeObject.Dispose();
                    removeList.Add(storePair.Key);
                }
            }

            removeList.ForEach(r => _createdTypes.Remove(r));
            removeList.Clear();
        }

        private void InitStoresOnLoad()
        {
            _createdTypes.Values.ToList().ForEach(s => s.InitOnLoadHub());
        }
    }
}
