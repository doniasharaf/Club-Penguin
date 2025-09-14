using UnityEngine;
namespace Persistence
{
    /// <summary>
    /// Central manager for saving and loading persistent game data.
    /// Delegates operations to an <see cref="IPersistentDataProvider"/>.
    /// Defaults to <see cref="JSONDataProvider"/> for the purpose of this prototype unless overridden later.
    /// </summary>
    public class PersistenceManager : MonoBehaviour
    {
        private IPersistentDataProvider _saveLoadProvider;

        private void Awake()
        {
            _saveLoadProvider = new JSONDataProvider();
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
}