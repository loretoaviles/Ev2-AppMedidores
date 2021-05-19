using ServicioComunicacionesApp.Hilos;
using System;
using System.Configuration;
using System.Threading;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ServicioComunicacionesApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            //Console.WriteLine("Iniciando Servidor");
            HiloServidor hiloServidor = new HiloServidor(puerto);
            Thread t = new Thread(new ThreadStart(hiloServidor.Ejecutar));
            
            t.Start();
        }
    }
}
