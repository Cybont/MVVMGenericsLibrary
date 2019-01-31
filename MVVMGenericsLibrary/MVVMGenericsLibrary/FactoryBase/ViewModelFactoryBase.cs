using System.Collections.Generic;
using System.Linq;
    
    namespace GenericsLibrary
{
    public abstract class ViewModelFactoryBase<TData, T, TKey>
    where T : IKey<TKey>
    where TData : IKey<TKey>
    {
        public abstract ItemViewModelBase<T, TKey> CreateItemViewModel(T obj);
        public virtual List<ItemViewModelBase<T, TKey>> GetItemViewModelCollection(ICRUD<T, TData, TKey> catalog)
        {
            List<ItemViewModelBase<T, TKey>> items = new List<ItemViewModelBase<T, TKey>>();

            foreach (T obj in catalog.All.OrderByDescending(Model => Model.Key))
            {
                items.Add(CreateItemViewModel(obj));
            }
            return items;
        }
        public virtual List<ItemViewModelBase<T, TKey>> GetItemViewModelCollection(ICRUD<T, TData, TKey> catalog, int FK)
        {
            List<ItemViewModelBase<T, TKey>> items = new List<ItemViewModelBase<T, TKey>>();

            foreach (T obj in catalog.All.OrderByDescending(Model => Model.Key))
            {
                items.Add(CreateItemViewModel(obj));
            }
            return items;
        }
    }
}
