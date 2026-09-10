using Microsoft.Extensions.Hosting;
using System;

namespace WPF_Hexagonal_DDD
{
    class Program
    {
        /*
         No utilizar. Para la capa de presentacion Utiliza segun tu preferencia WPF o Windows Forms.
         Este punto de inicio solo es funcional en un proyecto de consola
         */
        static void Main(string[] args)
        {
            IHost host = Bootstrapper.BuildHost();
            host.Start();

            Console.WriteLine("Aplicación iniciada. Presiona cualquier tecla para salir...");
            Console.ReadKey();

            host.StopAsync();
            host.Dispose();
        }
    }
}
