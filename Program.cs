using System.Net.NetworkInformation;
using System.Net;
using System;
using SimpleServer;
using System.Linq;
using Mono.Nat;

namespace HMStreamBackend
{
    static class Program
    {
        const int PORT = 2019;
        static INatDevice gatewayDevice;
        static IPAddress gatewayAddress = null;
        static void Main(string[] args)
        {
            NatUtility.DeviceFound += DeviceFoundAsync;
            Server.RegisterEndpoints();
            Server.Start(PORT);
            Server.onRequestReceived += LogMessage;
            Server.onServerStart += LogMessage;
            Server.onServerStop += LogMessage;
            Server.onEndpointRegistrationFinished += LogMessage;
            Server.onServerError += LogMessage;
            Console.CancelKeyPress += HandleQuit;

            IPAddress gateway = GetDefaultGateway();
            if (gateway != null)
            {
                gatewayAddress = gateway;
                NatUtility.StartDiscovery(new NatProtocol[] { NatProtocol.Upnp, NatProtocol.Pmp });
            }
        }

        private static void LogMessage(ServerEventData eventData)
        {
            string message = "";
            if (eventData.exception != null)
            {
                Console.WriteLine(eventData.exception.ToString());
            }
            if (eventData.path != null)
            {
                message += $"{eventData.path.ToString()}";
            }
            if (eventData.status != null)
            {
                message += $" {eventData.status.ToString()}";
            }
            if (eventData.message != null)
            {
                message += $" -- {eventData.message}";
            }
            Console.WriteLine(message);
        }

        private static IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }

        private static async void DeviceFoundAsync(object sender, DeviceEventArgs e)
        {
            if (gatewayAddress.ToString() == e.Device.DeviceEndpoint.Address.ToString())
            {
                NatUtility.StopDiscovery();
                Mapping mapping = new Mapping(Protocol.Tcp, PORT, PORT);
                var mappingResult = await e.Device.CreatePortMapAsync(mapping);
                System.Console.WriteLine($"Server mapped to {mappingResult.PrivatePort} on router port {mappingResult.PublicPort}");
                string hostName = Dns.GetHostName();
                string myIp = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                System.Console.WriteLine($"Your IP address: {myIp}");
                var externalIp = await e.Device.GetExternalIPAsync();
                System.Console.WriteLine($"You external IP address is {externalIp.ToString()}");
            }
        }

        private static async void HandleQuit(object sender, ConsoleCancelEventArgs args)
        {
            System.Console.WriteLine("quitting");
            if (gatewayDevice != null)
            {
                await gatewayDevice.DeletePortMapAsync(new Mapping(Protocol.Tcp, PORT, PORT));
                System.Console.WriteLine("Port mapping has been removed.");
            }
        }
    }
}
