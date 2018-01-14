namespace Monzo.Locations.Framework.Entities
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Encapsulates mutltiple Transaction objects. 
    /// </summary>
    public class Transactions
    {
        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        [JsonProperty("transactions")]
        public List<Transaction> TransactionList { get; set; }
    }
}