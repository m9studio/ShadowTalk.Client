namespace M9Studio.ShadowTalk.Client
{
    public partial class FormMain : Form
    {
        public Core core = new Core();
        public User userNow;//тот с кем открыт чат

        public FormMain()
        {
            InitializeComponent();
            core.form = this;
            labelName.Text = "";
            labelServer.Text = "";
        }


        public bool panelAirClose = true;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panelAir.Visible = true;
            PanelSetting panelSetting = new PanelSetting(this);
            panelAir.Controls.Add(panelSetting);
            // Центрируем элемент
            panelSetting.Anchor = AnchorStyles.None;
            panelSetting.Location = new Point(
                (panelAir.Width - panelSetting.Width) / 2,
                (panelAir.Height - panelSetting.Height) / 2
            );
            Text = "ShadowTalk Setting";
        }
        public void panelAir_Click(object sender, EventArgs e)
        {
            if (panelAirClose)
            {
                panelAir.Visible = false;
                panelAir.Controls.Clear();
                Text = "ShadowTalk";
            }
        }

        bool Search = false;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!Search)
            {
                Search = true;
                textBox1.Enabled = false;
                panelUsers.Controls.Clear();
                if (!core.SearchUser(textBox1.Text)){
                    Search = false;
                    textBox1.Enabled = true;
                    textBox1_TextChanged(null, null);
                }
            }
        }

        public void SearchUserCallBack(List<User> users)
        {
            this.Invoke(() =>
            {
                Search = false;
                textBox1.Enabled = true;
                foreach (var item in users)
                {
                    panelUsers.Controls.Add(item.Panel);
                }
            });
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            panelUsers.Controls.Clear();
            string currentText = textBox1.Text.Trim();
            core.Users.FindAll(u => u.Name.Contains(currentText));
        }





        private void buttonSend_Click(object sender, EventArgs e)
        {
            string text = messageText.Text;
            messageText.Text = "";
            core.SendMessage(userNow, text);
        }
        public void OpenChat(User user)
        {
            if(userNow != null)
            {
                userNow.Form = null;
            }
            userNow = user;
            userNow.Form = this;

            user.NewCount = 0;
            panelChat.Controls.Clear();

            labelName.Text = user.Panel.labelName.Text;
            labelName.ForeColor = user.Panel.labelName.ForeColor;

            labelServer.Text = user.Panel.labelServer.Text;
            labelServer.ForeColor = user.Panel.labelServer.ForeColor;

            messageText.Enabled = true;
            buttonSend.Enabled = true;
        }
    
        public void ClearChat()
        {
            messageText.Enabled = false;
            buttonSend.Enabled = false;
            panelChat.Controls.Clear();
            userNow.Form = null;
            userNow = null;

            messageText.Text = "";
            labelName.Text = "";
            labelServer.Text = "";
        }
    
    
    
    }
}
