using UnityEngine;

namespace Assets.Code.Helpers
{
    public static class EnvironmentVariables
    {
        private static string _storagePath = Application.persistentDataPath;

        public static string StoragePath => _storagePath;
    }
}
