using UnityEngine;

namespace Unidux
{
    internal class CoroutineHolder: SingletonMonoBehaviour<CoroutineHolder>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}