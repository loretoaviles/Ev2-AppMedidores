using ServicioComunicacionesModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioComunicacionesModel.DAL
{
    public interface ILecturasDAL
    {
        void RegistrarLectura(Lectura l);
        List<Lectura> ObtenerLecturasTrafico();
        List<Lectura> ObtenerLecturasConsumo();
    }
}
