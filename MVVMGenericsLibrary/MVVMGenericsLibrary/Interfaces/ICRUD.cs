using MVVMGenericsLibrary.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    //For Catelogs
    public interface ICRUD<T, TKey>
    {
        Task<TKey> Create(IViewData<TKey> data);
        Task<T> Read(TKey key);
        Task Update(IViewData<TKey> data);
        Task Delete(TKey key);

        List<T> All { get; }
    }
}