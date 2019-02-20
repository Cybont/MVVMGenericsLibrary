using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    public abstract class CatalogBaseDB<TData, T, TKey> : IEnumerable<T>, ICRUD<T, TData, TKey>
        where T : IKey<TKey>
        where TData : IKey<TKey>, new()
    {
        protected Dictionary<TKey, T> _data;
        protected IFactory<TData, T> _factory;
        private IDBSource<T, TKey> _dataSource;
        protected TData _dataPackage;

        #region Constructors
        protected CatalogBaseDB(IFactory<TData, T> factory, string serverURL, string apiId)
        {
            _dataSource = new DBSource<T, TKey>(serverURL, apiId);
            _factory = factory;
            _data = new Dictionary<TKey, T>();
            _dataPackage = new TData();
        }
        #endregion

        #region Properties
        public TData DataPackage
        {
            get => _dataPackage;
            set => _dataPackage = value;
        }
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
        //public virtual async Task Create(TData data, bool nextKey)
        //{
        //    T obj = _factory.Convert(data);
        //    if (nextKey)
        //    {
        //        TKey newKey = NextKey();
        //        obj.Key = newKey;
        //        await _dataSource.Create(obj);
        //    }
        //    _data.Add(obj.Key, obj);

        //}
        public virtual async Task<TKey> Create(TData data)
        {
            // DB
            T obj = _factory.Convert(data);
            obj.Key = await _dataSource.Create(obj);

            // Local
            _data.Add(obj.Key, await Read(obj.Key));
            return obj.Key;
        }
        public async Task<T> Read(TKey key)
        {
            return await _dataSource.Read(key);
        }
        public virtual async Task Update(TData data)
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
