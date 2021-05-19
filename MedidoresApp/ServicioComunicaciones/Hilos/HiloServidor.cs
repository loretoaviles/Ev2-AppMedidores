using SocketUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicioComunicacionesApp.Hilos
{
    class HiloServidor
    {
        private int puerto;
        private ServidorSocket servidor;
        public HiloServidor(int puerto)
        {
            this.puerto = puerto;
        }

        public void Ejecutar()
        {
            servidor = new ServidorSocket(puerto);
            Console.WriteLine("iniciando servidor en puerto {0}", puerto);
            if (servidor.Iniciar())
            {
                Console.WriteLine("Servidor Iniciado");
                while (true)
                {
                    Console.WriteLine("Esperando Medidores...");
                    MedidorSocket medidorSocket = servidor.ObtenerMedidorCliente();
                    Console.WriteLine("cliente encontrado!");
                    HiloMedidor hiloMedidor = new HiloMedidor(medidorSocket);
                    Thread t = new Thread(new ThreadStart(hiloMedidor.Ejecutar));
                    t.IsBackground = true;
                    t.Start();
                }



            }

        }
    }
   
}
