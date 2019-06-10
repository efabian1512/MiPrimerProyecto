using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    public class PeliculaViewModel
    {
        [Required(ErrorMessage ="Debe seleccionar una pelicula.")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Debe Seleccionar un archivo de video.")]
        public HttpPostedFileBase ArchivoDeVideo { get; set; }
        //public  ArchivoDeVideo { get; set; }
        //[Display(Name = "Archivo de Pelicula")]
        //public string RutaDeVideo { get; set; }
    }

        
}