namespace Monzo.Locations.Framework.Entities
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Encapsulates a single Accounts details. 
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the AccountID.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the created date. 
        /// </summary>
        /// <value>The created.</value>
        [JsonProperty("created")]
        public DateTime Created { get; set; } 
    }
}