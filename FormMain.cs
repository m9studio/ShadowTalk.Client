using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;

namespace M9Studio.ShadowTalk.Client
{
    public partial class FormMain : Form
    {
        public Core core = new Core();
        public User userNow;//тот с кем открыт чат
        public List<Message> messages;

        public FormMain()
        {
            InitializeComponent();
            core.form = this;
            labelName.Text = "";
            labelServer.Text = "";

            panelChat.FlowDirection = FlowDirection.TopDown;
            panelChat.WrapContents = false;

            PanelUser.form = this;

            Shown += (o, e) =>
            {
                textBox1_TextChanged(null, null);
            };
        }


        public bool panelAirClose = true;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Invoke(() =>
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
            });
        }
        public void panelAir_Click(object sender, EventArgs e)
        {
            Invoke(() =>
            {
                if (panelAirClose)
                {
                    panelAir.Visible = false;
                    panelAir.Controls.Clear();
                    Text = "ShadowTalk";
                }
            });
        }

        bool Search = false;
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            Invoke(() =>
            {
                if (!Search)
                {
                    Search = true;
                    textBox1.Enabled = false;
                    panelUsers.Controls.Clear();
                    if (!core.SearchUser(textBox1.Text))
                    {
                        Search = false;
                        textBox1.Enabled = true;
                        textBox1_TextChanged(null, null);
                    }
                }
            });
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
            Invoke(() =>
            {
                panelUsers.Controls.Clear();
                string currentText = textBox1.Text.Trim();
                core.Users.FindAll(u => u.Name.Contains(currentText)).ForEach(u => panelUsers.Controls.Add(u.Panel));
            });
        }





        private void buttonSend_Click(object sender, EventArgs e)
        {
            Invoke(() =>
            {
                string text = messageText.Text;
                messageText.Text = "";
                Message msg = core.SendMessage(userNow, text);
                if (ItemsChat.ContainsKey(msg.UUID))
                {
                    ItemsChat[msg.UUID].msg.Status = msg.Status;
                    ItemsChat[msg.UUID].Update();
                }
                else
                {
                    AddMessage(msg);
                }
            });
        }






        public Dictionary<string, MessageItem> ItemsChat;
        public void OpenChat(User user)
        {
            Invoke(() =>
            {
                if (userNow != null)
                {
                    userNow.Form = null;
                }
                //обновляем данные!!!!
                userNow = core.OnLoadUser(user.Server, user.Id);
                if ((userNow == null))
                {
                    return;
                }

                ItemsChat = new Dictionary<string, MessageItem>();

                userNow.Form = this;

                user.NewCount = 0;
                panelChat.Controls.Clear();

                labelName.Text = user.Panel.labelName.Text;
                labelName.ForeColor = user.Panel.labelName.ForeColor;

                labelServer.Text = user.Panel.labelServer.Text;
                labelServer.ForeColor = user.Panel.labelServer.ForeColor;

                messageText.Enabled = true;
                buttonSend.Enabled = true;

                messages = core.LoadChat(user.ServerId, user.Id);

                foreach (var message in messages)
                {
                    AddMessage(message);
                }
            });
        }
        public void AddMessage(Message message)
        {

            Invoke(() =>
            {
                MessageItem item = new MessageItem(message);
                //item.MaximumSize = new Size(panelChat.Width - 40, 0); // адаптивная ширина
                // Центрируем вручную с помощью Padding
                if (message.Sender == userNow.Id)
                {
                    // Прижать вправо — добавляем пустое пространство слева
                    /*int leftPadding = panelChat.Width / 2;
                    item.Padding = new Padding(leftPadding, 0, 0, 0);*/
                    item.SetRight();
                }
                else
                {
                    // Прижать влево — добавляем пустое пространство справа
                    /*int rightPadding = panelChat.Width / 2;
                    item.Padding = new Padding(0, 0, rightPadding, 0);*/
                }
                ItemsChat.Add(message.UUID, item);
                panelChat.Controls.Add(item);
                panelChat.ScrollControlIntoView(panelChat.Controls[panelChat.Controls.Count - 1]);



                // Допустим, у вас есть элемент, например button1
                int x = item.Location.X;
                int y = item.Location.Y;

                // Сохраняем позицию и изменяем ширину
                int newWidth = panelChat.Width - 40; // новая ширина
                item.SetBounds(x, y, newWidth, item.Height);
            });

        }
        public void UpdateMessage(string uuid)
        {
            Invoke(() =>
            {
                if (ItemsChat.ContainsKey(uuid))
                {
                    ItemsChat[uuid].update();
                }
            });
        }



        public void ClearChat()
        {
            Invoke(() =>
            {
                messageText.Enabled = false;
                buttonSend.Enabled = false;
                panelChat.Controls.Clear();
                userNow.Form = null;
                userNow = null;

                messageText.Text = "";
                labelName.Text = "";
                labelServer.Text = "";
            });
        }



        private void panelChat_Resize(object sender, EventArgs e)
        {
            Invoke(() =>
            {
                foreach (Control ctrl in panelChat.Controls)
                {
                    if (ctrl is MessageItem item)
                    {
                        // Допустим, у вас есть элемент, например button1
                        int x = item.Location.X;
                        int y = item.Location.Y;

                        // Сохраняем позицию и изменяем ширину
                        int newWidth = panelChat.Width - 40; // новая ширина
                        item.SetBounds(x, y, newWidth, item.Height);
                    }
                }
            });
        }

    }
}
