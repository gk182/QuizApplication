using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QuizApplication
{
    public class ClientSocket
    {
        private TcpClient client;
        private NetworkStream stream;
        private string username_temp;
        private Dictionary<string, List<string>> quizQuestions = new Dictionary<string, List<string>>();


        public async Task<bool> ConnectToServerAsync(string username, string code)
        {
            try
            {
                string serverIP = "127.0.0.1";
                int serverPort = 25000;

                client = new TcpClient();
                client.Connect(serverIP, serverPort);
                Debug.WriteLine($"[Client] Connected to server at {serverIP}:{serverPort}.");

                stream = client.GetStream();

                SendCodeAndUsername(code, username);

                string response = await ReceiveMessage();
                if (response == "VALID_CODE")
                {

                    username_temp = username;
                    HandleServerMessages(client);
                }
                return response == "VALID_CODE";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Client] Exception: {ex.Message}");
                return false;
            }
        }
        public async Task SendMessage(string message)
        {
            try
            {
                if (stream != null)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    Debug.WriteLine($"[Client] Sent message: {message}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Client] Error sending message: {ex.Message}");
            }
        }
        private void SendCodeAndUsername(string code, string username)
        {
            string message = $"CODE|{code}|{username}";
            SendMessage(message);
        }
        public async Task<string> ReceiveMessage()
        {
            try
            {
                if (stream != null)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        Debug.WriteLine("[Client] Server has closed the connection.");
                        return null;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    Debug.WriteLine($"[Client] Received message: {message}");
                    return message;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Client] Error receiving message: {ex.Message}");
            }
            return null;
        }

        
        private async void HandleServerMessages(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Client đóng kết nối

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    if (string.IsNullOrEmpty(message))
                    {
                        Debug.WriteLine("[Client] Empty message received.");
                        break;
                    }

                    string[] parts = message.Split('|');
                    if (parts.Length < 2)
                    {
                        Debug.WriteLine("[Client] Invalid message format.");
                        continue;
                    }

                    switch (parts[0])
                    {
                        case "QUESTION":
                            HandleQuestionAsync(parts);
                            break;
                        case "SCORE":
                            HandleScore(parts);
                            break;
                        default:
                            Debug.WriteLine("[Client] Unknown message received.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Client] Error receiving message: {ex.Message}");
            }
        }
        private async Task HandleQuestionAsync(string[] parts)
        {
            if (parts.Length < 5)
            {
                Debug.WriteLine("[Error] Invalid question data. Expected at least 5 parts.");
                return;
            }
            string question = parts[1];
            string[] answers = { parts[2], parts[3], parts[4], parts[5] };
            quizQuestions[question] = new List<string> { answers[0], answers[1], answers[2], answers[3] };

            // DEBUGGGGG
            Debug.WriteLine($"[Client] Received question: {question}");
            for (int i = 0; i < answers.Length; i++)
            {
                Debug.WriteLine($"{i + 1}. {answers[i]}");
            }

        }
        private async Task  HandleScore(string[] parts)
        {
            int score = int.Parse(parts[1]);
            foreach (Form form in Application.OpenForms)
            {
                if (form is QuizForm quizForm)
                {
                    // Cập nhật điểm số cho QuizForm
                    quizForm.UpdateScore(score);
                    break;
                }
            }
            Debug.WriteLine($"Your score: {score}");
        }
        public async Task SendAnswerToServer( string answers)
        {
            try
            {
                string message = $"ANSWER|{username_temp}|{answers}";
                await SendMessage(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Client] Error sending answer: {ex.Message}");
            }
        }

        private void DisplayScore(int score)
        {
            Console.WriteLine($"Your score: {score}");
        }
        public  Dictionary<string, List<string>> GetQuizQuestions()
        {
            return quizQuestions;
        }
        public void CloseConnection()
        {
            try
            {
                stream?.Close();
                client?.Close();
                Debug.WriteLine("[Client] Connection closed.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Client] Error closing connection: {ex.Message}");
            }
        }
    }
}
