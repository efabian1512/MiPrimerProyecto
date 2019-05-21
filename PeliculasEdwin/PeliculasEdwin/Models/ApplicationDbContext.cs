using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    //DbContext Entity Framework
    public class ApplicationDbContext : DbContext
    {
        //Cadena de conexion
        public ApplicationDbContext()
            : base("ConexionPCCasa")
        {
        }
        //DbSets o tablas
        public DbSet<Pelicula> PeliculasEdwin { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Clave primaria tabla PeliculasEdwin
            modelBuilder.Entity<Pelicula>().HasKey(x => x.Id);
            //Nombre de tabla
            modelBuilder.Entity<Pelicula>().ToTable("PeliculasEdwin");
            //llave foranea tabla comentario
            modelBuilder.Entity<Comentario>().HasRequired(x => x.Pelicula);
            //Nombre de tabla comentarios
            modelBuilder.Entity<Comentario>().ToTable("Comentarios");
           

        }
    }
}