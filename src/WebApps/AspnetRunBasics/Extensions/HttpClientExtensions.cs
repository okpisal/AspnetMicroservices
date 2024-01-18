using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspnetRunBasics.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage httpResponseMessage)
        {
            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Something went wrong calling the api : {httpResponseMessage.ReasonPhrase}");
            }
                var DataAsString=await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(DataAsString,new JsonSerializerOptions { PropertyNameCaseInsensitive=true});
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient http,string url,T data)
        {
            var dataAsString=JsonSerializer.Serialize(data);
            var content=new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return http.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient,string url,T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);

        }
    }
}
