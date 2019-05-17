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
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<EstudiantePrueba> EstudiantePrueba { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pelicula>().HasKey(x => x.Id);
            modelBuilder.Entity<Pelicula>().ToTable("PeliculasEdwin");
            modelBuilder.Entity<Comentario>().HasRequired(x => x.Pelicula);
            modelBuilder.Entity<Comentario>().ToTable("Comentarios");
            modelBuilder.Entity<EstudiantePrueba>().ToTable("Estudiantes");

        }
    }
}