namespace Monzo.Locations.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Monzo.Locations.Framework.Entities;
    using Monzo.Locations.Framework.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The name of the enviroment variable for access token.
        /// </summary>
        private readonly string EnviromentVariableAccessTokenName = "MONZO";

        /// <summary>
        /// The name of the enviroment variable google maps key.
        /// </summary>
        private readonly string EnviromentVariableGoogleMapsKey = "GOOGLEAPI";

        /// <summary>
        /// Locking object for multi threaded processing of the transaction requests. 
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// Home Page.
        /// </summary>
        /// <returns>The index.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var configService = new ConfigurationService();
            var accountService = Monzo.Framework.Factory.CreateAccountService();
            var accounts = accountService.GetAccountsAsync();

            ViewData["googlemapskey"] = configService.GetEnviroment(EnviromentVariableGoogleMapsKey);
           
            Task.WaitAll(accounts);
            ViewData["authenticated"] = accounts.Result.AccountCollection.Any() ? "Authenticated" : "Not Authenticated";

            return View();
        }

        /// <summary>
        /// Gets the transactions for the latest account between the specified dates. 
        /// </summary>
        /// <returns>The transactions.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        [HttpGet]
        public string GetTransactions(string startDate, string endDate)
        {
            if (!DateTime.TryParseExact(startDate, "yyyyMMdd", CultureInfo.CurrentUICulture, DateTimeStyles.None, out DateTime parsedStart))
            {
                throw new ArgumentException($"{nameof(startDate)} could not be parsed"); 
            }

            if (!DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.CurrentUICulture, DateTimeStyles.None, out DateTime parsedEnd))
            {
                throw new ArgumentException($"{nameof(endDate)} could not be parsed"); 
            }

            using (var configService = new ConfigurationService())
            using (var httpService = new HttpService())
            using (var service = new MonzoService(httpService, configService.GetEnviroment(EnviromentVariableAccessTokenName)))
            {
                var account = service.GetAccounts();

                if (account == null || account.AccountList == null || account.AccountList.Count == 0)
                {
                    throw new MissingMemberException("No Accounts found");                     
                }

                var returnedTransactions = new Transactions { TransactionList = new List<Transaction>() };
                var accountTasks = new List<Task>(account.AccountList.Count()); 

                foreach (Account currentAccount in account.AccountList)
                {
                    accountTasks.Add(Task.Factory.StartNew(() =>
                    {
                        var currentTransactions = service.GetPhysicalTransactionsByDate(currentAccount, parsedStart, parsedEnd).TransactionList;

                        lock (lockObject)
                        {
                            returnedTransactions.TransactionList = returnedTransactions.TransactionList.Union(currentTransactions);
                        }
                    }));                                                                            
                }

                Task.WaitAll(accountTasks.ToArray());              
                returnedTransactions.TransactionList = returnedTransactions.TransactionList.OrderBy(x => x.Created); 

                return JsonConvert.SerializeObject(returnedTransactions);      
            }
        }
    }
}