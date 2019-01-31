namespace GenericsLibrary
{
    //For Factories
    public interface IFactory<TData, T>
    {
        T Convert(TData data);
    }
}