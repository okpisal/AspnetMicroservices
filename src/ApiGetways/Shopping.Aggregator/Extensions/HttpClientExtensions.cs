using System.Text.Json;

namespace Shopping.Aggregator.Extensions
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
    }
}
