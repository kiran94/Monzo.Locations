namespace Monzo.Locations.Framework.Contracts
{
    using System;
    using Monzo.Locations.Framework.Entities;

    /// <summary>
    /// Encapsulates logic to interact with the Monzo API. 
    /// </summary>
    public interface IMonzoService
    {
        /// <summary>
        /// Gets authentication details. 
        /// </summary>
        /// <returns>The authentication object.</returns>
        Authentication GetAuthentication(); 
    }
}