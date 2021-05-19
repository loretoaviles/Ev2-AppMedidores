using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ServicioComunicacionesModel.DTO;

namespace ServicioComunicacionesModel.DAL
{
    public class LecturasDALArchivos : ILecturasDAL
    {
        //Singleton
        //1 Constructor Privado
        private LecturasDALArchivos()
        {

        }
        //2 Referencia a si mismo
        private static ILecturasDAL instancia;
        //3 metodo static para obtener instancia
        public static ILecturasDAL GetInstance()
        {
            if (instancia == null)
                instancia = new LecturasDALArchivos();
            return instancia;
        }

        private string archivoTrafico = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "traficos.txt";
        private string archivoConsumo = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "consumos.txt";

        public List<Lectura> ObtenerLecturasConsumo()
        {
            List<Lectura> lecturasConsumo = new List<Lectura>();
            try
            {
                using (StreamReader reader = new StreamReader(archivoConsumo))
                {
                    string texto = null;
                    do
                    {
                        
                        texto = reader.ReadLine();
                        if (texto != null)
                        {
                            //Parsear El JSON
                            Lectura lc = new JavaScriptSerializer().Deserialize<Lectura>(texto);
                            if (lc.Tipo.Equals("Consumo"))
                                lecturasConsumo.Add(lc);
                        }
                    } while (texto != null);
                }
            }
            catch (IOException ex)
            {
                lecturasConsumo = null;
            }

            return lecturasConsumo;

        }

        public List<Lectura> ObtenerLecturasTrafico()
        {
            List<Lectura> lecturasTrafico = new List<Lectura>();
            try
            {
                using (StreamReader reader = new StreamReader(archivoTrafico))
                {
                    string texto = null;
                    do
                    {
                        //Lectura del JSON
                       
                        texto = reader.ReadLine();
                        if (texto != null)
                        {
                            //Parsear El JSON
                            Lectura lt = new JavaScriptSerializer().Deserialize<Lectura>(texto);
                            if (lt.Tipo.Equals("Trafico"))
                                lecturasTrafico.Add(lt);

                        }
                    } while (texto != null);
                }
            }
            catch (IOException ex)
            {
                lecturasTrafico = null;
            }

            return lecturasTrafico;
        }

        public void RegistrarLectura(Lectura l)
        {
            string ubicacion = "";
            if (l.Tipo.Equals("Trafico"))
            {
                ubicacion = archivoTrafico;
            }
            else
            {
                ubicacion = archivoConsumo;
            }
           

            string lecturaJSON = new JavaScriptSerializer().Serialize(l);
            try
            {
                using (StreamWriter writer = new StreamWriter(ubicacion, true))
                {
                    writer.WriteLine(lecturaJSON);
                    writer.Flush();

                }
            }
            catch (IOException ex)
            {

            }





        }
    }
}
