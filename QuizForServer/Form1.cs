using System.Net.Sockets;
using System.Collections.Generic;
using QuizForServer;

namespace QuizForServer
{
    public partial class Form1 : Form
    {
        private ServerSocket serverSocket;
        private string quizCode;
        private Dictionary<string, int> clientScores;
        public Form1()
        {
            InitializeComponent();
            clientScores = new Dictionary<string, int>();
            serverSocket = new ServerSocket();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            quizCode = random.Next(100000, 999999).ToString();
            LabelCode.Text = $"Quiz Code: {quizCode}";

            serverSocket.StartServer(quizCode);
        }
        public void UpdateClientList(string username, int score = -1)
        {
            Invoke((MethodInvoker)delegate
            {
                if (score == -1) // Khi client kết nối
                {
                    // Kiểm tra nếu tên người dùng đã tồn tại trong danh sách chưa
                    bool userExists = false;
                    foreach (var item in listBoxClients.Items)
                    {
                        if (item.ToString().StartsWith(username))
                        {
                            userExists = true;
                            break;
                        }
                    }

                    if (!userExists)
                    {
                        // Thêm tên người dùng vào danh sách và hiển thị "Waiting..."
                        listBoxClients.Items.Add($"{username} - Waiting...");
                    }
                }
                else // Khi client hoàn thành bài thi
                {
                    for (int i = 0; i < listBoxClients.Items.Count; i++)
                    {
                        if (listBoxClients.Items[i].ToString().StartsWith(username))
                        {
                            // Cập nhật điểm cho người dùng
                            listBoxClients.Items[i] = $"{username} - {score} points";
                            break;
                        }
                    }
                }
            });
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void LabelCode_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
