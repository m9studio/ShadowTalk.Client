namespace M9Studio.ShadowTalk.Client
{
    public partial class MessageItem : UserControl
    {
        ContextMenuStrip contextMenu = new ContextMenuStrip();
        public Message msg;

        ToolTip tooltip = new ToolTip();
        public MessageItem(Message msg)
        {
            this.msg = msg;
            int n = 250;
            InitializeComponent();
            /*label1.AutoSize = false; // чтобы можно было задать фиксированную ширину
            label1.MaximumSize = new Size(n, 0); // n — нужная ширина в пикселях
            label1.AutoEllipsis = false; // если не хотите троеточие
            label1.TextAlign = ContentAlignment.TopLeft; // или другое выравнивание*/
            label1.Text = msg.Text;


            this.ContextMenuStrip = contextMenu;

            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(msg.Date).LocalDateTime;
            label2.Text = dateTime.ToString("dd.MM.yyyy\nHH:mm:ss");

            label2.Font = new Font(label2.Font.FontFamily, 8);

            // Настраиваем при необходимости
            tooltip.AutoPopDelay = 5000;     // сколько показывать (мс)
            tooltip.InitialDelay = 500;      // задержка перед показом (мс)
            tooltip.ReshowDelay = 200;       // задержка при повторном наведении
            tooltip.ShowAlways = true;       // показывать даже если окно не в фокусе

            update();
        }

        public void update()
        {
            contextMenu.Items.Clear();
            if (msg.Status == -2 || msg.Status == -1)
            {
                contextMenu.Items.Add("Повторить отправку", null, (s, e) => {
                    var m = PanelUser.form.core.SendMessage(PanelUser.form.userNow, msg.Text, msg.UUID);
                    msg.Status = m.Status;
                    update();
                });
                contextMenu.Items.Add("Удалить", null, (s, e) => {
                    PanelUser.form.core.DataBase.Send("DELETE FROM messages WHERE uuid = ?", msg.UUID);
                    PanelUser.form.messages.Remove(msg);
                    PanelUser.form.ItemsChat.Remove(msg.UUID);
                    PanelUser.form.panelChat.Controls.Remove(this);
                });

                tooltip.SetToolTip(label1, "Сообщение не доставлено");
                label2.ForeColor = Color.Red;
                return;
            }
            if(msg.Status == 0)
            {
                tooltip.SetToolTip(label1, "Сообщение ожидает получения");
                contextMenu.Items.Add("Удалить", null, (s, e) => {
                    PanelUser.form.core.DataBase.Send("DELETE FROM messages WHERE uuid = ?", msg.UUID);
                    PanelUser.form.messages.Remove(msg);
                    PanelUser.form.ItemsChat.Remove(msg.UUID);
                    PanelUser.form.panelChat.Controls.Remove(this);
                });
                label2.ForeColor = Color.Yellow;
                return;
            }
            //status == 1
            tooltip.SetToolTip(label1, "Сообщение доставлено");
            contextMenu.Items.Add("Удалить", null, (s, e) => {
                PanelUser.form.core.DataBase.Send("DELETE FROM messages WHERE uuid = ?", msg.UUID);
                PanelUser.form.messages.Remove(msg);
                PanelUser.form.ItemsChat.Remove(msg.UUID);
                PanelUser.form.panelChat.Controls.Remove(this);
            });
            label2.ForeColor = Color.Green;

        }

        public void SetRight()
        {
            label1.TextAlign = ContentAlignment.TopRight;
        }

    }
}
