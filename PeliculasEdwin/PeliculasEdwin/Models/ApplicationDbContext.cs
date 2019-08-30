using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PeliculasEdwin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PeliculasEdwin.Models
{

    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(30, ErrorMessage = "Maximo permitido, 30 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "Maximo permitido, 50 caracteres.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name ="Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }
        public List<string> NombreRol { get; set; }
        //[Display(Name ="Teléfono")]
        //public string Telefono { get; set; }
        

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
        public DbSet<Visita> Visitas { get; set; }
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
            modelBuilder.Entity<Visita>().HasRequired(x => x.Pelicula);

            


        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}