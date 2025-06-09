namespace M9Studio.ShadowTalk.Client
{
    public partial class PanelUser : UserControl
    {
        public User User;
        public PanelUser(User user)
        {
            InitializeComponent();
            User = user;

            labelName.Text = User.Name;
            labelServer.Text = User.Server.ServerName;
            labelCount.Text = User.NewCount.ToString();
        }
    }
}