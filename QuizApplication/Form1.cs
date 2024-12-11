using System;
using System.Windows.Forms;
using System.Windows;


namespace QuizApplication
{
    public partial class Form1 : Form
    {
        private ClientSocket clientSocket;
        public Form1()
        {
            InitializeComponent();
            clientSocket = new ClientSocket();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string ipAddress = "127.0.0.1";
            int port = 25000;
            string code = TextCode.Text;
            string username = TextUserName.Text;
            bool isValid = await clientSocket.ConnectToServerAsync(username, code);
            if (isValid)
            {
                await Task.Delay(1000);
                var quizQuestions = clientSocket.GetQuizQuestions();

                var formQuiz = new QuizForm(clientSocket, quizQuestions);
                formQuiz.Show();

                // Hide the current form (Form1)
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Code!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
