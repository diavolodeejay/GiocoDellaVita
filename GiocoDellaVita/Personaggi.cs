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

namespace GiocoDellaVita
{
    abstract class Personaggio
    {
        private int mEnergia,mX,mY,mTipo;
        private bool mVivo;    
        public bool vivo
        {
            get
            {
                return mVivo;
            }
            set
            {
                mVivo = value;
            }
        }

        public int tipo
        {
            get
            {
                return mTipo;
            }
        }

        public int energia
        {
            get
            {
                return mEnergia;
            }
            set
            {
                mEnergia = value;
            }
        }

        public int x
        {
            get
            {
                return mX;
            }
            set
            {
                mX = value;
            }
        }

        public int y
        {
            get
            {
                return mY;
            }
            set
            {
                mY = value;
            }
        }

        public Personaggio()
        {
            energia = 100;
            vivo = true;
            mTipo = 0;
        }

        /// <summary>
        /// Crea un personaggio con 100 di energia
        /// </summary>
        /// <param name="Tipo">Il tipo di personaggio. -1 Carota morta, 0 Carota, 1 Coniglio, 2 Volpe</param>
        public Personaggio(int Tipo)
        {
            if (Tipo == 0)
            {
                energia = 10;
            }
            else if (Tipo == 1)
            {
                energia = 14;
            }
            else if (Tipo == 2)
            {
                energia = 12;
            }
            vivo = true;
            mTipo = Tipo;
        }

        public Personaggio(int energia,int Tipo,int x,int y)
        {
            this.energia = energia;
            vivo = true;
            mX = x;
            mY = y;
            mTipo = Tipo;
        }

        public abstract string Muovi(int x,int y);
        //public abstract string Percorso(Personaggio vittima);
    }

    class Volpe : Personaggio
    {
        //immagine della volpe
        public Bitmap img = new Bitmap(Properties.Resources.Dog);
        public override string Muovi(int x, int y)
        {
            this.x += x;
            if(this.x < 0)
            {
                this.x = 0;
            }
            if(this.x > 9)
            {
                this.x = 9;
            }
            this.y += y;
            if (this.y < 0)
            {
                this.y = 0;
            }
            if (this.y > 9)
            {
                this.y = 9;
            }
            string nome = "cell" + this.x.ToString() + this.y.ToString();
            return nome;
        }
        /// <summary>
        /// Determina il percorso da fare per andare a mangiare il coniglio più vicino
        /// </summary>
        /// <param name="vittime">Array di conigli dove cercare il coniglio più vicino</param>
        /// <returns>La nuova posizione della volpe</returns>
        public string Percorso(Coniglio[] vittime)
        {
            int nx = 0, ny = 0;
            double[] vicino = new double[3] {15,0,0};
            foreach(Coniglio vittima in vittime)
            {
                double temp = 0;
                temp = Math.Sqrt(Math.Pow(Convert.ToDouble(this.x) - Convert.ToDouble(vittima.x), 2) + Math.Pow(Convert.ToDouble(this.y) - Convert.ToDouble(vittima.y), 2));
                if(temp < vicino[0])
                {
                    vicino[0] = temp;
                    vicino[1] = vittima.x;
                    vicino[2] = vittima.y;
                }
            }
            if (this.x > vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = -1;
                    ny = -1;
                }
                else if (this.y == vicino[2])
                {
                    nx = -1;
                    ny = 0;
                }
                else if (this.y < vicino[2])
                {
                    nx = -1;
                    ny = 1;
                }
            }
            else if (x == vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = 0;
                    ny = -1;
                }
                else if (this.y < vicino[2])
                {
                    nx = 0;
                    ny = 1;
                }
            }
            else if (x < vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = 1;
                    ny = -1;
                }
                else if (this.y == vicino[2])
                {
                    nx = 1;
                    ny = 0;
                }
                else if(this.y < vicino[2])
                {
                    nx = 1;
                    ny = 1;
                }
            }
            string ris;
            ris = Muovi(nx, ny);
            this.energia--;
            return ris;
        }

        public Volpe()
        {
            this.x = 0;
            this.y = 0;
        }
    }

    class Coniglio : Personaggio
    {
        public Bitmap img = new Bitmap(Properties.Resources.yoshi);
        public override string Muovi(int x, int y)
        {
            this.x += x;
            if (this.x < 0)
            {
                this.x = 0;
            }
            if (this.x > 9)
            {
                this.x = 9;
            }
            this.y += y;
            if (this.y < 0)
            {
                this.y = 0;
            }
            if (this.y > 9)
            {
                this.y = 9;
            }
            string nome = "cell" + this.x.ToString() + this.y.ToString();
            return nome;
        }

        public string Percorso(Carota[] vittime)
        {
            if (vittime.Length <= 0)
            {
                return "NOCA";
            }
            int nx = 0, ny = 0;
            double[] vicino = new double[3] {15,0,0};
            foreach(Carota vittima in vittime)
            {
                double temp = 0;
                temp = Math.Sqrt(Math.Pow(Convert.ToDouble(this.x) - Convert.ToDouble(vittima.x), 2) + Math.Pow(Convert.ToDouble(this.y) - Convert.ToDouble(vittima.y), 2));
                if(temp < vicino[0])
                {
                    vicino[0] = temp;
                    vicino[1] = vittima.x;
                    vicino[2] = vittima.y;
                }
            }
            if (this.x > vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = -1;
                    ny = -1;
                }
                else if (this.y == vicino[2])
                {
                    nx = -1;
                    ny = 0;
                }
                else if (this.y < vicino[2])
                {
                    nx = -1;
                    ny = 1;
                }
            }
            else if (x == vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = 0;
                    ny = -1;
                }
                else if (this.y < vicino[2])
                {
                    nx = 0;
                    ny = 1;
                }
            }
            else if (x < vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = 1;
                    ny = -1;
                }
                else if (this.y == vicino[2])
                {
                    nx = 1;
                    ny = 0;
                }
                else if(this.y < vicino[2])
                {
                    nx = 1;
                    ny = 1;
                }
            }
            string ris;
            ris = Muovi(nx, ny);
            this.energia--;
            return ris;
        }

        //rifai
        public string Scappa(Volpe[] prede)
        {
            int nx = 0, ny = 0;
            double[] vicino = new double[3] { 15, 0, 0 };
            foreach (Volpe preda in prede)
            {
                double temp = 0;
                temp = Math.Sqrt(Math.Pow(Convert.ToDouble(this.x) - Convert.ToDouble(preda.x), 2) + Math.Pow(Convert.ToDouble(this.y) - Convert.ToDouble(preda.y), 2));
                if (temp < vicino[0])
                {
                    vicino[0] = temp;
                    vicino[1] = preda.x;
                    vicino[2] = preda.y;
                }
            }
            if (this.x > vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = +1;
                    ny = +1;
                }
                else if (this.y == vicino[2])
                {
                    nx = +1;
                    ny = -1;
                }
                else if (this.y < vicino[2])
                {
                    nx = +1;
                    ny = -1;
                }
            }
            else if (x == vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = -1;
                    ny = +1;
                }
                else if (this.y < vicino[2])
                {
                    nx = -1;
                    ny = -1;
                }
            }
            else if (x < vicino[1])
            {
                if (this.y > vicino[2])
                {
                    nx = -1;
                    ny = +1;
                }
                else if (this.y == vicino[2])
                {
                    nx = -1;
                    ny = -1;
                }
                else if (this.y < vicino[2])
                {
                    nx = -1;
                    ny = -1;
                }
            }
            string ris;
            ris = Muovi(nx, ny);
            this.energia--;
            return ris;
        }

        public Coniglio()
        {
            this.x = 0;
            this.y = 0;
        }
    }

    class Carota : Personaggio
    {
        public Bitmap img = new Bitmap(Properties.Resources.Carota);
        private bool mMarcio = false;
        public bool Marcio
        {
            get
            {
                return mMarcio;

            }
            set
            { 
                mMarcio = value;
            }
        }
        public override string Muovi(int x, int y)
        {
            string r = "cell" + x.ToString() + y.ToString();
            this.energia--;
            return r;
        }
    }
}
