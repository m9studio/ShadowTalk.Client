namespace M9Studio.ShadowTalk.Client
{
    public partial class MoreMenu : UserControl
    {
        public MoreMenu(List<MoreMenuButton.Info> infos) : this(infos.ToArray()) { }
        public MoreMenu(MoreMenuButton.Info[] infos)
        {
            InitializeComponent();
            Size = new Size(200, infos.Length * 30);
            for (int i = 0; i < infos.Length; i++)
            {
                MoreMenuButton.Info item = infos[i];
                MoreMenuButton element = new MoreMenuButton(item);
                element.Location = new Point(0, i * 30);
                Controls.Add(element);
            }
        }
    }
}
