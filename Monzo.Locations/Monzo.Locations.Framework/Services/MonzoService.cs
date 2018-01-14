namespace Monzo.Locations.Framework.Services
{
    using System;
    using System.Linq; 
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
        /// The access token.
        /// </summary>
        private readonly string AccessToken; 

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Monzo.Locations.Framework.Services.MonzoService"/> class.
        /// </summary>
        /// <param name="httpService">Http service.</param>
        public MonzoService(IHttpService httpService, string accessToken)
        {
            this.httpService = httpService;
            this.AccessToken = accessToken; 
        }

        /// <inheritdoc />
        public Authentication GetAuthentication()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                throw new ArgumentException($"Ensure env variable is set."); 
            }
                       
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    "Bearer " + this.AccessToken
                }
            };

          
            var getJsonTask = httpService.GetJson("https://api.monzo.com/ping/whoami", headers);
            getJsonTask.Wait();

            return JsonConvert.DeserializeObject<Authentication>(getJsonTask.Result);                     
        }

        /// <inheritdoc />
        public Accounts GetAccounts()
        {          
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                throw new ArgumentException($"Ensure env variable is set."); 
            }
                       
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    "Bearer " + this.AccessToken
                }
            };

          
            var getJsonTask = httpService.GetJson("https://api.monzo.com/accounts", headers);
            getJsonTask.Wait();

            return JsonConvert.DeserializeObject<Accounts>(getJsonTask.Result); 
        }

        /// <inheritdoc />
        public Transactions GetTransactions(Account account)
        {           
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                throw new ArgumentException($"Ensure {this.AccessToken} env variable is set."); 
            }
                       
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    "Bearer " + this.AccessToken
                }
            };

          
            var getJsonTask = httpService.GetJson($"https://api.monzo.com/transactions?expand[]=merchant&account_id={account.ID}", headers);
            getJsonTask.Wait();

            return JsonConvert.DeserializeObject<Transactions>(getJsonTask.Result); 
        }

        /// <inheritdoc />
        public Transactions GetPhysicalTransactions(Account account)
        {
            var transactions = this.GetTransactions(account);
            transactions.TransactionList = transactions.TransactionList
                .Where(x => x.Merchant != null
                       && !x.Merchant.IsOnline
                       && x.Merchant.ID != "merch_0000987lak9C9IRzz93Xaj"); // temp fix to filter out TFL transactions as they come out as offline transactions. But the Address seems to be thier office.

            return transactions; 
        }

        /// <inheritdoc />
        public Transactions GetPhysicalTransactionsByDate(Account account, DateTime start, DateTime end)
        {
            var transactions = this.GetPhysicalTransactions(account);
            transactions.TransactionList = transactions.TransactionList.Where(x => x.Created >= start && x.Created <= end);
            return transactions; 
        }
    }
}