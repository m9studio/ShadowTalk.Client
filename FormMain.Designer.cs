namespace M9Studio.ShadowTalk.Client
{
    partial class FormMain
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
            splitContainer1 = new SplitContainer();
            panelUsers = new FlowLayoutPanel();
            panel1 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            textBox1 = new TextBox();
            panelChat = new FlowLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            labelName = new Label();
            labelServer = new Label();
            panel3 = new Panel();
            messageText = new RichTextBox();
            buttonSend = new Button();
            panelAir = new Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(4, 5, 4, 5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panelUsers);
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelChat);
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel2.Controls.Add(panel3);
            splitContainer1.Panel2MinSize = 300;
            splitContainer1.Size = new Size(784, 561);
            splitContainer1.SplitterDistance = 335;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 0;
            // 
            // panelUsers
            // 
            panelUsers.AutoScroll = true;
            panelUsers.BackColor = SystemColors.ActiveBorder;
            panelUsers.Dock = DockStyle.Fill;
            panelUsers.Location = new Point(0, 50);
            panelUsers.Name = "panelUsers";
            panelUsers.Size = new Size(335, 511);
            panelUsers.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(textBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(335, 50);
            panel1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.search;
            pictureBox2.Location = new Point(296, 10);
            pictureBox2.Margin = new Padding(0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 30);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.setting;
            pictureBox1.Location = new Point(9, 9);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(30, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(50, 10);
            textBox1.Margin = new Padding(0);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Поиск";
            textBox1.Size = new Size(235, 30);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // panelChat
            // 
            panelChat.BackColor = SystemColors.ControlDarkDark;
            panelChat.Dock = DockStyle.Fill;
            panelChat.Location = new Point(0, 50);
            panelChat.Name = "panelChat";
            panelChat.Size = new Size(443, 436);
            panelChat.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(labelName);
            flowLayoutPanel1.Controls.Add(labelServer);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.RightToLeft = RightToLeft.No;
            flowLayoutPanel1.Size = new Size(443, 50);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(10, 10);
            labelName.Margin = new Padding(10, 10, 3, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(102, 23);
            labelName.TabIndex = 0;
            labelName.Text = "label name";
            // 
            // labelServer
            // 
            labelServer.AutoSize = true;
            labelServer.Location = new Point(125, 10);
            labelServer.Margin = new Padding(10, 10, 3, 0);
            labelServer.Name = "labelServer";
            labelServer.Size = new Size(106, 23);
            labelServer.TabIndex = 1;
            labelServer.Text = "label server";
            // 
            // panel3
            // 
            panel3.Controls.Add(messageText);
            panel3.Controls.Add(buttonSend);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 486);
            panel3.Name = "panel3";
            panel3.Size = new Size(443, 75);
            panel3.TabIndex = 1;
            // 
            // messageText
            // 
            messageText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            messageText.Enabled = false;
            messageText.Location = new Point(3, 6);
            messageText.Name = "messageText";
            messageText.Size = new Size(312, 66);
            messageText.TabIndex = 1;
            messageText.Text = "";
            // 
            // buttonSend
            // 
            buttonSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSend.Enabled = false;
            buttonSend.Location = new Point(321, 3);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(119, 32);
            buttonSend.TabIndex = 0;
            buttonSend.Text = "Отправить";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // panelAir
            // 
            panelAir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelAir.BackColor = Color.FromArgb(68, 68, 68, 68);
            panelAir.Location = new Point(0, 0);
            panelAir.Name = "panelAir";
            panelAir.Size = new Size(784, 561);
            panelAir.TabIndex = 0;
            panelAir.Visible = false;
            panelAir.Click += panelAir_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(panelAir);
            Controls.Add(splitContainer1);
            Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(600, 400);
            Name = "FormMain";
            Text = "ShadowTalk";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panel1;
        private TextBox textBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        public Panel panelAir;
        public FlowLayoutPanel panelUsers;
        private Panel panel3;
        private FlowLayoutPanel panelChat;
        private FlowLayoutPanel flowLayoutPanel1;
        public Label labelName;
        public Label labelServer;
        private Button buttonSend;
        private RichTextBox messageText;
    }
}
