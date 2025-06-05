namespace M9Studio.ShadowTalk.Client
{
    public partial class MoreMenuButton : UserControl
    {
        static Color ColorButton => ColorTranslator.FromHtml("#F0F0F0");
        static Color ColorButtonHover => ColorTranslator.FromHtml("#FFCC00");
        static Color ColorButtonClick => ColorTranslator.FromHtml("#FFA500");
        static Color ColorButtonText => ColorTranslator.FromHtml("#FF0000");

        public event Action? ButtonClick;

        public MoreMenuButton(Info info) : this(info.Icon, info.Text, info.click) { }
        public MoreMenuButton(string text, Bitmap icon, Action click) : this(icon, text, click) { }
        public MoreMenuButton(Bitmap icon, string text, Action click) : this(text, icon)
        {
            ButtonClick += click;
        }
        public MoreMenuButton(Bitmap icon, string text) : this(text, icon) { }
        public MoreMenuButton(string text, Bitmap icon)
        {
            InitializeComponent();

            pictureBox1.Image = icon;
            label1.Text = text;

            BackColor = ColorButton;
            ForeColor = ColorButtonText;
        }

        private void OnMouseClick(object sender, MouseEventArgs e) => ButtonClick?.Invoke();
        private void OnMouseLeave(object sender, EventArgs e) => BackColor = ColorButton;
        private void OnMouseEnter(object sender, EventArgs e) => BackColor = ColorButtonHover;
        private void OnMouseUp(object sender, MouseEventArgs e) => BackColor = ColorButtonHover;
        private void OnMouseDown(object sender, MouseEventArgs e) => BackColor = ColorButtonClick;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public struct Info
        {
            public string Text;
            public Bitmap Icon;
            public Action click;
        }
    }
}
