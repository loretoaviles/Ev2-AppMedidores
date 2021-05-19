using ServicioComunicacionesModel.DAL;
using ServicioComunicacionesModel.DTO;
using SocketUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioComunicacionesApp.Hilos
{
    class HiloMedidor
    {
        private IMedidoresDAL dalM = MedidoresDALFactory.CreateDal();
        private ILecturasDAL dalL = LecturasDALFactory.CreateDal();
        private MedidorSocket medidorSocket;

        public HiloMedidor(MedidorSocket medidorSocket)
        {
            this.medidorSocket = medidorSocket;
        }

        public void Ejecutar()
        {
            //Aqui se encuentra el protoco de comunicacion del lado del Servidor
            //2-Servidor 
            Console.ForegroundColor = ConsoleColor.Green;
            string mensaje = medidorSocket.Leer();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Datos recibidos desde el Medidor:{0}",mensaje);
            //validar
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Validando datos...");
            //Parsear mensaje
            string[] textoArray = mensaje.Split('|');
            //Crear medidor
            Medidor medidorCliente = new Medidor()
            {
                Id = Convert.ToInt32(textoArray[1]),
                Lectura = new Lectura() {Tipo=textoArray[2] }
            };
            //Validar fecha
            DateTime fechaMedidor = DateTime.ParseExact(textoArray[0], "yyyy-MM-dd-HH-mm-ss",null);
            TimeSpan ts = DateTime.Now - fechaMedidor;
            bool fechaValida = false; 
            if (ts.TotalMinutes<30)
                fechaValida = true;     //TODO: Ta bien
            else
                fechaValida = false;    //TODO: ESta mal
            //Validar numero serie y tipo
            List<Medidor> medidores = dalM.ObtenerMedidores();
            Medidor medidorMatch = medidores.Find(m => m.Id == medidorCliente.Id && m.Lectura.Tipo == medidorCliente.Lectura.Tipo);
            if (medidorMatch != null && fechaValida)
            {
                //Medidor encontrado en la lista static
                string respuesta = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "|WAIT";            
                Console.WriteLine("Medidor encontrado en la lista!");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Enviando al cliente: {0}", respuesta);
                medidorSocket.Escribir(respuesta);
                //4-Servidor
                mensaje = medidorSocket.Leer();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Respuesta recibida del medidor:{0}",mensaje);
                Console.ForegroundColor = ConsoleColor.Green;
                //Parsear mensaje
                string[] textoArray2 = mensaje.Split('|');
                //Crear medidor
                Medidor medidorCliente2 = new Medidor()
                {
                    Id = Convert.ToInt32(textoArray2[0]),
                    Lectura = new Lectura() { Fecha = textoArray2[1], Tipo = textoArray2[2], Valor = Convert.ToInt32(textoArray2[3]) }
                };
                if (!textoArray2[4].Equals("UPDATE"))
                    medidorCliente2.Lectura.Estado = Convert.ToInt32(textoArray2[4]);
                else
                    medidorCliente2.Lectura.Estado = 3;
                //dal save
                lock (dalL)
                {
                    dalL.RegistrarLectura(medidorCliente2.Lectura);
                }
 
                Console.WriteLine("Enviado OK al cliente");
                medidorSocket.Escribir(medidorCliente2.Id + "|OK");

            }
            else
            {
                //No valido
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("El medidor no existe en la lista");
                mensaje = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")+"|"+medidorCliente.Id+"|ERROR";
                Console.WriteLine("Enviando al cliente: {0}",mensaje); 
                medidorSocket.Escribir(mensaje);
                Console.ForegroundColor = ConsoleColor.Green;
            }

            
            medidorSocket.CerrarConexion();
        }


    }
}
