using System.Net;

namespace M9Studio.ShadowTalk.Client
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ushort port;
            IPAddress address;

            if(!IPAddress.TryParse(textBoxIP.Text, out address))
            {
                //окно ошибки
                return;
            }
            if (!ushort.TryParse(textBoxPort.Text, out port))
            {
                //окно ошибки
                return;
            }
            //todo окно входа
        }
    }
}
