using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WAAOPOS.Models
{
    public class Clientes
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public PersonaContacto PersonaContacto { get; set; }
    }
    public class Ubicacion
    {
        public string Calle { get; set; }
        public string Ciudad { get; set; }

    }

    public class PersonaContacto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}