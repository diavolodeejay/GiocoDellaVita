﻿using System;
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
        //Proprietà condivise da tutti i personaggi
        private int mEnergia,mX,mY;
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
        //costruttore di default che assegna 100 di energia e imposta come vivo il personaggio
        public Personaggio()
        {
            energia = 100;
            vivo = true;
        }
        //Classe astratta del movimento
        public abstract string Muovi(int x,int y);
    }
    //Classe Volpe
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
            //Dove andare in base alla posizione del personaggio attuale e del bersaglio
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


    //Classe Coniglio
    class Coniglio : Personaggio
    {
        //L'immagine del coniglio
        public Bitmap img = new Bitmap(Properties.Resources.yoshi);
        //Metodo per il movimento che controlla che il coniglio non esca dal campo
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

        /// <summary>
        /// Determina il percorso da fare per andare a mangiare la carota più vicina. Funziona bene!
        /// </summary>
        /// <param name="vittime">L'array di carote dove trovare la carota più vicina</param>
        /// <returns>Le nuove coordinate del coniglio</returns>
        public string Percorso(Carota[] vittime)
        {
            //se non ci sono più carote restituisce NOCA.
            if (vittime.Length <= 0)
            {
                return "NOCA";
            }
            int nx = 0, ny = 0;
            double[] vicino = new double[3] {15,0,0};
            foreach(Carota vittima in vittime)
            {
                //Calcola la carota più vicina
                double temp = 0;
                temp = Math.Sqrt(Math.Pow(Convert.ToDouble(this.x) - Convert.ToDouble(vittima.x), 2) + Math.Pow(Convert.ToDouble(this.y) - Convert.ToDouble(vittima.y), 2));
                if(temp < vicino[0])
                {
                    vicino[0] = temp;
                    vicino[1] = vittima.x;
                    vicino[2] = vittima.y;
                }
            }
            //Dove andare in base alla posizione del personaggio attuale e del bersaglio
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

        /// <summary>
        /// Determina il percorso da fare per evitare la volpe più vicina. Funziona male!
        /// </summary>
        /// <param name="cacciatori">L'array di volpi da evitare</param>
        /// <returns>Le nuove coordinate del coniglio</returns>
        public string Scappa(Volpe[] cacciatori)
        {
            int nx = 0, ny = 0;
            double[] vicino = new double[3] { 15, 0, 0 };
            foreach (Volpe preda in cacciatori)
            {
                //Calcola il personaggio più vicino
                double temp = 0;
                temp = Math.Sqrt(Math.Pow(Convert.ToDouble(this.x) - Convert.ToDouble(preda.x), 2) + Math.Pow(Convert.ToDouble(this.y) - Convert.ToDouble(preda.y), 2));
                if (temp < vicino[0])
                {
                    vicino[0] = temp;
                    vicino[1] = preda.x;
                    vicino[2] = preda.y;
                }
            }
            //Dove andare in base alla posizione del personaggio attuale e del cacciatore.
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
        /// <summary>
        /// Costruttore di default
        /// </summary>
        public Coniglio()
        {
            this.x = 0;
            this.y = 0;
        }
    }

    class Carota : Personaggio
    {
        //immagine della carota
        public Bitmap img = new Bitmap(Properties.Resources.CarotaFalza);
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
        /// <summary>
        /// Le carote non si muovono LOL
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>Coordinate dove dovrebbe muoversi. MA LE CAROTE NON SI MUOVONO, SONO CAROTE</returns>
        public override string Muovi(int x, int y)
        {
            string r = "cell" + x.ToString() + y.ToString();
            this.energia--;
            return r;
        }
    }
}
///(C) Di Nuovo Gabriele - 2016 
///Grazie stackoverflow per esistere.