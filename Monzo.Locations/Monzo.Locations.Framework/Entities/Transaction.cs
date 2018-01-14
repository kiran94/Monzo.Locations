namespace Monzo.Locations.Framework.Entities
{
    using System;
    using Newtonsoft.Json;

    public sealed class Transaction
    {
        /// <summary>
        /// Gets or sets the Transaction ID.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the amount in pennies. 
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Monzo.Locations.Framework.Entities.Transaction"/>
        /// is online.
        /// </summary>
        /// <value><c>true</c> if is online; otherwise, <c>false</c>.</value>
        [JsonProperty("online")]
        public bool IsOnline { get; set; }

        /// <summary>
        /// Gets or sets the merchant.
        /// </summary>
        /// <value>The merchant.</value>
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }
    }
}