namespace Core
{
    public interface IFactory<T>
    {
        T Create();
    }

    public interface IFactory<T, S>
    {
        T Create(S value);
    }

    public interface IFactory<T, S, K>
    {
        T Create(S value1, K value2);
    }

    public interface IFactory<T, S, K, V>
    {
        T Create(S value1, K value2, V value3);
    }  
}
