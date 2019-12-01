using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace usengecim
{
    public partial class mainform : Form
    {
        public mainform()
        {
            InitializeComponent();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            font = Font;
            textBox2.Text = File.ReadAllLines("data.base")[1];
            if(File.ReadAllLines("data.base")[0] == "1")
            {
                checkBox4.Checked = true;
            }
        }
        public static Font font;
        private string[] sago;
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Opacity = 0;
            }
            new Form1().Show();
        }
        private string resimden_base64_e(string resmin_konumu)
        {
            using (Image base64_e_cevirilecek_resim = Image.FromFile(resmin_konumu))
            {
                using (MemoryStream hafiza_akisi = new MemoryStream())
                {
                    base64_e_cevirilecek_resim.Save(hafiza_akisi, base64_e_cevirilecek_resim.RawFormat);
                    byte[] resmimizin_byte_dizisi = hafiza_akisi.ToArray();
                    string resimin_base64_degeri = Convert.ToBase64String(resmimizin_byte_dizisi);
                    return resimin_base64_degeri;
                }
            }
        }
        public void yukeme_islemi_bitti(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                byte[] sitenin_yaniti = e.Result;
                string durum = Encoding.UTF8.GetString(sitenin_yaniti);
                richTextBox1.Text = "[IMG]" + durum + "[/IMG]";
                if (File.ReadAllLines("data.base")[0] == "1")
                {
                    Clipboard.SetText(richTextBox1.Text);
                    MessageBox.Show("Resim URL'si [IMG] tag içerisinde panoya kopyalandı.", "Bilgi", 0, (MessageBoxIcon)64);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", 0, (MessageBoxIcon)48); }
        }
        private void surec(object sender, UploadProgressChangedEventArgs e)
        {
            try
            {
                toolStripProgressBar1.Value = e.ProgressPercentage;
                toolStripStatusLabel1.Text = "%" + e.ProgressPercentage.ToString();
            }
            catch (Exception) { }

        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Visible = (checkBox3.Checked) ? true : false; // Ternary if
            button3.Visible = (checkBox3.Checked) ? true : false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           FontDialog fntshow = new FontDialog();
           if(fntshow.ShowDialog() == DialogResult.OK)
            {
                font = fntshow.Font;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists("tht_image.jpeg"))
            {
                try
                {
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel1.Text = "%0";
                    using (WebClient yukleyici = new WebClient())
                    {
                        yukleyici.UploadValuesCompleted += new UploadValuesCompletedEventHandler(yukeme_islemi_bitti);
                        yukleyici.UploadProgressChanged += new UploadProgressChangedEventHandler(surec);
                        yukleyici.Headers.Add("User-Agent: Other"); //403 yasak hatası çözümü için.

                        NameValueCollection parametreler = new NameValueCollection();
                        parametreler["key"] = "407289b6f603c950af54fbc79311b9b0"; //imgyukle.com sitesinin API için default key'i. Üyelik gerektirmiyor.
                        parametreler["source"] = resimden_base64_e("tht_image.jpeg");
                        parametreler["format"] = "txt"; //Direkt olarak URL için txt formatında aldım, isterseniz json yazarak detaylı bilgilere erişebilirsiniz.

                        yukleyici.UploadValuesAsync(new Uri("https://imgyukle.com/api/1/upload"), "POST", parametreler); //Asenkron olarak upload işlemi.
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", 0, (MessageBoxIcon)48); }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog op = new OpenFileDialog())
            {
                op.Title = "Bir resim editörü programı seçiniz.";
                op.Filter = "Program (*.exe)|*.exe";
                if(op.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = op.FileName;
                    sago = File.ReadAllLines("data.base");
                    sago[1] = textBox2.Text;
                    File.WriteAllLines("data.base", sago);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = "mspaint.exe";
            sago = File.ReadAllLines("data.base");
            sago[1] =textBox2.Text;
            File.WriteAllLines("data.base", sago);
        }
        
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                sago = File.ReadAllLines("data.base");
                sago[0] = "1";
                File.WriteAllLines("data.base", sago);
            }
            else
            {
                sago = File.ReadAllLines("data.base");
                sago[0] = "0";
                File.WriteAllLines("data.base", sago);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                sago = File.ReadAllLines("data.base");
                sago[1] = textBox2.Text;
                File.WriteAllLines("data.base", sago);
            }
        }
    }
}
