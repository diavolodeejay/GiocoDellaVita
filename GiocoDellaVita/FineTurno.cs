using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiocoDellaVita
{
    class FineTurnoClass
    {
        public static Personaggio[] Muori(Personaggio[] pgarray, int pos)
        {
            var tmp = new List<Personaggio>(pgarray);
            tmp.RemoveAt(pos);
            pgarray = tmp.ToArray();
            return pgarray;
        }

    }
}
