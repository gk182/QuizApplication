using QuizApplication;

public class QuizForm : Form
{
    ClientSocket clientSocket;
    private Dictionary<string, List<string>> quizQuestions;
    private int currentQuestionIndex;
    private Label questionLabel;
    private List<RadioButton> radioButtons;
    private Button submitButton;
    private int score;
    private Label scoreLabel;

    private List<string> userAnswers; // Danh sách để lưu câu trả lời của người dùng

    public QuizForm(ClientSocket clientSocket, Dictionary<string, List<string>> questions)
    {
        this.clientSocket = clientSocket;
        quizQuestions = questions;
        currentQuestionIndex = 0;
        score = 0;

        userAnswers = new List<string>(); // Khởi tạo danh sách câu trả lời của người dùng

        // Cài đặt kích thước form
        this.Text = "Quiz Application";
        this.Size = new System.Drawing.Size(400, 350);

        // Tạo Label hiển thị câu hỏi
        questionLabel = new Label
        {
            Text = "",
            Location = new System.Drawing.Point(20, 20),
            AutoSize = true
        };
        this.Controls.Add(questionLabel);

        // Danh sách RadioButton cho đáp án
        radioButtons = new List<RadioButton>();

        // Nút submit
        submitButton = new Button
        {
            Text = "Submit",
            Location = new System.Drawing.Point(20, 200),
        };
        submitButton.Click += SubmitButton_Click;
        this.Controls.Add(submitButton);

        // Hiển thị câu hỏi đầu tiên
        ShowQuestion();
        // Khởi tạo các thành phần giao diện
        scoreLabel = new Label
        {
            Text = "Score: 0",
            Location = new System.Drawing.Point(20, 250),
            AutoSize = true
        };
        this.Controls.Add(scoreLabel);

    }

    private void ShowQuestion()
    {
        // Kiểm tra lại index trước khi truy cập câu hỏi
        if (quizQuestions.Count == 0 || currentQuestionIndex < 0 || currentQuestionIndex >= quizQuestions.Count)
        {
            MessageBox.Show("No questions available or invalid index.");
            return;
        }

        // Xóa các RadioButton cũ nếu có
        foreach (var rb in radioButtons)
        {
            this.Controls.Remove(rb);
        }
        radioButtons.Clear();

        // Lấy câu hỏi và đáp án từ quizQuestionsList
        var currentQuestion = quizQuestions.ElementAt(currentQuestionIndex);

        // Hiển thị câu hỏi
        questionLabel.Text = currentQuestion.Key;

        int yOffset = 60;
        foreach (var answer in currentQuestion.Value)
        {
            RadioButton rb = new RadioButton
            {
                Text = answer,
                Location = new System.Drawing.Point(20, yOffset),
                AutoSize = true
            };
            radioButtons.Add(rb);
            this.Controls.Add(rb);
            yOffset += 30;
        }

        // Thêm nút "Tiếp theo" nếu là câu hỏi cuối
        if (currentQuestionIndex == quizQuestions.Count - 1)
        {
            submitButton.Text = "Finish";
        }
        else
        {
            submitButton.Text = "Next";
        }
    }

    private string GetSelectedAnswer()
    {
        // Lấy câu trả lời đã chọn
        foreach (var rb in radioButtons)
        {
            if (rb.Checked)
            {
                return rb.Text;
            }
        }
        return string.Empty; // Trả về chuỗi rỗng nếu không có gì được chọn
    }

    private async void SubmitButton_Click(object sender, EventArgs e)
    {
        // Lấy câu trả lời của người dùng cho câu hỏi hiện tại
        string answer = GetSelectedAnswer();
        if (!string.IsNullOrEmpty(answer))
        {
            userAnswers.Add(answer); // Lưu câu trả lời của người dùng
        }

        // Nếu câu hỏi cuối, hiển thị kết quả
        if (currentQuestionIndex == quizQuestions.Count - 1)
        {
            string finalAnswers = string.Join(",", userAnswers); // Tạo chuỗi kết quả từ danh sách câu trả lời
            clientSocket.SendAnswerToServer(finalAnswers);
            await Task.Delay(500);
            MessageBox.Show($"Your score: {score}"); // Hiển thị chuỗi kết quả
        }

        // Tiến tới câu hỏi tiếp theo
        currentQuestionIndex++;
        ShowQuestion();
    }
    public async void UpdateScore(int newScore)
    {
        score = newScore;
        scoreLabel.Text = $"Score: {score}";
    }

}
