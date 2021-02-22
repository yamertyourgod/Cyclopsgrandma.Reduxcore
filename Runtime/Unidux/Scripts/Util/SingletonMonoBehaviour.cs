using UnityEngine;

namespace Unidux
{
    public class SingletonMonoBehaviour<TClass> : MonoBehaviour where TClass : SingletonMonoBehaviour<TClass>
    {
        protected static TClass _instance;

        public static TClass Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (TClass) FindObjectOfType(typeof(TClass));
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
                _instance = (TClass) this;
                return true;
            }
            else if (_instance == this)
            {
                return true;
            }

            Destroy(gameObject);
            return false;
        }
    }
}
