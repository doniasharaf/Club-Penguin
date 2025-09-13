using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private ISaveLoadProvider _saveLoadProvider;

    private void Awake()
    {
        _saveLoadProvider = new JSONSaveLoadProvider();
    }
    public void SaveData<T>(string key, T data)
    {
        if (_saveLoadProvider == null)
        {
            Debug.LogError("SaveLoadProvider is not initialized. Cannot save data.");
            return;
        }

        _saveLoadProvider.Save<T>(key, data);
    }
    public T LoadData<T>(string key)
    {
        if (_saveLoadProvider == null)
        {
            Debug.LogError("SaveLoadProvider is not initialized. Cannot load data.");
            return default;
        }

        return _saveLoadProvider.Load<T>(key);
    }
}
