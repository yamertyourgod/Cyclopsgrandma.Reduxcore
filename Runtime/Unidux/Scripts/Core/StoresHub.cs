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

        private static List<string> _createdTypes = new List<string>();

        private void Awake()
        {
            IncludedStores.ForEach(store => ValidateAndAdd(store));
        }

        private void ValidateAndAdd(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
            {
                var exist = _createdTypes.Contains(typeName);
                if (!exist)
                {
                    var instance = Activator.CreateInstance(type) as IStoreObject;
                    instance.Build();
                    _createdTypes.Add(typeName);
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
    }
}
