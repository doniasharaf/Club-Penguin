
namespace Persistence
{
    /// <summary>
    /// Defines methods for saving and loading data associated with a specific key.
    /// </summary>
    /// <remarks>This interface provides a generic mechanism for persisting and retrieving data. 
    /// Implementations may define the underlying storage mechanism, such as file-based,  database, or in-memory
    /// storage.</remarks>
    public interface IPersistentDataProvider
    {

        public T Load<T>(string key);

        public void Save<T>(string key, T data);
    }


}