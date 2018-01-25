namespace Monzo.Client
{
    using System;
    using System.Linq; 
    using Monzo.Locations.Framework.Services;
    using static System.Console; 

    /// <summary>
    /// Main class.
    /// </summary>
    public static class MainClass
    {
        /// <summary>
        /// The name of the enviroment variable for access token.
        /// </summary>
        private static string EnviromentVariableAccessTokenName = "MONZO";

        /// <summary>
        /// The section break.
        /// </summary>
        private static string SectionBreak = new string('-', 12); 

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            using(var configService = new ConfigurationService())
            using(var httpService = new HttpService())
            using(var service = new MonzoService(httpService, configService.GetEnviroment(EnviromentVariableAccessTokenName)))
            {
                WriteLine(SectionBreak); 
                var auth = service.GetAuthentication(); 

                WriteLine(auth);
                ForegroundColor = (auth.Authenticated) ? ConsoleColor.Green : throw new UnauthorizedAccessException(auth.ToString()); 

                WriteLine(SectionBreak); 

                // Print Detected Acccounts. 
                WriteLine("ID\tDesciption\t");
                var accounts = service.GetAccounts();
                foreach(var account in accounts.AccountList)
                {
                    WriteLine(account.ID + "\t" + account.Description);
                }

                WriteLine(SectionBreak); 

                // Print Physical Transactions by Date.
                WriteLine("ID\tName\tCreated\tLatitude\tLongitude\t");

                var transactions = service.GetPhysicalTransactionsByDate(accounts.AccountList.ElementAt(0), new DateTime(2017, 12, 22), new DateTime(2018, 01, 03));
                transactions.TransactionList = transactions.TransactionList.Where(x => x.Merchant != null && x.Merchant.Address != null);

                foreach (var t in transactions.TransactionList.OrderBy(x => x.Created))
                {
                    WriteLine(t.ID + "\t" + t.Merchant.Name + "\t" + t.Created + "\t" + t.Merchant.Address.Latitude + "\t" + t.Merchant.Address.longitude);
                }                                    
            }

            ReadKey();
        }
    }
}