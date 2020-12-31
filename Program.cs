using System;
using SimpleServer;
using System.Threading;

namespace HMStreamBackend
{
    static class Program
    {
        static void Main(string[] args)
        {
            Server.RegisterEndpoints();
            Server.Start(2019);
            Server.onRequestReceived += LogMessage;
            Server.onServerStart += LogMessage;
            Server.onServerStop += LogMessage;
            Server.onEndpointRegistrationFinished += LogMessage;
            Server.onServerError += LogMessage;
        }

        private static void LogMessage(ServerEventData eventData)
        {
            string message = "";
            if (eventData.exception != null)
            {
                Console.WriteLine(eventData.exception.StackTrace);
                Console.WriteLine(eventData.exception.Message);
                Console.WriteLine(eventData.exception.TargetSite);
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
    }
}
