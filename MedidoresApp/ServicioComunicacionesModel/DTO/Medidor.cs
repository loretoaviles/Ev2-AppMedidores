using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioComunicacionesModel.DTO
{
    public class Medidor
    {
        int id;
        Lectura lectura;


     

        public int Id { get => id; set => id = value; }
        public Lectura Lectura { get => lectura; set => lectura = value; }

        public static Boolean EnviarLectura(Lectura l)
       {
            return true;
       }


    }
}
