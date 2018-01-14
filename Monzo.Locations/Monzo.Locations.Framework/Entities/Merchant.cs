namespace Monzo.Locations.Framework.Entities
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Encapsulates the details for a Merchant. 
    /// </summary>
    public class Merchant
    {
        /// <summary>
        /// Gets or sets the address of the Merchant. 
        /// </summary>
        /// <value>The address.</value>
        [JsonProperty("address")]
        public Address Address { get; set; }
    }
}