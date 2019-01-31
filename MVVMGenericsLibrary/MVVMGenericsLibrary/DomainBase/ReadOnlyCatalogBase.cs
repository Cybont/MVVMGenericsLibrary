using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericsLibrary;

namespace Blomstertonden
{
    public class ReadOnlyCatalogBase<T, TKey>
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

        public async void Load()
        {
            List<T> data = await _dataSource.Load();
            foreach (T t in data)
            {
                _data.Add(t.Key, t);
            }
        }
    }
}
