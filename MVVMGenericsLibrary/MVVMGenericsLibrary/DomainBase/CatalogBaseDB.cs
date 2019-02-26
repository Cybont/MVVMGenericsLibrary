using MVVMGenericsLibrary.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public abstract class CatalogBaseDB<T, TKey> : IEnumerable<T>, ICRUD<T, TKey>
        where T : IKey<TKey>
    {
        protected Dictionary<TKey, T> _data;
        protected IFactory<T, TKey> _factory;
        private IDBSource<T, TKey> _dataSource;

        #region Constructors
        protected CatalogBaseDB(IFactory<T, TKey> factory, string serverURL, string apiId)
        {
            _dataSource = new DBSource<T, TKey>(serverURL, apiId);
            _factory = factory;
            _data = new Dictionary<TKey, T>();
        }
        #endregion

        #region Properties
        public T this[TKey key]
        {
            get { return Read(key).Result; }
        }
        public List<T> All => _data.Values.ToList();
        public Dictionary<TKey, T> Data => _data;

        public IDBSource<T, TKey> DataSource { get => _dataSource; set => _dataSource = value; }
        #endregion

        public async void Load()
        {
            List<T> data = await _dataSource.Load();
            foreach (T t in data)
            {
                _data.Add(t.Key, t);
            }
        }

        #region DB_CRUD
        public virtual async Task<TKey> Create(IViewData<TKey> data, bool nextKey)
        {
            T obj = _factory.Convert(data);

            // DB
            if (nextKey)
            {
                TKey newKey = NextKey();
                obj.Key = newKey;
                await _dataSource.Create(obj);
            }
            else
            {
                obj.Key = await _dataSource.Create(obj);
            }
            
            // Local
            _data.Add(obj.Key, await Read(obj.Key));
            return obj.Key;
        }

        public async Task<T> Read(TKey key)
        {
            return await _dataSource.Read(key);
        }
        public virtual async Task Update(IViewData<TKey> data)
        {
            // DB
            T obj = _factory.Convert(data);
            await _dataSource.Update(obj);

            // Local
            _data.Remove(obj.Key);
            _data.Add(obj.Key, await Read(obj.Key));
        }
        public virtual async Task Delete(TKey key)
        {
            // DB
           await _dataSource.Delete(key);

            // Local
           _data.Remove(key);
        }
        #endregion

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return All.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public async Task LocalCreate(TKey key)
        {
            Data.Add(key, await Read(key));
        }

        public abstract TKey NextKey();


    }
}
