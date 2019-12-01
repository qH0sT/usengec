using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace usengecim
{
    public partial class Form1 : Form
    {
        private Point XYBaslangic;
        private Point XYBitis;
        private bool MouseAssagidami;
        private Rectangle recancil;
        public Form1()
        {
            InitializeComponent();
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                pictureBox1.Image = bmp;
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (recancil != null)
            {
                try
                {
                    recancil = new Rectangle(
                        new Point(Math.Min(XYBaslangic.X, XYBitis.X), Math.Min(XYBaslangic.Y, XYBitis.Y)),
                        new Size(Math.Abs(XYBaslangic.X - XYBitis.X), Math.Abs(
                       XYBaslangic.Y - XYBitis.Y)));
                    e.Graphics.DrawRectangle(Pens.Red, recancil);
                }
                catch (Exception) { }

            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseAssagidami)
            {
                XYBitis = e.Location;
                MouseAssagidami = false;

                try
                {
                    Bitmap bmp = new Bitmap(recancil.Width - 1, recancil.Height - 1, PixelFormat.Format32bppArgb);
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(recancil.Left + 1, recancil.Top + 1, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                    if (((mainform)Application.OpenForms["mainform"]).checkBox3.Checked)
                    {
                        g.DrawString(((mainform)Application.OpenForms["mainform"]).textBox1.Text, 
                        mainform.font, Brushes.Red, 0f, 0f);
                    }
                    bmp.Save("tht_image.jpeg", ImageFormat.Jpeg);
                    if (((mainform)Application.OpenForms["mainform"]).checkBox1.Checked)
                    {
                        try
                        {
                            Process.Start(File.ReadAllText("editor.base"), "tht_image.jpeg");
                        }
                        catch (Exception) { }
                    }
                    //pictureBox1.Image = bmp;
                }
                catch (Exception) { }
                
                Close();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseAssagidami = true;
            XYBaslangic = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseAssagidami)
            {
                XYBitis = e.Location;
            
                pictureBox1.Refresh();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((mainform)Application.OpenForms["mainform"]).Opacity = 100;
        }
    }
}
