namespace Monzo.Locations.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using Monzo.Locations.Framework.Contracts;
    using Monzo.Locations.Framework.Entities;
    using Newtonsoft.Json;

    /// <inheritdoc />
    public class MonzoService : IMonzoService
    {
        /// <summary>
        /// The http service.
        /// </summary>
        private readonly IHttpService httpService;

        /// <summary>
        /// The config service.
        /// </summary>
        private readonly IConfigurationService configService;

        /// <summary>
        /// The name of the enviroment variable for access token.
        /// </summary>
        private const string EnviromentVariableAccessTokenName = "MONZO"; 

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Monzo.Locations.Framework.Services.MonzoService"/> class.
        /// </summary>
        /// <param name="httpService">Http service.</param>
        /// <param name="configService">Config service.</param>
        public MonzoService(IHttpService httpService, IConfigurationService configService)
        {
            this.httpService = httpService;
            this.configService = configService; 
        }

        /// <inheritdoc />
        public Authentication GetAuthentication()
        {
           
            var accesstoken = this.configService.GetEnviroment(EnviromentVariableAccessTokenName);

            if (string.IsNullOrEmpty(accesstoken))
            {
                throw new ArgumentException($"Ensure {EnviromentVariableAccessTokenName} env variable is set."); 
            }
                       
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    "Bearer " + accesstoken
                }
            };

          
            var getJsonTask = httpService.GetJson("https://api.monzo.com/ping/whoami", headers);
            getJsonTask.Wait();

            return JsonConvert.DeserializeObject<Authentication>(getJsonTask.Result);                     
        }

        /// <inheritdoc />
        public Accounts GetAccounts()
        {
            var accesstoken = this.configService.GetEnviroment(EnviromentVariableAccessTokenName);

            if (string.IsNullOrEmpty(accesstoken))
            {
                throw new ArgumentException($"Ensure {EnviromentVariableAccessTokenName} env variable is set."); 
            }
                       
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    "Bearer " + accesstoken
                }
            };

          
            var getJsonTask = httpService.GetJson("https://api.monzo.com/accounts", headers);
            getJsonTask.Wait();

            return JsonConvert.DeserializeObject<Accounts>(getJsonTask.Result); 
        }

        /// <inheritdoc />
        public Transactions GetTransactions(Account account)
        {
            var accesstoken = this.configService.GetEnviroment(EnviromentVariableAccessTokenName);

            if (string.IsNullOrEmpty(accesstoken))
            {
                throw new ArgumentException($"Ensure {EnviromentVariableAccessTokenName} env variable is set."); 
            }
                       
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    "Bearer " + accesstoken
                }
            };

          
            var getJsonTask = httpService.GetJson($"https://api.monzo.com/transactions?expand[]=merchant&account_id={account.ID}", headers);
            getJsonTask.Wait();

            return JsonConvert.DeserializeObject<Transactions>(getJsonTask.Result); 
        }
    }
}