using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace GiocoDellaVita
{
    public partial class Form1 : Form
    {
        Bitmap erba = new Bitmap(GiocoDellaVita.Properties.Resources.weed);
        int easter = 0;
        Volpe[] volpecavia;
        Coniglio[] ConiglioCavia;
        Carota[] Pioevsan;
        bool[,] usato = new bool[9, 9];
        public Form1()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            easter++;
            if (easter > 4)
            {
                label1.Text = "Gioco della Morte";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            volpecavia = new Volpe[(int)VolpiUpDown.Value];
            ConiglioCavia = new Coniglio[(int)ConigliUpDown.Value];
            Pioevsan = new Carota[(int)CaroteUpDown.Value];

            for (int a = 0; a < volpecavia.Length; a++)
            {
                volpecavia[a] = new Volpe();
            }
            for (int a = 0; a < Pioevsan.Length; a++)
            {
                Pioevsan[a] = new Carota();
            }
            for (int a = 0; a < ConiglioCavia.Length; a++)
            {
                ConiglioCavia[a] = new Coniglio();
            }
            /*volpecavia[0] = new Volpe();
            ConiglioCavia[0] = new Coniglio();
            ConiglioCavia[0].x = 6;
            ConiglioCavia[0].y = 4;
            ConiglioCavia[1] = new Coniglio();
            ConiglioCavia[1].x = 1;
            ConiglioCavia[1].y = 8;
            Pioevsan[0] = new Carota();
            Pioevsan[0].x = 5;
            Pioevsan[0].y = 2;*/

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[8];
            rng.GetBytes(data);
            int seed = BitConverter.ToInt32(data, 0);
            Random R = new Random(seed);
            foreach (Volpe vp in volpecavia)
            {
                vp.x = R.Next(0, 9);
                vp.y = R.Next(0, 9);
                while (usato[vp.x, vp.y] == true)
                {
                    vp.x = R.Next(0, 9);
                    vp.y = R.Next(0, 9);
                }
                usato[vp.x, vp.y] = true;
                vp.energia = 10;
            }
            foreach (Coniglio cc in ConiglioCavia)
            {
                cc.x = R.Next(0, 9);
                cc.y = R.Next(0, 9);
                while (usato[cc.x, cc.y] == true)
                {
                    cc.x = R.Next(0, 9);
                    cc.y = R.Next(0, 9);
                }
                usato[cc.x, cc.y] = true;
                cc.energia = 15;
            }
            foreach (Carota ca in Pioevsan)
            {
                ca.x = R.Next(0, 9);
                ca.y = R.Next(0, 9);
                while (usato[ca.x, ca.y] == true)
                {
                    ca.x = R.Next(0, 9);
                    ca.y = R.Next(0, 9);
                }
                usato[ca.x, ca.y] = true;
                ca.energia = 8;
            }
            // AAAA 
            GameEngine.RunWorkerAsync();
        }

        //Qui si fa tutto
        private void GameEngine_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker fermo = sender as BackgroundWorker;
            while (!fermo.CancellationPending)
            {
                if (fermo.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                CleanCampo();
                //movimenti
                foreach (Carota ca in Pioevsan)
                {
                    string coord = ca.Muovi(ca.x, ca.y);
                    Draw(coord, ca.img);
                }
                foreach (Coniglio cc in ConiglioCavia)
                {
                    string coord = cc.Percorso(Pioevsan);
                    //se non ci sono carote scappo dalle volpi
                    if (coord == "NOCA")
                    {
                        //rifai scappa
                        coord = cc.Scappa(volpecavia);
                    }
                    Draw(coord, cc.img);
                }
                foreach (Volpe vp in volpecavia)
                {
                    string coord = vp.Percorso(ConiglioCavia);
                    Draw(coord, vp.img);
                }
                try
                {
                    Console.WriteLine(volpecavia[0].energia.ToString());
                }
                catch
                {
                    Console.WriteLine("Volpe morta, unlucky");
                }
                FineTurno();
                Thread.Sleep(2000);
            }
        }
        //ferma il backgroudworker
        private void button2_Click(object sender, EventArgs e)
        {
            GameEngine.CancelAsync();
        }

        //disegna il personaggio dove indicato
        private void Draw(string dove,Bitmap Immagine)
        {
            this.Invoke(new MethodInvoker(delegate {
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    if (c.Name == dove)
                    {
                        ((PictureBox)c).Image = Immagine;
                    }
                }
            }));
        }

        //pulisce il campo
        private void CleanCampo()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    foreach (Control c in tableLayoutPanel1.Controls)
                    {
                        ((PictureBox)c).Image = erba;
                    }
                }));
            }
            catch
            {
                Console.WriteLine("Hai chiuso il programma prima che potessi pulire il campo :c");
            }
        }

        //FINE TURNO todo: metti in classe
        private void FineTurno()
        {
            int contac = 0;
            int contav = 0;
            //il foreach per le volpi
            foreach(Volpe vv in volpecavia)
            {
                if (vv.energia <= 0)
                {
                    vv.vivo = false;
                }
                if(vv.vivo == false)
                {
                    var tmp = new List<Volpe>(volpecavia);
                    tmp.RemoveAt(contav);
                    volpecavia = tmp.ToArray();
                }
                //il foreach dei conigli DENTRO LE VOLPI (o come fare male il codice)
                foreach (Coniglio cc in ConiglioCavia)
                {
                    if (volpecavia[0].x == cc.x && volpecavia[0].y == cc.y)
                    {
                        volpecavia[0].energia += 15;
                        cc.vivo = false;
                    }
                    if (cc.energia <= 0)
                    {
                        cc.vivo = false;
                    }
                    if (cc.vivo == false)
                    {
                        var tmp = new List<Coniglio>(ConiglioCavia);
                        tmp.RemoveAt(contac);
                        ConiglioCavia = tmp.ToArray();
                    }
                    
                    contac++;
                }
                contav++;
            }
            int contaa = 0;
            foreach (Carota ca in Pioevsan)
            {
                if (ca.energia <= 0)
                {
                    ca.Marcio = true;
                    ca.img = GiocoDellaVita.Properties.Resources.CarotaFalza;
                }
                foreach (Coniglio cc in ConiglioCavia)
                {
                    if (cc.x == ca.x && cc.y == ca.y)
                    {
                        if (ca.Marcio)
                        {
                            cc.energia -= 5;
                        }
                        else
                        {
                            cc.energia += 10;
                        }
                        ca.vivo = false;
                    }
                }
                if (ca.vivo == false)
                {
                    var tmp = new List<Carota>(Pioevsan);
                    tmp.RemoveAt(contaa);
                    Pioevsan = tmp.ToArray();
                }
                contaa++;
            }
            if (ConiglioCavia.Length == 0 && volpecavia.Length == 0 && Pioevsan.Length == 0)
            {
                MessageBox.Show("Gioco finito!", "FINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Invoke((MethodInvoker)delegate
                {
                    Start.Visible = false;
                    button2.Visible = false;
                    CaroteUpDown.Visible = false;
                    label2.Visible = false;
                });  

                GameEngine.CancelAsync();
            }
        }

        private void CaroteUpDown_ValueChanged(object sender, EventArgs e)
        {

        }


    }
}
