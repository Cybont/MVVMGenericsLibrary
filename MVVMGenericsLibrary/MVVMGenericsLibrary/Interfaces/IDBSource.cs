using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public interface IDBSource<T, TKey>
    {
        Task<TKey> Create(T obj);
        Task<T> Read(TKey key);
        Task Update(T obj);
        Task Delete(TKey key);
        Task<List<T>> Load();
    }
}
