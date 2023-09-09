namespace WAAOPOS.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Parametros
    {
        public int id { get; set; }

        public string SQLProductos { get; set; }

        public string SQLClientes { get; set; }

        public int SerieOV { get; set; }
    }
}
