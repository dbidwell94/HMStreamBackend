using System.Text;
using System.Net;
using System;
using System.Net.Http;
using SimpleServer;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMStreamBackend;

namespace HMStreamBackend.BackendUtils
{
    public static class BackendManager
    {
        const string MAIN_BACKEND = "https://hmstreambackend.biddydev.com";

        private static string token = null;

        internal static async void ServerPing()
        {
            var client = new HttpClient();
            IPAddress externalIpAddress = null;
            while (true)
            {
                if (Program.gatewayDevice != null && token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    try
                    {
                        externalIpAddress = await Program.gatewayDevice.GetExternalIPAsync();
                        var result = await client.GetAsync($"{MAIN_BACKEND}/api");
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var resultString = await result.Content.ReadAsStringAsync();

                            var addressToSend = JsonConvert.SerializeObject(new Dictionary<string, object>()
                            {
                                { "address", $"{externalIpAddress.ToString()}:{Program.PORT}" }
                            });

                            var postResult = await client.PostAsync($"{MAIN_BACKEND}/api/addresses/address",
                                new StringContent(addressToSend, Encoding.UTF8, "application/json"));

                            if (postResult.StatusCode == HttpStatusCode.Created)
                            {
                                System.Console.WriteLine("Ip Address updated remotely");
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Program.LogMessage(new ServerEventData(null, null, null, e.Message));
                    }
                }
                Thread.Sleep(30000);
            }
        }

        internal static async void AuthWithServer()
        {
            while (true)
            {
                var command = Console.ReadLine();
                Console.Clear();
                if (command == "login")
                {
                    Console.Write("Enter Username: ");
                    var username = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    var password = Console.ReadLine();
                    await Login(username, password);
                }
            }
        }

        private static async Task Login(string username, string password)
        {
            Console.Clear();
            var client = new HttpClient();

            var jsonData = JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                {"username", username},
                {"password", password}
            });

            var response = await client.PostAsync($"{MAIN_BACKEND}/api/users/login", new StringContent(jsonData, Encoding.UTF8, "application/json"));
            System.Console.WriteLine(response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();

            var responseToken = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
            if (responseToken != null)
            {
                if (responseToken.TryGetValue("token", out object tokenObj))
                {
                    token = tokenObj.ToString();
                }
            }
        }
    }
}