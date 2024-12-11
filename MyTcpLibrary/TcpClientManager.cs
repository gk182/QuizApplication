using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpLibrary
{
    public class TcpClientManager
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public event Action<string> OnMessageReceived;

        public async Task ConnectToServer(string ipAddress, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ipAddress, port);
            _stream = _client.GetStream();
            Console.WriteLine("Connected to server.");

            // Start receiving messages
            _ = ReceiveMessages();
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                OnMessageReceived?.Invoke(message);
            }
        }

        public async Task SendMessage(string message)
        {
            var buffer = Encoding.ASCII.GetBytes(message);
            await _stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public void Close()
        {
            _client?.Close();
        }
    }
}
