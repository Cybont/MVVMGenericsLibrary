using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsLibrary
{
    /// <summary>
    /// File-specific implementation of IDataSource... interfaces. 
    /// Ties a IStringPersistence implementation to a IDataConverter 
    /// implementation.
    /// </summary>
    /// <typeparam name="T">
    /// Type of objects to load/save.
    /// </typeparam>
    public class FileSource<T, TKey> : IFileSource<TKey, T>
        where T : IKey<TKey>
    {
        private string _fileName;
        private IStringPersistence _stringPersistence;
        private IStringConverter<T> _stringConverter;

        /// <summary>
        /// If nothing else is specified, data is stored 
        /// in a text file  called (NameOfClass)Model.dat, 
        /// for instance CarModel.dat.
        /// </summary>
        public FileSource(IStringPersistence stringPersistence, IStringConverter<T> stringConverter, string fileSuffix = "Model.dat")
        {
            _stringPersistence = stringPersistence;
            _stringConverter = stringConverter;
            _fileName = typeof(T).Name + fileSuffix;
        }


        public List<T> ListFromDictionary(Dictionary<TKey, T> dictionary)
        {
            List<T> list = new List<T>();
            foreach (var item in dictionary)
            {
                list.Add(item.Value);
            }
            return list;
        }

        public Dictionary<TKey, T> DictionaryFromList(List<T> list)
        {
            Dictionary<TKey, T> dictionary = new Dictionary<TKey, T>();
            foreach (var item in list)
            {
                dictionary.Add(item.Key, item);
            }
            return dictionary;
        }

        /// <summary>
        /// Loads objects from file
        /// </summary>
        /// <returns>
        /// List of loaded objects, wrapped in an awaitable Task.
        /// </returns>
        /// 

        public async Task<Dictionary<TKey, T>> Load()
        {
            string data = await _stringPersistence.LoadAsync(_fileName);
            List<T> list= _stringConverter.ConvertFromString(data);
            return DictionaryFromList(list);
        }

        /// <summary>
        /// Saves objects to file
        /// </summary>
        /// <param name="objects">
        /// List of objects to save
        /// </param>
        public Task Save(Dictionary<TKey, T> objects)
        {
            List<T> list = ListFromDictionary(objects);
            string data = _stringConverter.ConvertToString(list);
            return _stringPersistence.SaveAsync(_fileName, data);
        }
    }
}