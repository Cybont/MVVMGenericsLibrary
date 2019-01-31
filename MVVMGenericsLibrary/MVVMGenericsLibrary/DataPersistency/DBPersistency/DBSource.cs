using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace GenericsLibrary
{
    public class DBSource<T, TKey> : IDBSource<T, TKey>
        where T : IKey<TKey>
    {
        private enum APIMethod { Load, Create, Read, Update, Delete }

        private string _serverURL;
        private string _apiPrefix;
        private string _apiId;
        private HttpClientHandler _httpClientHandler;
        private HttpClient _httpClient;

        public DBSource(string serverUrl, string apiId, string apiPrefix = "api")
        {
            _serverURL = serverUrl;
            _apiId = apiId;
            _apiPrefix = apiPrefix;
            _httpClientHandler = new HttpClientHandler {UseDefaultCredentials = true};
            _httpClient = new HttpClient(_httpClientHandler) {BaseAddress = new Uri(_serverURL)};
        }

        public async Task<List<T>> Load()
        {
            return await InvokeAPIWithReturnValueAsync<List<T>>(() => _httpClient.GetAsync(BuildRequestURI(APIMethod.Load)));
        }
        #region CRUD
        public async Task<TKey> Create(T obj)
        {
            HttpResponseMessage response = await InvokeAPIAsync(() => _httpClient.PostAsJsonAsync(BuildRequestURI(APIMethod.Create), obj));
            T createdObj = await response.Content.ReadAsAsync<T>();
            return createdObj.Key;
        }
        public async Task<T> Read(TKey key)
        {
            return await InvokeAPIWithReturnValueAsync<T>(() => _httpClient.GetAsync(BuildRequestURI(APIMethod.Read, key)));
        }
        //public async Task<T> Read(TKey key, TKey key2)
        //{
        //    return await InvokeAPIWithReturnValueAsync<T>(() => _httpClient.GetAsync(BuildRequestURI())
        //}
        public async Task Update(T obj)
        {
            await InvokeAPINoReturnValueAsync(() => _httpClient.PutAsJsonAsync(BuildRequestURI(APIMethod.Update, obj.Key), obj));
        }
        public async Task Delete(TKey key)
        {
            await InvokeAPINoReturnValueAsync(() => _httpClient.DeleteAsync(BuildRequestURI(APIMethod.Update, key)));
        }
        #endregion

        #region Private method for API method invocation
        private async Task<U> InvokeAPIWithReturnValueAsync<U>(Func<Task<HttpResponseMessage>> apiMethod)
        {
            HttpResponseMessage response = await InvokeAPIAsync(apiMethod);
            return await response.Content.ReadAsAsync<U>();
        }

        private async Task InvokeAPINoReturnValueAsync(Func<Task<HttpResponseMessage>> apiMethod)
        {
            await InvokeAPIAsync(apiMethod);
        }
        private async Task<HttpResponseMessage> InvokeAPIAsync(Func<Task<HttpResponseMessage>> apiMethod)
        {
            // Prepare HTTP client for method invocation
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Invoke the method - the method will at some point 
            // return an HttpResponseMessage 
            HttpResponseMessage response = await apiMethod();
            // Throw exception if the invocation was unsuccessful
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            // Return the HttpResponseMessage, which we now know 
            // is a response to a successful method invocation
            return response;
        }
        private string BuildRequestURI(APIMethod method, TKey key = default (TKey))
        {
            switch (method)
            {
                case APIMethod.Load:
                    return $"{_apiPrefix}/{_apiId}";
                case APIMethod.Create:
                    return $"{_apiPrefix}/{_apiId}";
                case APIMethod.Read:
                    return $"{_apiPrefix}/{_apiId}/{key}";
                case APIMethod.Update:
                    return $"{_apiPrefix}/{_apiId}/{key}";
                case APIMethod.Delete:
                    return $"{_apiPrefix}/{_apiId}/{key}";
                default:
                    //Shouldn't it be URL??
                    throw new ArgumentException("BuildRequestURI");
            }
        }

        #endregion
    }
}
