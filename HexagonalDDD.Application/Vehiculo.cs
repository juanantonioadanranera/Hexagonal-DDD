using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHexagonalDDD.Application
{
    class Vehiculo
    {
        private string codigo_vehiculo;
        private string matricula;
        private string modelo;
        private string marca;
        private string año;
        private string cilindrada;
        private string caballaje;
        private string color;

        public string Matricula { get => matricula; set => matricula = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Año { get => año; set => año = value; }
        public string Cilindrada { get => cilindrada; set => cilindrada = value; }
        public string Caballaje { get => caballaje; set => caballaje = value; }
        public string Color { get => color; set => color = value; }
        public string Codigo_vehiculo { get => codigo_vehiculo; set => codigo_vehiculo = value; }
    }
}
