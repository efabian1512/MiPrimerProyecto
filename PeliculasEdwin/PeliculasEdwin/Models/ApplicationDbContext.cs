using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PeliculasEdwin.Models
{

    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //DbContext Entity Framework
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //Cadena de conexion
        public ApplicationDbContext()
            : base("ConexionPCCasa", throwIfV1Schema: false)
        {
        }
        //DbSets o tablas
        public DbSet<Pelicula> PeliculasEdwin { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        //public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Clave primaria tabla PeliculasEdwin
            modelBuilder.Entity<Pelicula>().HasKey(x => x.Id);
            //Nombre de tabla
            modelBuilder.Entity<Pelicula>().ToTable("PeliculasEdwin");
            //llave foranea tabla comentario
            modelBuilder.Entity<Comentario>().HasRequired(x => x.Pelicula);
            //Nombre de tabla comentarios
            modelBuilder.Entity<Comentario>().ToTable("Comentarios");

            


        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}