 

namespace WAAOPOS.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.UI.WebControls;

    [Table("BitacoraUso")]

    public partial class BitacoraUso
    {
        public int id { get; set; }
        public string Descripcion { get; set; }
        public string IP { get; set; }
        public DateTime Fecha { get; set; }
        public string Respuesta { get; set; }
        public string Metodo { get; set; }
    }
}