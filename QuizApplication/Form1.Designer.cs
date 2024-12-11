namespace QuizApplication
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button1 = new Button();
            TextUserName = new TextBox();
            label2 = new Label();
            label3 = new Label();
            TextCode = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(244, 91);
            label1.Name = "label1";
            label1.Size = new Size(288, 38);
            label1.TabIndex = 0;
            label1.Text = "Quiz Form For Client";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(300, 273);
            button1.Name = "button1";
            button1.Size = new Size(116, 30);
            button1.TabIndex = 1;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // TextUserName
            // 
            TextUserName.Location = new Point(300, 171);
            TextUserName.Name = "TextUserName";
            TextUserName.Size = new Size(266, 27);
            TextUserName.TabIndex = 2;
            TextUserName.TextChanged += TextUserName_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(176, 178);
            label2.Name = "label2";
            label2.Size = new Size(121, 20);
            label2.TabIndex = 3;
            label2.Text = "Nhập UserName:";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(172, 232);
            label3.Name = "label3";
            label3.Size = new Size(125, 20);
            label3.TabIndex = 4;
            label3.Text = "Nhập mã câu hỏi:";
            // 
            // TextCode
            // 
            TextCode.Location = new Point(300, 225);
            TextCode.Name = "TextCode";
            TextCode.Size = new Size(266, 27);
            TextCode.TabIndex = 5;
            TextCode.TextChanged += textBox2_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TextCode);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(TextUserName);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private TextBox TextUserName;
        private Label label2;
        private Label label3;
        private TextBox TextCode;
    }
}
