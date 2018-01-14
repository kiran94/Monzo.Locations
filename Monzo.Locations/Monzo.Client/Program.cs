namespace Monzo.Client
{
    using System;
    using System.Linq; 
    using Monzo.Locations.Framework.Services;

    public class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var httpService = new HttpService();
            var configService = new ConfigurationService();
            var service = new MonzoService(httpService, configService);

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

            var accounts = service.GetAccounts();
            foreach(var account in accounts.AccountList)
            {
                Console.WriteLine(account.ID);
            }

            var transactions = service.GetPhysicalTransactions(accounts.AccountList.First()); 

            foreach(var t in transactions.TransactionList)
            {
                Console.Write(t.ID + "\t" + t.IsOnline); 
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