using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;

namespace M9Studio.ShadowTalk.Client
{
    public partial class PanelUser : UserControl
    {
        public static FormMain form;
        public User User;
        public PanelUser(User user)
        {
            InitializeComponent();
            User = user;

            labelName.Text = User.Name;
            labelServer.Text = User.Server.ServerName;
            labelCount.Text = User.NewCount.ToString();

            this.Click += clik;
            labelName.Click += clik;
            labelServer.Click += clik;
            labelCount.Click += clik;

        }
        private void clik(object sender, EventArgs e)
        {
            form.OpenChat(User);
        }
    }
}