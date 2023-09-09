namespace WAAOPOS.Models.ModelCliente
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelCliente : DbContext
    {
        public ModelCliente()
            : base("name=ModelCliente")
        {
        }

        public virtual DbSet<BitacoraErrores> BitacoraErrores { get; set; }
        public virtual DbSet<ConexionSAP> ConexionSAP { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<BitacoraUso> BitacoraUso { get; set; }
        public virtual DbSet<Login> Login { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.StackTrace)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.Metodo)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.SAPUser)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.SAPPass)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.SQLUser)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.ServerSQL)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.ServerLicense)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.SQLPass)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.SQLType)
                .IsUnicode(false);

            modelBuilder.Entity<ConexionSAP>()
                .Property(e => e.SQLBD)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.SQLProductos)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.SQLClientes)
                .IsUnicode(false);
        }
    }
}
