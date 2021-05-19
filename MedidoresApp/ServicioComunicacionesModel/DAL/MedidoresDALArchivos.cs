using ServicioComunicacionesModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioComunicacionesModel.DAL
{
    public class MedidoresDALArchivos : IMedidoresDAL
    {
        //Singleton:
        //1. constructor privado
        private MedidoresDALArchivos()
        {

        }
        //2. Referencia static de si mismo
        private static IMedidoresDAL instancia;
        //3. un metodo estatico que sea el unico que permita acceder a la instancia
        public static IMedidoresDAL GetInstance()
        {
            if (instancia==null)
                instancia = new MedidoresDALArchivos();
            return instancia;
        }

        static List<Medidor> medidores = new List<Medidor>()
        {
          //Aqui se crean los medidores estaticos existentes en el servidor
           new Medidor(){Id= 1010,Lectura = new Lectura(){Tipo="Trafico"} },
           new Medidor(){Id= 1020,Lectura = new Lectura(){Tipo="Trafico"} },
           new Medidor(){Id= 1030,Lectura = new Lectura(){Tipo="Trafico"} },
           new Medidor(){Id= 1040,Lectura = new Lectura(){Tipo="Trafico"} },

           new Medidor(){Id= 1110,Lectura = new Lectura(){Tipo="Consumo"} },
           new Medidor(){Id= 1120,Lectura = new Lectura(){Tipo="Consumo"} },
           new Medidor(){Id= 1130,Lectura = new Lectura(){Tipo="Consumo"} },
           new Medidor(){Id= 1140,Lectura = new Lectura(){Tipo="Consumo"} }

        };
        public List<Medidor> ObtenerMedidores()
        {
            return medidores;
        }
    }
}
