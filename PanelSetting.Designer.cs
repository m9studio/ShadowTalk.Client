namespace M9Studio.ShadowTalk.Client
{
    partial class PanelSetting
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            button1 = new Button();
            textBoxPort = new TextBox();
            textBoxIP = new TextBox();
            textBoxName = new TextBox();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Location = new Point(4, 5);
            listView1.Margin = new Padding(4, 5, 4, 5);
            listView1.Name = "listView1";
            listView1.Size = new Size(430, 484);
            listView1.TabIndex = 8;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.KeyDown += listView1_KeyDown;

            // 
            // button1
            // 
            button1.Location = new Point(327, 545);
            button1.Margin = new Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new Size(107, 35);
            button1.TabIndex = 7;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBoxPort
            // 
            textBoxPort.Location = new Point(216, 545);
            textBoxPort.Margin = new Padding(4, 5, 4, 5);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.PlaceholderText = "Port";
            textBoxPort.Size = new Size(103, 23);
            textBoxPort.TabIndex = 6;
            textBoxPort.Text = "60606";
            // 
            // textBoxIP
            // 
            textBoxIP.Location = new Point(4, 545);
            textBoxIP.Margin = new Padding(4, 5, 4, 5);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.PlaceholderText = "IP/Domain";
            textBoxIP.Size = new Size(204, 23);
            textBoxIP.TabIndex = 5;
            textBoxIP.Text = "127.0.0.1";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(4, 499);
            textBoxName.Margin = new Padding(4, 5, 4, 5);
            textBoxName.Name = "textBoxName";
            textBoxName.PlaceholderText = "Название сервера";
            textBoxName.Size = new Size(430, 23);
            textBoxName.TabIndex = 9;
            textBoxName.Text = "Main";
            // 
            // PanelSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.White;
            Controls.Add(textBoxName);
            Controls.Add(listView1);
            Controls.Add(button1);
            Controls.Add(textBoxPort);
            Controls.Add(textBoxIP);
            Name = "PanelSetting";
            Size = new Size(438, 585);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView1;
        private Button button1;
        private TextBox textBoxPort;
        private TextBox textBoxIP;
        private TextBox textBoxName;
    }
}
