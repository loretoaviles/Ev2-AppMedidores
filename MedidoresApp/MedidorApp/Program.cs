using ServicioComunicacionesModel.DTO;
using SocketUtils;
using System;
using System.Configuration;
using System.Globalization;

namespace MedidorApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Medidor m = Ejecutar();
            string ip = ConfigurationManager.AppSettings["ip"];
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            MedidorSocket medidorSocket = new MedidorSocket(ip, puerto);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("conectandose al servidor {0} en el puerto {1}", ip, puerto);
            if (medidorSocket.Conectar())
            {
                //Protocolo de comunicacion
                //1-Cliente
                string mensaje = m.Lectura.Fecha + "|" + m.Id + "|" + m.Lectura.Tipo;
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Datos Enviados al servidor: {0} ", mensaje);
                Console.ForegroundColor = ConsoleColor.Green;
                medidorSocket.Escribir(mensaje);
                //3-Cliente     
                mensaje = medidorSocket.Leer();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Mensaje recibido del servidor:{0}",mensaje);
                Console.ForegroundColor = ConsoleColor.Green;
                if (mensaje.Substring(mensaje.Length - 4).Equals("WAIT"))
                {
                    //WAIT
                    string respuesta = EjecutarRespuesta(m);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Mensaje enviado al servidor: " + respuesta);
                    Console.ForegroundColor = ConsoleColor.Green;
                    medidorSocket.Escribir(respuesta);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Mensaje recibido del servidor:" + medidorSocket.Leer());
                    Console.ForegroundColor = ConsoleColor.Green;
                    
                }
                Console.WriteLine("Aprete una tecla para cerrar la sesion");
                Console.ReadKey();
                medidorSocket.CerrarConexion();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error de conexion");
            }
        }

        private static string EjecutarRespuesta(Medidor m)
        {
            string respuesta = "";
            //nroserie
            int nroSerie;
            do
            {
                Console.WriteLine("Ingrese numero de serie");
                try
                {
                    nroSerie = Convert.ToInt32(Console.ReadLine().Trim());

                }
                catch (Exception ex)
                {
                    nroSerie = -1;
                    Console.WriteLine("Tiene que ingresar el mismo numero de serie");
                }
            } while (nroSerie != m.Id);
            //fecha
            DateTime dt;
            string fechaString = "";
            bool validarFecha = false;
            do
            {
                Console.WriteLine("Ingrese fecha (Formato yyyy-MM-dd-HH-mm-ss)");
                fechaString = Console.ReadLine();
                validarFecha = DateTime.TryParseExact(fechaString, "yyyy-MM-dd-HH-mm-ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dt);
                if (!validarFecha)
                {
                    Console.WriteLine("ingrese fecha valida");
                }
            } while (!validarFecha);
            string fecha = dt.ToString("yyyy-MM-dd-HH-mm-ss");
            //tipo
            string opc;
            string tipo = "";
            do
            {
                Console.WriteLine("Ingrese que tipo medidor");
                Console.WriteLine("1.Trafico");
                Console.WriteLine("2.Consumo");
                opc = Console.ReadLine();
                switch (opc)
                {
                    case "1":
                        tipo = "Trafico";
                        break;
                    case "2":
                        tipo = "Consumo";
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta");
                        opc = null;
                        break;
                }
            } while (opc == null || !(tipo.Equals(m.Lectura.Tipo)));
            //Valor
            int valor = 0;
            if (tipo.Equals("Trafico"))
                Console.WriteLine("Ingrese cantidad de vehiculos");
            else
                Console.WriteLine("Ingrese consumo actual del medidor");

            do
            {
                try
                {
                    valor = Convert.ToInt32(Console.ReadLine().Trim());
                    if (valor > 1000 || valor < 0)
                    {
                        Console.WriteLine("ingrese opcion valida");
                    }
                }
                catch (Exception ex)
                {
                    valor = -1;
                    Console.WriteLine("ingrese opcion valida");
                }
            } while (valor > 1000 || valor < 0);
            //Estado
            int estado = 3;
            do {
                Console.WriteLine("Ingrese estado");
                Console.WriteLine("1. Error de lectura "); //-1
                Console.WriteLine("2. OK ");                // 0    
                Console.WriteLine("3. Punto de carga lleno");//1
                Console.WriteLine("4. Requiere mantencio preventiva");//2
                Console.WriteLine("0. Sin estado ");// sin estado
                string opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        estado = -1;
                        break;
                    case "2":
                        estado = 0;
                        break;
                    case "3":
                        estado = 1;
                        break;
                    case "4":
                        estado = 2;
                        break;
                    case "0":
                        estado = 3;
                        break;
                    default:
                        estado = 4;
                        Console.WriteLine("Ingrese una opcion valida");
                        break;
                }
            } while (opc == null || opc.Equals(4));
            //Crear el String respuesta
            respuesta = nroSerie + "|" + fecha + "|" + tipo + "|" + valor + "|";
            if (estado != 3)
                respuesta += estado + "|UPDATE";
            else
                respuesta += "UPDATE";

            return respuesta;
        }
        public static Medidor Ejecutar()
        {
            //tipo
            string opc;
            string tipo = "";
            do
            {
                Console.WriteLine("Ingrese que tipo medidor");

                Console.WriteLine("1.Trafico");
                Console.WriteLine("2.Consumo");
                opc = Console.ReadLine();
                switch (opc)
                {
                    case "1":
                        tipo = "Trafico";
                        break;
                    case "2":
                        tipo = "Consumo";
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta");
                        opc = null;
                        break;
                }
            } while (opc == null);
            //nroSerie
            int nroSerie;
            do
            {
                Console.WriteLine("Ingrese numero de serie");
                try
                {
                    nroSerie = Convert.ToInt32(Console.ReadLine().Trim());
                }
                catch (Exception ex)
                {
                    nroSerie = -1;
                    Console.WriteLine("Ingrese numero de serie valido");
                }
            } while (nroSerie < 0);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            //crear medidor

            Lectura l = new Lectura();

            l.Tipo = tipo;
            l.Fecha = fecha;
            Medidor m = new Medidor()
            {
                Id = nroSerie,
                Lectura = l
            };



            return m;

        }
    }

}

