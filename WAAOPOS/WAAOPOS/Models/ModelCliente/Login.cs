  
namespace WAAOPOS.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.UI.WebControls;

    [Table("Login")]

    public partial class Login
    {
        public int id { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
    }
}