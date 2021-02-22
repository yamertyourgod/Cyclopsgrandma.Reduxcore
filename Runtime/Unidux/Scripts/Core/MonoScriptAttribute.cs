using UnityEngine;
using System;

namespace Unidux
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        public System.Type type;
    }
}
