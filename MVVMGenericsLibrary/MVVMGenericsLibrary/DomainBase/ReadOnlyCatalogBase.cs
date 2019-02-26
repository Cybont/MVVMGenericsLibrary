using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericsLibrary;

namespace Blomstertonden
{
    public class ReadOnlyCatalogBase<T, TKey> : IEnumerable<T>
        where T : IKey<TKey>
    {
        private DBSource<T, TKey> _dataSource;
        private Dictionary<TKey, T> _data;
        public ReadOnlyCatalogBase(string serverURL, string apiId)
        {
            _data = new Dictionary<TKey, T>();
            _dataSource = new DBSource<T, TKey>(serverURL, apiId);
             Load();
        }

        public Dictionary<TKey,T> All
        {
            get => _data;
        }

        public List<T> AllList
        {
            get; set;
        }
        
        public T this[TKey key]
        {
            get { return Read(key).Result; }
        }
        
        public async void Load()
        {
            List<T> data = await _dataSource.Load();
            AllList = data;
            foreach (T t in data)
            {
                _data.Add(t.Key, t);
            }
        }

        public async Task<T> Read(TKey key)
        {
            return await _dataSource.Read(key);
        }

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return AllList.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
