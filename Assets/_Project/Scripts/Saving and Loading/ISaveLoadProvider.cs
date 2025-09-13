
public interface ISaveLoadProvider
{

    public T Load<T>(string key);

    public void Save<T>(string key, T data);
}


