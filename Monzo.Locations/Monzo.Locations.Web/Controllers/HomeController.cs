namespace Monzo.Locations.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
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
        private string EnviromentVariableAccessTokenName = "MONZO";

        /// <summary>
        /// The name of the enviroment variable google maps key.
        /// </summary>
        private string EnviromentVariableGoogleMapsKey = "GOOGLEAPI"; 

        /// <summary>
        /// Home Page.
        /// </summary>
        /// <returns>The index.</returns>
        [HttpGet]
        public ActionResult Index()
        {                      
            // have to launch vs from the terminal because of this issue: https://www.placona.co.uk/1592/dotnet/osx-pro-tip-for-environment-variables/
            using (var configService = new ConfigurationService())
            using (var httpService = new HttpService())
            using (var service = new MonzoService(httpService, configService.GetEnviroment(EnviromentVariableAccessTokenName)))
            {
                var auth = service.GetAuthentication();
                ViewData["authenticated"] = auth.Authenticated ? "Authenticated" : "Not Authenticated";
                ViewData["googlemapskey"] = configService.GetEnviroment(EnviromentVariableGoogleMapsKey); 
            }

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
                var auth = service.GetAuthentication(); 
                var account = service.GetAccounts();

                if (account == null || account.AccountList == null || account.AccountList.Count == 0)
                {
                    throw new MissingMemberException("No Accounts found");                     
                } 

                var latestAccount = account.AccountList.Last();
                var transactions = service.GetPhysicalTransactionsByDate(latestAccount, parsedStart, parsedEnd);
                return JsonConvert.SerializeObject(transactions);          
            }
        }
    }
}