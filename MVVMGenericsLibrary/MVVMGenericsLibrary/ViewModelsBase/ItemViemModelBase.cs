using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{ 
    public class ItemViewModelBase<T, TKey>
        where  T : IKey<TKey>
    {
        public ItemViewModelBase(T obj)
        {
            Obj = obj;
        }
        public T Obj;
    }
}
