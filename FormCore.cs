
using System.Runtime.InteropServices;

namespace M9Studio.ShadowTalk.Client
{
    public partial class FormCore : Form
    {
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_BORDER_COLOR = 34;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 1;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HTCAPTION = 0x2;

        private const int resizeArea = 10;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        public FormCore()
        {
            InitializeComponent();
            EnableRoundedCorners();

        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (Environment.OSVersion.Version.Build >= 22000) // Windows 11
            {
                int preference = 2; // Round corners
                DwmSetWindowAttribute(this.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref preference, sizeof(int));

                int backdropType = 2; // Mica / Default
                DwmSetWindowAttribute(this.Handle, DWMWA_SYSTEMBACKDROP_TYPE, ref backdropType, sizeof(int));
            }
        }



        private void pictureBox1_Click(object sender, EventArgs e) => this.Close();
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                pictureBox2.Image = Properties.Resources.maximize;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                pictureBox2.Image = Properties.Resources.normalize;
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        public enum DwmWindowCornerPreference
        {
            Default = 0,
            DoNotRound = 1,
            Round = 2,
            RoundSmall = 3
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref DwmWindowCornerPreference pvAttribute, int cbAttribute);

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style |= 0x00040000; // WS_SIZEBOX (aka WS_THICKFRAME) — позволяет ресайз
                cp.Style |= 0x00020000; // WS_MINIMIZEBOX
                cp.Style |= 0x00010000; // WS_MAXIMIZEBOX
                return cp;
            }
        }
        public void EnableRoundedCorners()
        {
            if (Environment.OSVersion.Version.Build >= 22000) // Windows 11
            {
                var attribute = DwmWindowCornerPreference.Round;
                DwmSetWindowAttribute(this.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref attribute, sizeof(uint));
            }
        }


        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m); // ВАЖНО: вызываем первым
                Point cursor = this.PointToClient(Cursor.Position);
                int x = cursor.X;
                int y = cursor.Y;
                int w = this.ClientSize.Width;
                int h = this.ClientSize.Height;

                if (x <= resizeArea && y <= resizeArea)
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (x >= w - resizeArea && y <= resizeArea)
                    m.Result = (IntPtr)HTTOPRIGHT;
                else if (x <= resizeArea && y >= h - resizeArea)
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (x >= w - resizeArea && y >= h - resizeArea)
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (x <= resizeArea)
                    m.Result = (IntPtr)HTLEFT;
                else if (x >= w - resizeArea)
                    m.Result = (IntPtr)HTRIGHT;
                else if (y <= resizeArea)
                    m.Result = (IntPtr)HTTOP;
                else if (y >= h - resizeArea)
                    m.Result = (IntPtr)HTBOTTOM;
                // иначе: не трогаем результат от base.WndProc
                return;
            }

            base.WndProc(ref m);
        }
    }
}
