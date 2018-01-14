namespace Monzo.Locations.Framework.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// Entity to represent an Authentication Response. 
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Gets or sets if the user is authenticated. 
        /// </summary>
        /// <value>The authenticated.</value>
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }

        /// <summary>
        /// Gets or sets the client id. 
        /// </summary>
        /// <value>The client identifier.</value>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the user id. 
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public override string ToString()
        {
            return string.Format("[Authentication: Authenticated={0}, ClientId={1}, UserId={2}]", Authenticated, ClientId, UserId);
        }
    }
}