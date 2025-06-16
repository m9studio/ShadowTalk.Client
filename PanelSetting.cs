using System.Net.Sockets;

namespace M9Studio.ShadowTalk.Client
{
    public partial class PanelSetting : UserControl
    {
        FormMain form;
        public PanelSetting(FormMain form)
        {
            this.form = form;
            InitializeComponent();
            // Установим режим Details и скрытие заголовка
            listView1.View = View.Details;
            listView1.HeaderStyle = ColumnHeaderStyle.None;

            // Очищаем старые колонки и создаём одну новую на всю ширину
            listView1.Columns.Clear();
            listView1.Columns.Add("", listView1.ClientSize.Width - 4); // -4 для скролла

            // Обновляем ширину при изменении размера окна
            listView1.Resize += (s, e) =>
            {
                if (listView1.Columns.Count > 0)
                    listView1.Columns[0].Width = listView1.ClientSize.Width - 4;
            };

            // Добавляем элементы
            listView1.Items.Clear();

            foreach (var item in form.core.Servers)
            {
                int id = item.Key;
                ServerInfo server = item.Value;

                string text = $"{id}) {server.Name} [{server.ServerName} {server.IP}:{server.Port}]";
                ListViewItem lvi = new ListViewItem(text);
                lvi.Tag = server;
                lvi.ForeColor = item.Value.Session != null ? Color.Green : Color.Red;
                listView1.Items.Add(lvi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visible = false;
            var panel = new PanelLogin(textBoxName.Text, textBoxIP.Text, Convert.ToInt32(textBoxPort.Text), form, this);

            form.panelAir.Controls.Add(panel);
            // Центрируем элемент
            panel.Anchor = AnchorStyles.None;
            panel.Location = new Point(
                (form.panelAir.Width - panel.Width) / 2,
                (form.panelAir.Height - panel.Height) / 2
            );
            form.Text = "ShadowTalk Login";
        }


        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];

                if (selectedItem.Tag is ServerInfo server)
                {
                    // Вызов функции удаления
                    DeleteServer(server);

                    // Удаляем из ListView
                    listView1.Items.Remove(selectedItem);
                }
            }
        }


        private void DeleteServer(ServerInfo server)
        {
            if (form.core.Servers.ContainsKey(server.Id))
            {
                form.core.Servers.Remove(server.Id);
            }
            if(server.ChannelManager != null)
            {
                Socket socket = server.Socket;
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both); // отключение приёма и передачи
                    socket.Close();                       // освобождение ресурсов
                }
                server.Socket = null;
                server.ChannelManager = null;
                server.Session = null;
                //todo отключить все UDP сессии
                foreach (var item in server.Users)
                {
                    form.core.Users.Remove(item.Value);
                    form.panelUsers.Controls.Remove(item.Value.Panel);
                }
                form.core.DataBase.Send("DELETE FROM servers WHERE serverid = ?", server.ServerId);
                form.core.DataBase.Send("DELETE FROM users WHERE serverid = ?", server.ServerId);
                form.core.DataBase.Send("DELETE FROM messages WHERE serverid = ?", server.ServerId);

                if(form.userNow != null && form.userNow.ServerId == server.ServerId)
                {
                    form.ClearChat();
                }
            }
        }
    }
}
