using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {

        private Label yilanUcu;
        private int YilanParcasiArasiMesafe=2;
        private int yilanParcasiSayisi;
        private int yilanBoyutu=20;
        private int yemBoyutu = 20;
        private Label yem;
        private Random random;
        private HareketYonu yon;



        public Form1()
        {
            InitializeComponent();
            random = new Random();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Sifirla();



        }

        private void YenidenBaslat()
        {
            lblPuan.Text = "0";
            lblSure.Text = "0";

            Sifirla();


        }

        public void Sifirla()
        {
            panel1.Controls.Clear();
            yilanParcasiSayisi = 0;
            YemOlustur();
            yeminYeriniDegistir();
            yilaniYerlestir();
            timerYilanHareket.Enabled = true;
            timerSure.Enabled = true;

        }

        private Label YilanParcasiOlustur(int locationX, int locationY)
        {
            yilanParcasiSayisi++;
            Label lbl = new Label()
            {
                Name = "yilanParca" + yilanParcasiSayisi,
                BackColor = Color.Red,
                Width = yilanBoyutu,
                Height = yilanBoyutu,
                Location = new Point(locationX, locationY)

            };
            this.panel1.Controls.Add(lbl);
            return lbl;

        }

        private void yilaniYerlestir()
        {
            yilanUcu = YilanParcasiOlustur(0, 0);
            yilanUcu.Text = ":";
            yilanUcu.TextAlign = ContentAlignment.MiddleCenter;
            yilanUcu.ForeColor = Color.White;
            var locationX = (panel1.Width / 2) - (yilanUcu.Width / 2);
            var locationY = (panel1.Height / 2) - (yilanUcu.Height / 2);
            yilanUcu.Location = new Point(locationX, locationY);
        }

        private void YemOlustur()
        {
            Label lbl = new Label()
            {
                Name = "yem",
                BackColor = Color.White,
                Width = yemBoyutu,
                Height = yemBoyutu
                

            };
            yem = lbl;
            this.panel1.Controls.Add(lbl);
            
        }

        private void yeminYeriniDegistir()
        {
            var locationX = 0;
            var locationY = 0;

            bool durum;

            do
            {
                durum = false;
                locationX = random.Next(0, panel1.Width - yemBoyutu);
                locationY = random.Next(0, panel1.Height - yemBoyutu);
                var rect1 = new Rectangle(new Point(locationX, locationY), yem.Size);


                foreach (Control control in panel1.Controls)
                {
                    if (control is Label && control.Name.Contains("yilanParca"))
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);

                        if (rect1.IntersectsWith(rect2))
                        {
                            durum = true;
                            break;


                        }
                    }
                }

            } while (durum);

            yem.Location = new Point(locationX, locationY);

        }


        private enum HareketYonu
        {
            Yukari,
            Asagi,
            Sola,
            Saga
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var keyCode = e.KeyCode;

            if (yon==HareketYonu.Sola && keyCode==Keys.D
                || yon == HareketYonu.Saga && keyCode == Keys.A
                || yon == HareketYonu.Yukari && keyCode == Keys.S
                || yon == HareketYonu.Asagi && keyCode == Keys.W)
            {
                return;

            }

            switch (keyCode)
            {
                case Keys.W:
                    yon = HareketYonu.Yukari;
                    break;
                case Keys.S:
                    yon = HareketYonu.Asagi;
                    break;
                case Keys.A:
                    yon = HareketYonu.Sola;
                    break;
                case Keys.D:
                    yon = HareketYonu.Saga;
                    break;
                case Keys.P:
                    timerSure.Enabled = false;
                    timerYilanHareket.Enabled = false;
                    break;
                case Keys.C:
                    timerSure.Enabled = true;
                    timerYilanHareket.Enabled = true;
                    break;

                default:
                    break;
            }

        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            YilanUcunuTakipEt();
            yilaniYurut();


            oyunBittimi();
            yilanYemiYedimi();
            
        }

        private void yilaniYurut()
        {
            var locationX = yilanUcu.Location.X;
            var locationY = yilanUcu.Location.Y;


            switch (yon)
            {
                case HareketYonu.Yukari:
                    yilanUcu.Location = new Point(locationX, locationY - (yilanUcu.Width + YilanParcasiArasiMesafe));
                    break;
                case HareketYonu.Asagi:
                    yilanUcu.Location = new Point(locationX, locationY + (yilanUcu.Width + YilanParcasiArasiMesafe));
                    break;
                case HareketYonu.Sola:
                    yilanUcu.Location = new Point(locationX - (yilanUcu.Width + YilanParcasiArasiMesafe), locationY);
                    break;
                case HareketYonu.Saga:
                    yilanUcu.Location = new Point(locationX + (yilanUcu.Width + YilanParcasiArasiMesafe), locationY);
                    break;
                default:
                    break;
            }
        }

        
        private void oyunBittimi()
        {
            bool oyunBittimi = false;

            var rect1 = new Rectangle(yilanUcu.Location, yilanUcu.Size);

            foreach (Control control in panel1.Controls)
            {
                if (control is Label && control.Name.Contains("yilanParca")  && control.Name != yilanUcu.Name)
                {
                    var rect2 = new Rectangle(control.Location, control.Size);
                    if (rect1.IntersectsWith(rect2))
                    {
                        oyunBittimi = true;
                        break;

                    }
                }
            }

            if (oyunBittimi)
            {
                timerYilanHareket.Enabled = false;
                timerSure.Enabled = false;
                DialogResult sonuc =MessageBox.Show("Puanınız: "+lblPuan.Text+" | Yeniden Oynamak için Tamama tıkla.", "GAME OVER!",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);

                if (sonuc== DialogResult.OK)
                {
                    YenidenBaslat();
                }

                
            }
        }

        private void yilanYemiYedimi()
        {
            var rect1 = new Rectangle(yilanUcu.Location, yilanUcu.Size);
            var rect2 = new Rectangle(yem.Location, yem.Size);

            if (rect1.IntersectsWith(rect2))
            {
                lblPuan.Text = (Convert.ToInt32(lblPuan.Text)+10).ToString();
                yeminYeriniDegistir();
                YilanParcasiOlustur(-  yilanBoyutu, - yilanBoyutu  );


            }
        }

        private void YilanUcunuTakipEt()
        {
            if (yilanParcasiSayisi <= 1) return;

            
            for (int i = yilanParcasiSayisi; i > 1; i--)
            {
                var sonrakiParca = (Label)panel1.Controls[i];
                var oncekiParca = (Label)panel1.Controls[i-1];

                sonrakiParca.Location = oncekiParca.Location;

            }
        }

        private void timerSure_Tick(object sender, EventArgs e)
        {
            lblSure.Text = (Convert.ToInt32(lblSure.Text)+1).ToString();
        }
    }
}
