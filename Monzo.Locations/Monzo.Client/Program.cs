using System;
using System.Collections.Generic;
using Monzo.Locations.Framework.Services;

namespace Monzo.Client
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var httpService = new HttpService();
            var configService = new ConfigurationService();
            var service = new MonzoService(httpService, configService);  


            Console.WriteLine(service.GetAuthentication());
            Console.ReadLine();

        }

       
    }
}
