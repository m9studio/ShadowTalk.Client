using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M9Studio.ShadowTalk.Client
{
    public partial class FormCore : Form
    {
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

        private const int resizeArea = 10;


        public FormCore()
        {
            InitializeComponent();


        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
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
