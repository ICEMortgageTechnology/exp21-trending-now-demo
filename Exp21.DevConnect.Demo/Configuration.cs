using IMT.DeveloperConnect.API;
using IMT.DeveloperConnect.API.OAuth2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Exp21.DevConnect.Demo
{
    internal static class Configuration
    {
        public static string HostUrl { get; } = ArgumentChecks.IsNotNullOrEmpty(ConfigurationManager.AppSettings[nameof(HostUrl)], nameof(HostUrl));

        public static UserCredentials UserCredentials { get; } = new UserCredentials(
                    ArgumentChecks.IsNotNullOrEmpty(ConfigurationManager.AppSettings["UserName"], "UserName"),
                    ArgumentChecks.ValueOrDefault(ConfigurationManager.AppSettings["UserPwd"]?.ToSecureString(),
                        () =>
                        {
                            Console.WriteLine("Please Enter Password for user: " + ConfigurationManager.AppSettings["UserName"]);
                            return ReadSecureString();
                        }));

        public static ClientCredentials ClientCredentials { get; } = new ClientCredentials(
                    ArgumentChecks.IsNotNullOrEmpty(ConfigurationManager.AppSettings["ClientId"], "ClientId"),
                    ArgumentChecks.ValueOrDefault(ConfigurationManager.AppSettings["ClientSecret"]?.ToSecureString(), 
                        () =>
                        {
                            Console.WriteLine("Please Enter ClientSecret for clientId: " + ConfigurationManager.AppSettings["ClientId"]);
                            return ReadSecureString();
                        }));

        public static string LoanNumber { get; } = ArgumentChecks.IsNotNullOrEmpty(ConfigurationManager.AppSettings[nameof(LoanNumber)], nameof(LoanNumber));

        public static string LoanFolder { get; } = ArgumentChecks.IsNotNullOrEmpty(ConfigurationManager.AppSettings[nameof(LoanFolder)], nameof(LoanFolder));

        public static async Task<IApiSession> GetOAuth2Session()
        {
            var session = new OAuth2Session(HostUrl, ClientCredentials, UserCredentials);
            await session.GetAccessToken();
            return session;
        }

        public static SecureString ReadSecureString()
        {
            var secureString = new SecureString();
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && secureString.Length > 0)
                {
                    Console.Write("\b \b");
                    secureString.RemoveAt(secureString.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    secureString.AppendChar(keyInfo.KeyChar);
                }
            } while (key != ConsoleKey.Enter);
            return secureString;
        }
    }
}
