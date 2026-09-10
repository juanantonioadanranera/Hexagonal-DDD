using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHexagonalDDD.Applicaion
{
    class ClienteAlquilerVehiculo
    {
        private string codigo_vehiculo;
        private string codigo_cliente;

        public string Codigo_vehiculo { get => codigo_vehiculo; set => codigo_vehiculo = value; }
        public string Codigo_cliente { get => codigo_cliente; set => codigo_cliente = value; }
    }
}
