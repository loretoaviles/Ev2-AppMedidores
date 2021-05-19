using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioComunicacionesModel.DTO
{
    public class Lectura
    {
        string fecha;
        string tipo;
        int valor;
        int estado;
        public string Fecha { get => fecha; set => fecha = value; }
        public int Valor { get => valor; set => valor = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public int Estado { get => estado; set => estado = value; }
    }
}
