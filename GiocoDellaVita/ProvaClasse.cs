using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coooooords
{
    abstract class CCoords
    {
        private int mX;
        private int mY;

        public int X
        {
            get { return mX; }
            set { mX = value; }
        }

        public int Y
        {
            get { return mY; }
            set { mY = value; }
        }

        public CCoords()
        {
            X = 0;
            Y = 0;
        }

        public CCoords(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        ~CCoords()
        {
            
        }//DISTRUTTORE

        public virtual double Distanza()
        {
            return (Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)));   
        }

        public abstract string foo();  /* Con 'virtual' si ha la scelta di ridefinire il metodo nelle sottoclassi. Con 'abstract' è obbligatorio ridefinire il metodo nelle sottoclassi
        
                                        è una classe guida, obbliga chi usa la classe a creare una sottoclasse col metodo 'foo' 
                                        */   
    }

    class CColorCoords : CCoords
    {
        private System.Drawing.Color mColor;

        public System.Drawing.Color Color
        {
            get { return mColor;}
            set { mColor = value;}
        }

        public CColorCoords() : base()
        {
            Color = System.Drawing.Color.Black;
        }

        public CColorCoords(int X, int Y) : base(X,Y)
        {
            this.X = 5;
            this.Y = 7;
            Color = System.Drawing.Color.Red;
        }

        ~CColorCoords()
        {
            Color = System.Drawing.Color.White;
        } //Distruttore

        public override string ToString()
        {
            return "Coordinate ColorCoords";
        }

        public override string foo()
        {
            return "foo-ColorCoords"; 
        }

    }

    class C3DCoords : CCoords
    {
        private int mZ;

        public int Z
        {
            get {return mZ;}
            set {mZ = value;}
        }

        public C3DCoords()
        {
            Z = 0;
        }

        public C3DCoords(int X, int Y, int Z) : base (X,Y)
        {
            this.Z = Z;
            this.X = X;
            this.Y = Y;
        }

        public override string ToString()
        {
            return "Coordinate 3DColor";
        }

        public override double Distanza()
        {
            return (Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)));
        }

        public override string foo()
        {
            return "foo-3DCoords";
        }

    }


}