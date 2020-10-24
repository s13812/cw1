using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var url = "";

            try
            {
                url = args[0];
            }
            catch
            {
                throw new ArgumentNullException("arg[0]");
            }            

            var urlRegex = new Regex("^(https?)://[-a-zA-Z0-9+&@#/%?=~_|!:,.;]*[-a-zA-Z0-9+&@#/%=~_|]");

            if (!urlRegex.IsMatch(url))
            {
                throw new ArgumentException("arg[0] nie jest poprawnym adresem URL");
            }

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            httpClient.Dispose();

            if (response.IsSuccessStatusCode) 
            {
                var html = await response.Content.ReadAsStringAsync();
                var regex = new Regex("[a-z0-9]+@[a-z.]+");

                var matches = regex.Matches(html);

                if (matches.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono adresow email");
                } else
                {
                    foreach (var i in (from Match m in matches select m.Value).Distinct())
                    {
                        Console.WriteLine(i);
                    }
                }

            } else
            {
                Console.WriteLine("Blad w czasie pobierania strony");
            }

            Console.WriteLine("\nKoniec!");
        }
    }
}
