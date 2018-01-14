namespace Monzo.Locations.Framework.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Encapsualtes logic for interacting via HTTP. 
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Makes a GET response using the given url to get JSON data. 
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="url">URL.</param>
        /// <param name="headers">(optional) headers for the HTTP request.</param>
        Task<string> GetJson(string url, IDictionary<string, string> headers = null); 
    }
}