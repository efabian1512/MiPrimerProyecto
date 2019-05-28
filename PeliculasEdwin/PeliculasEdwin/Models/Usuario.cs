using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(30,ErrorMessage ="Maximo permitido, 30 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "Maximo permitido, 50 caracteres.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(15, ErrorMessage = "Maximo permitido, 15 caracteres.")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "Maximo permitido, 50 caracteres.")]
        public string CorreoElectronico { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "Maximo permitido, 50 caracteres.")]
        [Compare("CorreoElectronico", ErrorMessage ="Los correos no coinciden.")]
        public string ConfirmarCorreo { get; set; }
        [StringLength(30,ErrorMessage ="El valor sobre pasa la cantidad maxima.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "Maximo permitido, 50 caracteres.")]
        public string Password { get; set; }
        [NotMapped]
        [StringLength(50, ErrorMessage = "Maximo permitido, 50 caracteres.")]
        [Compare("Password", ErrorMessage = "El password no coincide.")]
        [ScaffoldColumn(true)]
        public string ConfirmarPassword { get; set; }
    }
}