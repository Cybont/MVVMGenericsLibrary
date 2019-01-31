using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    //For Catelogs
    public interface ICRUD<T, TData, TKey>
    {
        Task<TKey> Create(TData data);
        Task<T> Read(TKey key);
        Task Update(TData data);
        Task Delete(TKey key);

        List<T> All { get; }
        TData DataPackage { get; set; }

        Task LocalCreate(TKey key);
    }
}