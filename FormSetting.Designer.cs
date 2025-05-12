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
            textBoxIP.Location = new Point(12, 415);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.PlaceholderText = "IP/Domain";
            textBoxIP.Size = new Size(135, 23);
            textBoxIP.TabIndex = 1;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new Point(153, 415);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.PlaceholderText = "Port";
            textBoxPort.Size = new Size(73, 23);
            textBoxPort.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(232, 415);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(12, 12);
            listView1.Name = "listView1";
            listView1.Size = new Size(295, 397);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // FormSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(319, 450);
            Controls.Add(listView1);
            Controls.Add(button1);
            Controls.Add(textBoxPort);
            Controls.Add(textBoxIP);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSetting";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "FormSetting";
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