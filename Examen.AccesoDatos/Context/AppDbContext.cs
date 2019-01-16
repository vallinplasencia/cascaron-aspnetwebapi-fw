using Examen.Dominio.Entidades;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.AccesoDatos.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Responsable> Responsables { get; set; }
        public DbSet<Activo> Activos { get; set; }

        public AppDbContext()
           : base("ExamenConnection", throwIfV1Schema: false)
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Decimal con precision en el campo Valor en el objeto Categoria
            modelBuilder.Entity<Categoria>().Property(c => c.Valor).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

    }
}
