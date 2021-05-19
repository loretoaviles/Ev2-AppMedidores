using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioComunicacionesModel.DTO
{
   public class PuntoCarga
    {
        int idPuntoCarga;
        string tipo;
        int capacidad;
        DateTime vida;
        MedidorConsumo medidorConsumo;
        MedicionTrafico medidorTrafico;
        EstacionServicio estacionServicio;

    }
}
