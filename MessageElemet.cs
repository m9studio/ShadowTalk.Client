using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M9Studio.ShadowTalk.Client
{
    public partial class MessageElemet : UserControl
    {
        public MessageElemet()
        {
            InitializeComponent();
            panelDelete.BackColor = AppColors.Button;
            panelEdit.BackColor = AppColors.Button;
            label1.ForeColor = AppColors.ButtonText;
            label2.ForeColor = AppColors.ButtonText;
        }

        private void panelDelete_MouseClick(object sender, MouseEventArgs e)
        {

        }


        private void panelEdit_Click(object sender, EventArgs e)
        {

        }

        private void panelMore_MouseLeave(object sender, EventArgs e)
        {
            //panelMore.Visible = false;
        }

        private void panelDelete_MouseLeave(object sender, EventArgs e) => panelDelete.BackColor = AppColors.Button;
        private void panelDelete_MouseEnter(object sender, EventArgs e) => panelDelete.BackColor = AppColors.ButtonHover;
        private void panelDelete_MouseUp(object sender, MouseEventArgs e) => panelDelete.BackColor = AppColors.ButtonHover;
        private void panelDelete_MouseDown(object sender, MouseEventArgs e) => panelDelete.BackColor = AppColors.ButtonClick;



        private void panelEdit_MouseLeave(object sender, EventArgs e) => panelEdit.BackColor = AppColors.Button;
        private void panelEdit_MouseEnter(object sender, EventArgs e) => panelEdit.BackColor = AppColors.ButtonHover;
        private void panelEdit_MouseUp(object sender, MouseEventArgs e) => panelEdit.BackColor = AppColors.ButtonHover;
        private void panelEdit_MouseDown(object sender, MouseEventArgs e) => panelEdit.BackColor = AppColors.ButtonClick;

    }
}
