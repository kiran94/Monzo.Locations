namespace Monzo.Client
{
    using System;
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

            Console.ReadLine();

        }
    }
}