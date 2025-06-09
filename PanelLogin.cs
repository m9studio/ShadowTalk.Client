namespace M9Studio.ShadowTalk.Client
{
    public partial class PanelLogin : UserControl
    {
        FormMain form;
        PanelSetting setting;
        string name;
        string ip;
        int port;

        public PanelLogin(string name, string ip, int port, FormMain form, PanelSetting setting)
        {
            this.form = form;
            this.setting = setting;
            this.name = name;
            this.ip = ip;
            this.port = port;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.panelAirClose = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;

            ServerInfo server = new ServerInfo();
            server.ServerName = name;
            server.Port = port;
            server.IP = ip;
            server.ServerId = form.core.Servers.Any() ? form.core.Servers.Keys.Max() + 1 : 1;
            if(form.core.NewConnect(server, textBox1.Text, textBox2.Text))
            {
                MessageBox.Show("Вы успешно вошли", "ShadowTalk Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка входа", "ShadowTalk Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            form.panelAirClose = true;
            form.panelAir_Click(null, null);
        }
    }
}
