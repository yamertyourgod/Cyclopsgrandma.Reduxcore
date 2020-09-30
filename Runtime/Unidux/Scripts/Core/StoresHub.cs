using System;
using System.Collections.Generic;
using System.IO;
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
        private void Awake()
        {
            IncludedStores.ForEach(store => ValidateAndAdd(store));
        }

        private void ValidateAndAdd(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type) as IStoreObject;
                instance.Build();
            }
            else
            {
                Debug.LogWarning($"Some store has not been assigned!");
            }
        }
    }
}
