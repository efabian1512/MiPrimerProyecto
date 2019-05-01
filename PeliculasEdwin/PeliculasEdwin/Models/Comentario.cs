using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    public class Comentario
    {
        
        public int Id { get; set; }
        public string Contenido { get; set; }
        public Pelicula Pelicula { get; set; }

    }
}