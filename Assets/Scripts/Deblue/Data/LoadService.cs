using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Deblue.Data
{
    public class LoadService
    {
        private static Encoding DefaultEncoding => Encoding.GetEncoding(1251);
        
        public string LoadStreamingAsset(string fileName)
        {
            CheckIsEmpty(fileName);

            var filePath = Application.dataPath + "/StreamingAssets/" + fileName;

            CheckIsExist(filePath);

            return File.ReadAllText(filePath);
        }

        public T LoadResource<T>(string filePath) where T : Object
        {
            CheckIsEmpty(filePath);

            T result = Resources.Load<T>(filePath);
            if (result == null)
            {
                throw new System.Exception($"Loading resource {typeof(T).Name} by given path <<{filePath}>> failed.");
            }

            return result;
        }

        public void Save<T>(string fileName, T obj)
        {
            var filePath = GetPersistentFilePath(fileName, false);
            using (Stream stream = File.Create(filePath))
            {
                using (StreamWriter writer = new StreamWriter(stream, DefaultEncoding))
                {
                    writer.Write(JsonUtility.ToJson(obj));
                }
            }
        }

        public T LoadJSON<T>(string filePath, T defoultValue = default(T))
        {
            CheckIsEmpty(filePath);
            CheckIsExist(filePath);

            T result;
            using (Stream stream = File.OpenRead(filePath))
            {
                using (StreamReader reader = new StreamReader(stream, DefaultEncoding))
                {
                    result = JsonUtility.FromJson<T>(reader.ReadToEnd());
                }

                if (result == null)
                {
                    result = defoultValue;
                }
            }

            return result;
        }

        public T LoadPersistentJSON<T>(string fileName, T defoultValue = default(T))
        {
            var filePath = GetPersistentFilePath(fileName, true);
            return LoadJSON(filePath, defoultValue);
        }

        private void CheckIsEmpty(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new System.ArgumentNullException("File name can't be null.");
            }
        }

        private void CheckIsExist(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new System.ArgumentException($"The specified file ({filePath}) does not exists.");
            }
        }

        private string GetPersistentFilePath(string fileName, bool checkExisting)
        {
            CheckIsEmpty(fileName);
            var filePath = string.Format("{0}/{1}", Application.persistentDataPath, fileName);
            if (checkExisting)
            {
                CheckIsExist(filePath);
            }

            return filePath;
        }
    }
}