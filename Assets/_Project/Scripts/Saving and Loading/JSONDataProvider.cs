using System.IO;
using UnityEngine;

namespace Persistence
{
    /// <summary>
    /// Implements saving and loading of data using JSON serialization to files in the persistent data path.
    /// </summary>
    /// <remarks>This class provides a simple file-based storage mechanism using JSON format. It saves each
    /// piece of data to a separate file named after the provided key, with a .json extension.</remarks>

    public class JSONDataProvider : IPersistentDataProvider
    {
        private string _path;

        public JSONDataProvider()
        {
            _path = $"{Application.persistentDataPath}";
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        public T Load<T>(string key)
        {
            string filePath = $"{_path}/{key}.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json);
            }

            // Return default if file does not exist
            return default;
        }

        public void Save<T>(string key, T data)
        {
            string filePath = $"{_path}/{key}.json";
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }

    }
}