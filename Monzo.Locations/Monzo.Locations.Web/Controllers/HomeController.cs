namespace Monzo.Locations.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using Monzo.Locations.Framework.Services;
 
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
        /// Home Page.
        /// </summary>
        /// <returns>The index.</returns>
        public ActionResult Index()
        {                      
            // have to launch vs from the terminal because of this issue: https://www.placona.co.uk/1592/dotnet/osx-pro-tip-for-environment-variables/
            using (var configService = new ConfigurationService())
            using (var httpService = new HttpService())
            using (var service = new MonzoService(httpService, configService.GetEnviroment(EnviromentVariableAccessTokenName)))
            {
                var auth = service.GetAuthentication();
                ViewData["authenticated"] = auth.Authenticated ? "Authenticated" : "Not Authenticated";  
            }

            return View(); 
        }
    }
}