namespace M9Studio.ShadowTalk.Client
{
    partial class MessageElemet
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            panelMore = new Panel();
            panelEdit = new Panel();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            panelDelete = new Panel();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panelMore.SuspendLayout();
            panelEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panelDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelMore
            // 
            panelMore.BackColor = Color.Transparent;
            panelMore.Controls.Add(panelEdit);
            panelMore.Controls.Add(panelDelete);
            panelMore.Location = new Point(10, 10);
            panelMore.Margin = new Padding(0);
            panelMore.Name = "panelMore";
            panelMore.Size = new Size(200, 80);
            panelMore.TabIndex = 0;
            panelMore.MouseLeave += panelMore_MouseLeave;
            // 
            // panelEdit
            // 
            panelEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelEdit.BackColor = Color.Honeydew;
            panelEdit.Controls.Add(label2);
            panelEdit.Controls.Add(pictureBox2);
            panelEdit.Cursor = Cursors.Hand;
            panelEdit.Location = new Point(0, 40);
            panelEdit.Margin = new Padding(0);
            panelEdit.Name = "panelEdit";
            panelEdit.Size = new Size(200, 40);
            panelEdit.TabIndex = 2;
            panelEdit.Click += panelEdit_Click;
            panelEdit.MouseDown += panelEdit_MouseDown;
            panelEdit.MouseEnter += panelEdit_MouseEnter;
            panelEdit.MouseLeave += panelEdit_MouseLeave;
            panelEdit.MouseUp += panelEdit_MouseUp;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.ForeColor = Color.LightSeaGreen;
            label2.Location = new Point(55, 5);
            label2.Name = "label2";
            label2.Size = new Size(108, 30);
            label2.TabIndex = 2;
            label2.Text = "Изменить";
            label2.MouseDown += panelEdit_MouseDown;
            label2.MouseEnter += panelEdit_MouseEnter;
            label2.MouseLeave += panelEdit_MouseLeave;
            label2.MouseUp += panelEdit_MouseUp;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.edit;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(40, 40);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.MouseDown += panelEdit_MouseDown;
            pictureBox2.MouseEnter += panelEdit_MouseEnter;
            pictureBox2.MouseLeave += panelEdit_MouseLeave;
            pictureBox2.MouseUp += panelEdit_MouseUp;
            // 
            // panelDelete
            // 
            panelDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelDelete.BackColor = Color.Honeydew;
            panelDelete.Controls.Add(label1);
            panelDelete.Controls.Add(pictureBox1);
            panelDelete.Cursor = Cursors.Hand;
            panelDelete.Location = new Point(0, 0);
            panelDelete.Margin = new Padding(0);
            panelDelete.Name = "panelDelete";
            panelDelete.Size = new Size(200, 40);
            panelDelete.TabIndex = 1;
            panelDelete.MouseClick += panelDelete_MouseClick;
            panelDelete.MouseDown += panelDelete_MouseDown;
            panelDelete.MouseEnter += panelDelete_MouseEnter;
            panelDelete.MouseLeave += panelDelete_MouseLeave;
            panelDelete.MouseUp += panelDelete_MouseUp;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.ForeColor = Color.LightSeaGreen;
            label1.Location = new Point(55, 5);
            label1.Name = "label1";
            label1.Size = new Size(90, 30);
            label1.TabIndex = 1;
            label1.Text = "Удалить";
            label1.MouseDown += panelDelete_MouseDown;
            label1.MouseEnter += panelDelete_MouseEnter;
            label1.MouseLeave += panelDelete_MouseLeave;
            label1.MouseUp += panelDelete_MouseUp;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.trash;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += panelDelete_MouseDown;
            pictureBox1.MouseEnter += panelDelete_MouseEnter;
            pictureBox1.MouseLeave += panelDelete_MouseLeave;
            pictureBox1.MouseUp += panelDelete_MouseUp;
            // 
            // MessageElemet
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(panelMore);
            Name = "MessageElemet";
            Size = new Size(302, 150);
            panelMore.ResumeLayout(false);
            panelEdit.ResumeLayout(false);
            panelEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panelDelete.ResumeLayout(false);
            panelDelete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMore;
        private PictureBox pictureBox1;
        private Panel panelDelete;
        private Panel panelEdit;
        private PictureBox pictureBox2;
        private Label label2;
        private Label label1;
    }
}
