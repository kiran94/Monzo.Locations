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
        /// Gets or sets the ID for the Merchant..
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the address of the Merchant. 
        /// </summary>
        /// <value>The address.</value>
        [JsonProperty("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Monzo.Locations.Framework.Entities.Transaction"/>
        /// is online.
        /// </summary>
        /// <value><c>true</c> if is online; otherwise, <c>false</c>.</value>
        [JsonProperty("online")]
        public bool IsOnline { get; set; }
    }
}