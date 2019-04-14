using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("ConexionPCCasa")
        {
        }
        public DbSet<Pelicula> PeliculasEdwin { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pelicula>().HasKey(x => x.Id);
            modelBuilder.Entity<Pelicula>().ToTable("PeliculasEdwin");

        }
    }
}