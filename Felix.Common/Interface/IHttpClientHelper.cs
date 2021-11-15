using Felix.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Common.Interface
{
    public interface IHttpClientHelper
    {
        IHttpClientHelper CreateAccessToken();
        IHttpClientHelper SetAccessToken();
        Task<Envelope<T>> SendAsync<T>(string url);
        Task<Envelope<T>> PostAsync<T>(string url, Dictionary<string, string> values);
        Task<Envelope<T>> PostAsync<T>(string url, object json);
        Task<Envelope<T>> PutAsync<T>(string url, object json);
        Task<Envelope<T>> GetAsync<T>(string url);
    }
}
