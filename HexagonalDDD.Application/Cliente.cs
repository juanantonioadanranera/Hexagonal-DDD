using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFHexagonalDDD.Applicaion
{
    class Cliente
    {
        private string codigo_cliente;
        private string nif;
        private string nombre;
        private string direccion;
        private string telefono;
        private string codpostal;
        private string municipio;
        private string provincia;
        private string pais;
        private bool tieneAlquilado;

        public string NIF { get => nif; set => nif = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Codpostal { get => codpostal; set => codpostal = value; }
        public string Municipio { get => municipio; set => municipio = value; }
        public string Provincia { get => provincia; set => provincia = value; }
        public string Pais { get => pais; set => pais = value; }
        public bool TieneAlquilado { get => tieneAlquilado; set => tieneAlquilado = value; }
        public string Codigo_cliente { get => codigo_cliente; set => codigo_cliente = value; }
    }
}
