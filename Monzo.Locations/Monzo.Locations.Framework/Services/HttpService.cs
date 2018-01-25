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
        public async Task<string> GetJson(string url, IDictionary<string, string> headers)
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

        #region IDisposable Support

        /// <summary>
        /// Flag indicating if the class is disposed or not.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Dispose the resources if we are disposing from the Dispose method and not the finaliser..
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                disposedValue = true;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:Monzo.Locations.Framework.Services.HttpService"/> is reclaimed by garbage collection.
        /// </summary>
         ~HttpService() 
        {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
         }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Monzo.Locations.Framework.Services.HttpService"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Monzo.Locations.Framework.Services.HttpService"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Monzo.Locations.Framework.Services.HttpService"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Monzo.Locations.Framework.Services.HttpService"/> so the garbage collector can reclaim the
        /// memory that the <see cref="T:Monzo.Locations.Framework.Services.HttpService"/> was occupying.</remarks>
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);          
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}