using MVVMGenericsLibrary.Interfaces;

namespace GenericsLibrary
{
    //For Factories
    public interface IFactory<T, TKey>
    {
        T Convert(IViewData<TKey> data);
    }
}