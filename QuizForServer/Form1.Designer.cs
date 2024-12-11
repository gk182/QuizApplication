namespace QuizForServer
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
            LabelCode = new Label();
            listBoxClients = new ListBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(240, 9);
            label1.Name = "label1";
            label1.Size = new Size(272, 46);
            label1.TabIndex = 0;
            label1.Text = "Form for Server";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(284, 58);
            button1.Name = "button1";
            button1.Size = new Size(186, 35);
            button1.TabIndex = 1;
            button1.Text = "StartServer";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // LabelCode
            // 
            LabelCode.AutoSize = true;
            LabelCode.Location = new Point(343, 109);
            LabelCode.Name = "LabelCode";
            LabelCode.Size = new Size(78, 20);
            LabelCode.TabIndex = 3;
            LabelCode.Text = "Quiz Code";
            LabelCode.Click += LabelCode_Click;
            // 
            // listBoxClients
            // 
            listBoxClients.FormattingEnabled = true;
            listBoxClients.Location = new Point(167, 157);
            listBoxClients.Name = "listBoxClients";
            listBoxClients.Size = new Size(480, 184);
            listBoxClients.TabIndex = 4;
            listBoxClients.SelectedIndexChanged += listBoxClients_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listBoxClients);
            Controls.Add(LabelCode);
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
        private Label LabelCode;
        private ListBox listBoxClients;
    }
}
