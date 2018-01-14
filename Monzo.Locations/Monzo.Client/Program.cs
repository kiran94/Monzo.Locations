namespace Monzo.Client
{
    using System;
    using System.Linq; 
    using Monzo.Locations.Framework.Services;

    public class MainClass
    {
        /// <summary>
        /// The name of the enviroment variable for access token.
        /// </summary>
        private static string EnviromentVariableAccessTokenName = "MONZO";

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var httpService = new HttpService();
            var configService = new ConfigurationService();
            var accesstoken = configService.GetEnviroment(EnviromentVariableAccessTokenName);
                      
            var service = new MonzoService(httpService, accesstoken);

            var auth = service.GetAuthentication(); 

            if (auth.Authenticated)
            {
                Console.ForegroundColor = ConsoleColor.Green; 
                Console.WriteLine(auth);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(auth);
            }

            Console.WriteLine("ID\tDesciption\t");
            var accounts = service.GetAccounts();
            foreach(var account in accounts.AccountList)
            {
                Console.WriteLine(account.ID + "\t" + account.Description);
            }


            Console.WriteLine("ID\tName\tCreated\tLatitude\tLongitude\t");
            var transactions = service.GetPhysicalTransactionsByDate(accounts.AccountList.ElementAt(0), new DateTime(2017, 12, 22), new DateTime(2018, 01, 03)); 

            foreach(var t in transactions.TransactionList.OrderBy(x => x.Created))
            {
                Console.Write(t.ID + "\t" + t.Merchant.Name + "\t" + t.Created); 
                if (t.Merchant != null)
                {
                    if (t.Merchant.Address != null)
                    {
                        Console.Write("\t" + t.Merchant.Address.Latitude + "\t" + t.Merchant.Address.longitude);
                    }
                }

                Console.WriteLine(string.Empty);
            }

            Console.ReadLine();

        }
    }
}