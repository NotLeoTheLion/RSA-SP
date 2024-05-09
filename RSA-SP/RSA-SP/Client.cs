using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RSA_SP
{
    internal class Client
    {
        public static void SendMessage(string message)
        {
            try
            {
                using (var client = new TcpClient("localhost", 8080))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepavyko issiusti: {ex.Message}");
            }
        }
    }
}
