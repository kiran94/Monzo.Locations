namespace Monzo.Locations.Framework.Entities
{

    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Encapsulates the Accounts response. 
    /// </summary>
    public class Accounts
    {        
        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>The accounts.</value>
        [JsonProperty("accounts")]
        public List<Account> AccountList { get; set; }
    }
}
