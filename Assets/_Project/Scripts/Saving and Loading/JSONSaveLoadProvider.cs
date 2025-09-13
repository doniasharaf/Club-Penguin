using System.IO;
using UnityEngine;


public class JSONSaveLoadProvider : ISaveLoadProvider
{
    private string _path;

    public JSONSaveLoadProvider()
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