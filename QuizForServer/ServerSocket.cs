using QuizForServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuizForServer
{
    public class ServerSocket
    {
        private IPAddress mIP;
        private int nPort;
        private TcpListener serverListener;
        private List<TcpClient> connectedClients = new List<TcpClient>();
        private Dictionary<string, List<string>> quizQuestions;
        private Dictionary<string, string> quizAnswers;
        private Dictionary<TcpClient, string> clientUsernames = new Dictionary<TcpClient, string>();

        private string currentQuizCode;

        public ServerSocket()
        {
            // Tạo bộ câu hỏi và câu trả lời mẫu
            quizQuestions = new Dictionary<string, List<string>>
            {
                { "Hệ Mặt trời có bao nhiêu hành tinh?", new List<string> { "8", "9", "7", "10" } },
                { "Ai là nữ hoàng của Anh?", new List<string> { "Elizabeth II", "Victoria", "Mary", "Anne" } },
                { "Loài động vật nào lớn nhất thế giới?", new List<string> { "Cá voi xanh", "Voi châu Phi", "Hải cẩu", "Gấu bắc cực" } },
                { "Đơn vị đo lường nào dùng để đo nhiệt độ?", new List<string> { "Celsius", "Meter", "Kilogram", "Volt" } },
                { "Quốc gia nào có diện tích lớn nhất thế giới?", new List<string> { "Nga", "Canada", "Mỹ", "Trung Quốc" } },
                { "Vật liệu nào là dẫn điện tốt nhất?", new List<string> { "Bạc", "Đồng", "Nhôm", "Sắt" } },
                { "Mặt trăng quay quanh hành tinh nào?", new List<string> { "Trái Đất", "Mặt Trời", "Sao Hỏa", "Sao Kim" } },
                { "Đâu là quốc gia có mật độ dân số cao nhất thế giới?", new List<string> { "Monaco", "Ấn Độ", "Trung Quốc", "Singapore" } },
                { "Tất cả các loại cây đều có gì?", new List<string> { "Chất diệp lục", "Đoạn thân", "Quả", "Hạt" } },
                { "Khoa học về sự sống được gọi là gì?", new List<string> { "Sinh học", "Hóa học", "Vật lý học", "Thiên văn học" } }
            };
            quizAnswers = new Dictionary<string, string>
            {
                { "Hệ Mặt trời có bao nhiêu hành tinh?", "8" },
                { "Ai là nữ hoàng của Anh?", "Elizabeth II" },
                { "Loài động vật nào lớn nhất thế giới?", "Cá voi xanh" },
                { "Đơn vị đo lường nào dùng để đo nhiệt độ?", "Celsius" },
                { "Quốc gia nào có diện tích lớn nhất thế giới?", "Nga" },
                { "Vật liệu nào là dẫn điện tốt nhất?", "Bạc" },
                { "Mặt trăng quay quanh hành tinh nào?", "Trái Đất" },
                { "Đâu là quốc gia có mật độ dân số cao nhất thế giới?", "Monaco" },
                { "Tất cả các loại cây đều có gì?", "Chất diệp lục" },
                { "Khoa học về sự sống được gọi là gì?", "Sinh học" }
            };
        }

        public void StartServer(string quizCode)
        {
            try
            {
                IPEndPoint ipaddr = null;
                int port = 0;
                mIP = IPAddress.Any;  // Or a specific IP
                nPort = 25000;  // Default port


                Debug.WriteLine($"IP address {mIP} - Port: {nPort}");
                Debug.WriteLine($"[Server] Starting server with quiz code: {quizCode}");

                currentQuizCode = quizCode;

                serverListener = new TcpListener(IPAddress.Any, 25000);
                Debug.WriteLine("[Server] TcpListener initialized, starting server...");

                serverListener.Start();
                Debug.WriteLine("[Server] Server started...");

                AcceptClients();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Server] Error in StartServer: {ex.Message}");
            }
        }

        private async void AcceptClients()
        {
            try
            {
                while (true)
                {
                    var client = await serverListener.AcceptTcpClientAsync();
                    connectedClients.Add(client);
                    Debug.WriteLine("[Server] New client connected.");
                    HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Server] Error in AcceptClients: {ex.Message}");
            }
        }

        private async void HandleClient(TcpClient client)
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
                        Debug.WriteLine("[Server] Empty message received, closing connection.");
                        break;
                    }

                    Debug.WriteLine($"[Server] Message received: {message}");

                    string[] parts = message.Split('|');
                    if (parts.Length < 2)
                    {
                        await SendMessage(client, "ERROR|Invalid message format");
                        Debug.WriteLine("[Server] Invalid message format received.");
                        continue;
                    }

                    if (parts[0] == "CODE")
                    {
                        string code = parts[1];
                        string username = parts[2];

                        // Lưu tên người dùng vào server và map với client
                        clientUsernames[client] = username;

                        if (code == currentQuizCode)
                        {
                            await SendMessage(client, "VALID_CODE");
                            Debug.WriteLine("[Server] Code validated.");

                            // Cập nhật danh sách người dùng trong Form1 với trạng thái 'Waiting...'
                            (Application.OpenForms[0] as Form1)?.UpdateClientList(username, -1);

                            await SendQuestionsToClient(client);
                        }
                        else
                        {
                            await SendMessage(client, "INVALID_CODE");
                            Debug.WriteLine("[Server] Invalid code received.");
                        }
                    }
                    else if (parts[0] == "ANSWER")
                    {
                        string username = parts[1];
                        string answers = parts[2];
                        int score = GradeQuiz(answers);

                        await SendMessage(client, $"SCORE|{score}");
                        Debug.WriteLine($"[Server] Scored {score} for {username}.");

                        (Application.OpenForms[0] as Form1)?.UpdateClientList(username, score);
                    }
                    else
                    {
                        await SendMessage(client, "ERROR|Unknown command");
                        Debug.WriteLine("[Server] Unknown command received.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Server] Exception: {ex.Message}");
            }
            finally
            {
                string username = clientUsernames.ContainsKey(client) ? clientUsernames[client] : "Unknown User";

                connectedClients.Remove(client);
                clientUsernames.Remove(client);

                (Application.OpenForms[0] as Form1)?.UpdateClientList(username, -1);

                client.Close();
                Debug.WriteLine("[Server] Client disconnected.");
            }
        }

        private async Task SendQuestionsToClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                foreach (var question in quizQuestions)
                {
                    string message = $"QUESTION|{question.Key}|{string.Join("|", question.Value)}";
                    await SendMessage(client, message);
                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Server] Error sending questions: {ex.Message}");
            }
        }

        private async Task SendMessage(TcpClient client, string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
                await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                Debug.WriteLine($"[Server] Sent message: {message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Server] Error sending message: {ex.Message}");
            }
        }

        private int GradeQuiz(string answers)
        {
            string[] clientAnswers = answers.Split(',');
            int score = 0;

            // Đảm bảo số lượng câu trả lời của người dùng không vượt quá số lượng câu hỏi
            if (clientAnswers.Length != quizAnswers.Count)
            {
                Debug.WriteLine("[Server] Invalid number of answers received.");
                return 0;  // Có thể xử lý theo cách khác, như gửi thông báo lỗi cho client.
            }

            // Duyệt qua các câu hỏi và so sánh câu trả lời của người dùng
            int index = 0;
            foreach (var question in quizAnswers)
            {
                string correctAnswer = question.Value;

                // Kiểm tra câu trả lời của người dùng có đúng không
                if (clientAnswers.Length > index && correctAnswer == clientAnswers[index])
                {
                    score++;
                }
                index++;
            }

            return score;
        }

    }
}
