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
    /// <summary>
    /// GIOCO DELLA VITA DI GABRIELE DI NUOVO
    /// Bug Noti:
    /// -I personaggi POSSONO sovrapporsi dando origine a un bug in caso un predatore provi a mangiarli
    /// -I conigli scappano in modo un po' stupido dalle volpi
    /// -Certe eccezzioni sono gestite tramite MessageBox, ma è intenzionale dato che chi gioca deve capire cosa è successo e perchè il gioco si è riavviato
    /// </summary>


    public partial class Gioco : Form
    {
        int conigliuccisi = 0;
        int turno = 0;
        int carotemangiate = 0;
        //variabili varie
        Bitmap erba = new Bitmap(GiocoDellaVita.Properties.Resources.weed);
        int easter = 0;
        Volpe[] volpecavia;
        Coniglio[] ConiglioCavia;
        Carota[] Carote;
        bool[,] usato = new bool[9, 9];
        public Gioco()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            easter++;
            if (easter > 4)
            {
                Titolo.Text = "Gioco della Morte";
            }
        }

        //Pulsante che inizializza il gioco
        private void button1_Click(object sender, EventArgs e)
        {
            turno = 0;
            Start.Enabled = false;
            Stop.Enabled = true;
            VolpiUpDown.Enabled = false;
            CaroteUpDown.Enabled = false;
            conigliuccisi = 0;
            carotemangiate = 0;
            ConigliUpDown.Enabled = false;
            //crea volpi, conigli e carote in base al numero impostato
            volpecavia = new Volpe[(int)VolpiUpDown.Value];
            ConiglioCavia = new Coniglio[(int)ConigliUpDown.Value];
            Carote = new Carota[(int)CaroteUpDown.Value];

            for (int a = 0; a < volpecavia.Length; a++)
            {
                volpecavia[a] = new Volpe();
            }
            for (int a = 0; a < Carote.Length; a++)
            {
                Carote[a] = new Carota();
            }
            for (int a = 0; a < ConiglioCavia.Length; a++)
            {
                ConiglioCavia[a] = new Coniglio();
            }

            //Crea un seed davvero random.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[8];
            rng.GetBytes(data);
            int seed = BitConverter.ToInt32(data, 0);
            Random R = new Random(seed);
            //Posiziona ogni volpe, coniglio e carota nel campo in posizioni casuali non ripetitive e assegna dell'energia a ogni personaggio
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
                //Energia
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
                //Energia
                cc.energia = 15;
            }
            foreach (Carota ca in Carote)
            {
                ca.x = R.Next(0, 9);
                ca.y = R.Next(0, 9);
                while (usato[ca.x, ca.y] == true)
                {
                    ca.x = R.Next(0, 9);
                    ca.y = R.Next(0, 9);
                }
                usato[ca.x, ca.y] = true;
                //Energia
                ca.energia = 8;
            }
            // Inizia il gioco
            try
            {
                GameEngine.RunWorkerAsync();
            }
            catch
            {
                turno = 0;
                Start.Enabled = true;
                Stop.Enabled = false;
                VolpiUpDown.Enabled = true;
                CaroteUpDown.Enabled = true;
                ConigliUpDown.Enabled = true;
                MessageBox.Show("Per favore attendi un secondo...", "Attendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        //Qui si fa tutto
        private void GameEngine_DoWork(object sender, DoWorkEventArgs e)
        {
            //se viene premuto il pulsante si ferma tutto. E' UNO STOP
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
                    foreach (Carota ca in Carote)
                    {
                        string coord = ca.Muovi(ca.x, ca.y);
                        Draw(coord, ca.img);
                    }
                    foreach (Coniglio cc in ConiglioCavia)
                    {
                        string coord = cc.Percorso(Carote);
                        //se non ci sono carote scappo dalle volpi
                        if (coord == "NOCA")
                        {
                            //La fuga funziona un po male. Se il coniglio si trova in un angolo è morto, se la volpe non muore prima di fame.
                            coord = cc.Scappa(volpecavia);
                        }
                        Draw(coord, cc.img);
                    }
                    foreach (Volpe vp in volpecavia)
                    {
                        string coord = vp.Percorso(ConiglioCavia);
                        Draw(coord, vp.img);
                    }
                    FineTurno();
                    //Attesa di un secondo tra ogni turno
                    Thread.Sleep(1000);
                }       
        }
        //ferma il backgroudworker
        private void button2_Click(object sender, EventArgs e)
        {
            Stop.Enabled = false;
            Start.Enabled = true;
            CaroteUpDown.Enabled = true;
            ConigliUpDown.Enabled = true;
            VolpiUpDown.Enabled = true;
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

        //FINE TURNO
        private void FineTurno()
        {
            int contac = 0;
            int contav = 0;
            //il foreach per le volpi, controlla che siano vive altrimenti le rimuove dal gioco
            foreach(Volpe vv in volpecavia)
            {
                if (vv.energia <= 0)
                {
                    vv.vivo = false;
                }
                if(vv.vivo == false)
                {
                    //elimina la volpe dall'array.
                    var tmp = new List<Volpe>(volpecavia);
                    tmp.RemoveAt(contav);
                    volpecavia = tmp.ToArray();
                }
                //controlla che i conigli siano vivi. Se una volpe è nella stessa posizione di un coniglio il coniglio muore e la volpe guadagna 15 di energia
                foreach (Coniglio cc in ConiglioCavia)
                {
                        if (volpecavia[0].x == cc.x && volpecavia[0].y == cc.y)
                        {
                            volpecavia[0].energia += 15;
                            conigliuccisi++;
                            cc.vivo = false;
                        }
                        //se l'energia è 0 (o meno di 0?) segna il coniglio come morto
                        if (cc.energia <= 0)
                        {
                            cc.vivo = false;
                        }
                        if (cc.vivo == false)
                        {
                            var tmp = new List<Coniglio>(ConiglioCavia);
                            tmp.RemoveAt(contac);
                            contac--;
                            ConiglioCavia = tmp.ToArray();
                        }

                        contac++;
                }
                contav++;
            }
            int contaa = 0;
            foreach (Carota ca in Carote)
            {
                //se l'energia della carota va a 0 imposta la carota come marcia e cambia l'immagine
                if (ca.energia <= 0)
                {
                   
                    ca.Marcio = true;
                    ca.img = GiocoDellaVita.Properties.Resources.carotamarcia;
                }
                //se il coniglio mangia la carota guadagna o perde energia.
                foreach (Coniglio cc in ConiglioCavia)
                {
                    if (cc.x == ca.x && cc.y == ca.y)
                    {
                        carotemangiate++;
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
                //se la carota è morta (mangiata) viene rimossa dall'array delle carote
                if (ca.vivo == false)
                {
                    var tmp = new List<Carota>(Carote);
                    tmp.RemoveAt(contaa);
                    Carote = tmp.ToArray();
                }
                contaa++;
            }
            turno++;
            //uso un invoke per modificare i parametri del form dal backgroundworker. Stackoverflow mi ha insegnato così
            Invoke((MethodInvoker)delegate
            {
                KillC.Text = conigliuccisi.ToString();
                KillCa.Text = carotemangiate.ToString();
                TurnoS.Text = turno.ToString();
            });  
            //dichiara il vincitore in base alle presenze dei personaggi
            if (ConiglioCavia.Length == 0 && volpecavia.Length == 0 && Carote.Length == 0)
            {
                MessageBox.Show("Gioco finito!\nNon ha vinto nessuno...", "FINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Invoke((MethodInvoker)delegate
                {
                    Start.Visible = false;
                    Stop.Visible = false;
                    CaroteUpDown.Visible = false;
                    VolpiUpDown.Visible = false;
                    ConigliUpDown.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                });  

                GameEngine.CancelAsync();
            }
            else if (ConiglioCavia.Length > 0 && volpecavia.Length == 0 && Carote.Length == 0)
            {
                MessageBox.Show("Gioco finito!\nHanno vinto i conigli!", "FINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Invoke((MethodInvoker)delegate
                {
                    Start.Visible = false;
                    Stop.Visible = false;
                    CaroteUpDown.Visible = false;
                    VolpiUpDown.Visible = false;
                    ConigliUpDown.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                });

                GameEngine.CancelAsync();
            }
            else if (ConiglioCavia.Length == 0 && volpecavia.Length > 0 && Carote.Length == 0)
            {
                FineMusica();
                MessageBox.Show("Gioco finito!\nHanno vinto le volpi!", "FINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Invoke((MethodInvoker)delegate
                {
                    Start.Visible = false;
                    Stop.Visible = false;
                    CaroteUpDown.Visible = false;
                    VolpiUpDown.Visible = false;
                    ConigliUpDown.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    
                });

                GameEngine.CancelAsync();
            }
            else if (ConiglioCavia.Length == 0 && volpecavia.Length == 0 && Carote.Length > 0)
            {
                MessageBox.Show("Gioco finito!\nHanno vinto le carote!", "FINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Invoke((MethodInvoker)delegate
                {
                    Start.Visible = false;
                    Stop.Visible = false;
                    CaroteUpDown.Visible = false;
                    VolpiUpDown.Visible = false;
                    ConigliUpDown.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                });

                GameEngine.CancelAsync();
            }
        }

        void FineMusica()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "Dogsong.wav";
            player.PlayLooping();
        }

    }
}
