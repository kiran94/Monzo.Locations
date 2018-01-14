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
        /// Gets or sets the amount in pennies. 
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        public int Amount { get; set; }
               
        /// <summary>
        /// Gets or sets the merchant.
        /// </summary>
        /// <value>The merchant.</value>
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }

        /// <summary>
        /// Gets or sets the date time the Transaction was created. 
        /// </summary>
        /// <value>The created.</value>
        [JsonProperty("created")]
        public DateTime Created { get; set; }
    }
}