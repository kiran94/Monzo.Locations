namespace Monzo.Locations.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Monzo.Locations.Framework.Contracts;

    /// <inheritdoc/>
    public class HttpService : IHttpService
    {
        /// <inheritdoc/>
        public async Task<string> GetJson(string url, IDictionary<string, string> headers = null)
        {
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                {
                    foreach (var keyvalue in headers)
                    {
                        client.DefaultRequestHeaders.Add(
                          keyvalue.Key,
                          keyvalue.Value);
                    }
                }

                var response = await client.GetAsync(new Uri(url));
                return await response.Content.ReadAsStringAsync();
            }          
        }
    }
}