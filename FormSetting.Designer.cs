namespace M9Studio.ShadowTalk.Client
{
    partial class FormSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxIP = new TextBox();
            textBoxPort = new TextBox();
            button1 = new Button();
            listView1 = new ListView();
            SuspendLayout();
            // 
            // textBoxIP
            // 
            textBoxIP.Location = new Point(13, 513);
            textBoxIP.Margin = new Padding(4, 5, 4, 5);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.PlaceholderText = "IP/Domain";
            textBoxIP.Size = new Size(204, 30);
            textBoxIP.TabIndex = 1;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new Point(225, 511);
            textBoxPort.Margin = new Padding(4, 5, 4, 5);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.PlaceholderText = "Port";
            textBoxPort.Size = new Size(103, 30);
            textBoxPort.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(336, 508);
            button1.Margin = new Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new Size(107, 35);
            button1.TabIndex = 3;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(13, 14);
            listView1.Margin = new Padding(4, 5, 4, 5);
            listView1.Name = "listView1";
            listView1.Size = new Size(430, 484);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // FormSetting
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(456, 557);
            Controls.Add(listView1);
            Controls.Add(button1);
            Controls.Add(textBoxPort);
            Controls.Add(textBoxIP);
            Font = new Font("Tahoma", 14.25F);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSetting";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Настройки";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBoxIP;
        private TextBox textBoxPort;
        private Button button1;
        private ListView listView1;
    }
}