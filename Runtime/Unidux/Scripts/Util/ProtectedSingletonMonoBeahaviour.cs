using UnityEngine;

namespace Unidux
{
    public class ProtectedSingletonMonoBehaviour<TClass> : MonoBehaviour where TClass : ProtectedSingletonMonoBehaviour<TClass>
    {
        private static TClass _instance;

        protected static TClass instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (TClass)FindObjectOfType(typeof(TClass));

                    if (_instance == null)
                    {
                        Debug.LogWarning(typeof(TClass) + "is nothing");
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (_instance == null)
            {
                _instance = (TClass)this;
                return true;
            }
            else if (_instance == this)
            {
                return true;
            }

            Destroy(this);
            return false;
        }
    }
}

