// Client.cs
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuizClient
{
    public class Client
    {
        private string username;
        private string code;

        public Client(string username, string code)
        {
            this.username = username;
            this.code = code;
        }

        public async Task StartAsync()
        {
            using var client = new TcpClient();
            try
            {
                await client.ConnectAsync("127.0.0.1", 25000);
                Console.WriteLine("[Client] Connected to server.");

                var stream = client.GetStream();
                await SendMessageAsync(stream, $"CODE|{code}|{username}");

                var buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"[Client] Response: {response}");

                if (response == "VALID_CODE")
                {
                    while (true)
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string questionMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                        if (questionMessage.StartsWith("QUESTION"))
                        {
                            var parts = questionMessage.Split('|');
                            string question = parts[1];
                            string[] options = parts[2..];

                            Console.WriteLine($"Question: {question}");
                            for (int i = 0; i < options.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {options[i]}");
                            }

                            Console.Write("Your answer: ");
                            string answer = Console.ReadLine();
                            await SendMessageAsync(stream, $"ANSWER|{answer}");
                        }
                        else if (questionMessage.StartsWith("SCORE"))
                        {
                            string score = questionMessage.Split('|')[1];
                            Console.WriteLine($"Your score: {score}");
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid code or username.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client] Error: {ex.Message}");
            }
        }

        private async Task SendMessageAsync(NetworkStream stream, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task Main(string[] args)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter code: ");
            string code = Console.ReadLine();

            Client client = new Client(username, code);
            await client.StartAsync();
        }
    }
}
