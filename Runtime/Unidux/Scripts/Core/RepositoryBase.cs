using Assets.Code.Helpers;
using Assets.Source;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Unidux
{
    public class RepositoryBase : IDisposable
    {
        public T LoadConfig<T>(string configName)
        {
            var directory = $"{EnvironmentVariables.StoragePath}/Configs/";
            string path = $"{directory}{configName}";
            if (File.Exists(path))
            {
                string fileText = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(fileText, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            else
            {
                var empty = Activator.CreateInstance<T>();
                Directory.CreateDirectory(directory);
                File.WriteAllText(path, JsonConvert.SerializeObject(empty));
                return empty;
            }
        }

        public void SaveConfig<T>(string configName, T model)
        {
            string path = $"{EnvironmentVariables.StoragePath}/Configs/{configName}";
            string fileText = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            if (File.Exists(path))
            {
                File.WriteAllText(path, fileText);
            }
            else
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(path, fileText);
            }
        }

        public Sprite CreateSpriteFromTexture(Texture2D tex)
        {
            var w = tex.width;
            var h = tex.height;

            var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, w, h), new Vector2(0.5f, 0.5f), 100.0f);
            tex = null;
            return sprite;
        }

        public virtual void Dispose()
        {

        }
    }
}
