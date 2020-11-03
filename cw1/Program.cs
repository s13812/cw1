using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException("args[0]", "An URL should be passed as a first parameter.");
            }

            bool result = Uri.TryCreate(args[0], UriKind.Absolute, out Uri url)
                && (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                throw new ArgumentException("The passed argument is not a correct URL.");
            }

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK) 
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
                Console.WriteLine($"Blad \"{response.StatusCode}\" w czasie pobierania strony");
            }

            Console.WriteLine("\nKoniec!");
        }
    }
}
