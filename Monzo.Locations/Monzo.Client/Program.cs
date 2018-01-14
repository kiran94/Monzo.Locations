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

            var tt = test(); 
            tt.Wait();

            Console.WriteLine(tt.Result);
            Console.ReadLine();

        }

        public static async System.Threading.Tasks.Task<string> test()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {
                    "Authorization",
                    ""
                }
            }; 

            HttpService service = new HttpService();
            return await service.GetJson("https://api.monzo.com/ping/whoami", headers);
        }
    }
}
