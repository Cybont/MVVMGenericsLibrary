using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    //For Data Sources
    public interface IFileSource<TKey, T>
    {
        Task<Dictionary<TKey, T>> Load();
        Task Save(Dictionary<TKey, T> objects);
    }
}
