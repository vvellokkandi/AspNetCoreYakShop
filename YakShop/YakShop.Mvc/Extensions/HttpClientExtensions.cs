using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostBasicAsync(this HttpClient client, string uri, object content)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                string json = "{}";
                if (content != null)
                    json = JsonConvert.SerializeObject(content);

                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    return await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false);

                }
            }
        }
    }
}
