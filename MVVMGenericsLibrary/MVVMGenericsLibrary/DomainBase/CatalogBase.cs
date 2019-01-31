using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public abstract class CatalogBase<TData, T, TKey>
        where T : IKey<TKey>
        where TData : IKey<TKey>
    {
        protected Dictionary<TKey, T> _data;
        protected IFactory<TData, T> _factory;
        protected FileSource<T, TKey> _dataSource;
        protected CatalogBase(IFactory<TData, T> factory)
        {
            _factory = factory;
            _data = new Dictionary<TKey, T>();
            _dataSource = new FileSource<T, TKey>(new FileStringPersistence(), new JSONConverter<T>());
            Load();
        }
        public List<T> All => _data.Values.ToList();
        public Dictionary<TKey, T> Data => _data;

        public TData DataPackage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async void Save()
        {
            await _dataSource.Save(Data);
        }
        private async void Load()
        {
            _data = await _dataSource.Load();
        }
        public virtual async Task Create(TData data)
        {
            T obj = _factory.Convert(data);
            TKey newKey = NextKey();
            obj.Key = newKey;
            _data.Add(newKey, obj);
            Save();
        }
        public Task<T> Read(TKey key)
        {
            throw new NotImplementedException();
        }
        public virtual async Task Update(TData data)
        {
            T obj = _factory.Convert(data);
            _data.Remove(obj.Key);
            _data.Add(obj.Key, obj);
            Save();
        }
        public virtual async Task Delete(TKey key)
        {
            _data.Remove(key);
            Save();
        }
        public abstract TKey NextKey();

 
    }
}
