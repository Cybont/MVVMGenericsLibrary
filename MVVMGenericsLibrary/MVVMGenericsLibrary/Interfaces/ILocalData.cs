using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public interface ILocalData<TKey>
    {
        Task LocalCreate(TKey key);
    }
}
