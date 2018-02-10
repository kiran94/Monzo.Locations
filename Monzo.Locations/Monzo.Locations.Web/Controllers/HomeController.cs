namespace Monzo.Locations.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Monzo.Framework.Entities;
    using Monzo.Locations.Framework.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
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
            ViewData["BaseURL"] = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

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

            var accountService = Monzo.Framework.Factory.CreateAccountService();
            var accounts = accountService.GetAccountsAsync();

            var transactionService = Monzo.Framework.Factory.CreateTransactionService();
            var transactions = new List<Monzo.Framework.Entities.Transaction>();
            var accountTasks = new List<Task>(accounts.Result.AccountCollection.Count());

            foreach (var account in accounts.Result.AccountCollection)
            {
                accountTasks.Add(Task.Factory.StartNew(() =>
                {
                    var currentTransactions = transactionService.GetTransactionsByDateAsync(account, parsedStart, parsedEnd)
                                                           .Result
                                                           .TransactionCollection
                                                           .Where(x => x.Merchant != null
                                                                  && x.Merchant.Address != null
                                                           //&& !x.Merchant.IsOnline
                                                           // temp fix to filter out TFL transactions as they come out as offline transactions. 
                                                           // But the Address seems to be thier office.
                                                           && x.Merchant.ID != "merch_0000987lak9C9IRzz93Xaj");
                    lock (lockObject)
                    {
                        transactions = transactions.Union(currentTransactions).ToList();
                    }                                      
                }));                                                              
            }

            Task.WaitAll(accountTasks.ToArray());
            return JsonConvert.SerializeObject(transactions);           
        }
    }
}