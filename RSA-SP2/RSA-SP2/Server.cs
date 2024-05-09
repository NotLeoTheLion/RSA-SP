using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SP2
{
    internal static class Server
    {
        private static TcpListener listener;
        public static event Action<string> MessageReceived;

        public static void StartServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server started.");
            Task.Run(() => AcceptClients());
        }

        private static void AcceptClients()
        {
            while (true)
            {
                try
                {
                    using (var client = listener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        var message = reader.ReadLine();
                        Console.WriteLine("Received data: " + message);
                        MessageReceived?.Invoke(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accepting clients: " + ex.Message);
                    break;
                }
            }
        }
    }
}
