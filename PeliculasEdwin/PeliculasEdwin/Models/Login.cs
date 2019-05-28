using PeliculasEdwin.Models.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    public class Login
    {
        [ValidarCantidadCaracteresUsuario]
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
    }
}