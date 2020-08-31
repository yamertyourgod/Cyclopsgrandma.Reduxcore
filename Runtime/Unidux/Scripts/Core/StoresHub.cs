using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unidux;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.ThirdParty.Unidux.Scripts.Core
{
    public class StoresHub: MonoBehaviour
    {
        [SerializeField]
        private List<MonoScript> IncludedStores = new List<MonoScript>();
        private void Awake()
        {
            IncludedStores.ForEach(store => ValidateAndAdd(store.GetClass()));
        }

        private void ValidateAndAdd(Type type)
        {
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
