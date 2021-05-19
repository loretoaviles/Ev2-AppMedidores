using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketUtils
{
    public class MedidorSocket
    {
        private string ip;
        private int puerto;
        private Socket comServidor;
        private StreamReader reader;
        private StreamWriter writer;

        public MedidorSocket(Socket comServidor)
        {
            this.comServidor = comServidor;
            Stream stream = new NetworkStream(this.comServidor);
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
        }

        public MedidorSocket(string ip, int puerto)
        {
            this.puerto = puerto;
            this.ip = ip;
        }

        public bool Conectar()
        {
            try
            {
                this.comServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), puerto);
                this.comServidor.Connect(endpoint);
                Stream stream = new NetworkStream(this.comServidor);
                this.reader = new StreamReader(stream);
                this.writer = new StreamWriter(stream);
                return true;
            }
            catch (IOException ex)
            {

                return false;
            }
        }

        public bool Escribir(string mensaje)
        {
            try
            {
                this.writer.WriteLine(mensaje);
                this.writer.Flush();
                return true;
            }
            catch (IOException ex)
            {
                return false;
            }
        }

        public string Leer()
        {
            try
            {
                
                return this.reader.ReadLine().Trim();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void CerrarConexion()
        {
            this.comServidor.Close();
        }

    }

   
}
