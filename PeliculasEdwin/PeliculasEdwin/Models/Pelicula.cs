using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    //Modelo pelicula
    public class Pelicula
    {


        //DEclaracion de propiedades modelo Pelicula
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Título { get; set; }
        [StringLength(20, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Género { get; set; }
        [StringLength(10, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]

       

        public string Duración { get; set; }

        [StringLength(20, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        public string País { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(8)]
        public string Año { get; set; }

        [Display(Name = "En Cartelera")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        
        public bool EnCarTelera { get; set; }
        [StringLength(500, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.MultilineText)]
        public string Sinopsis { get; set; }
        [Display(Name ="Portada")]
        public string RutaDeImagen { get; set; }
        [ScaffoldColumn(false)]
        [NotMapped]
        [Display(Name ="Cambiar Imagen de Portada")]
        public HttpPostedFileBase ArchivoDeImagen { get; set; }
        [Display(Name = "Archivo de Pelicula")]
        public string RutaDeVideo { get; set; }

        [ScaffoldColumn(false)]
        [NotMapped]
        [Display(Name = "Cambiar Video")]
        //public HttpPostedFileBase ArchivoDeVideo { get; set; }
        public HttpPostedFileBase ArchivoDeVideo { get; set; }

        public List<Comentario> Comentarios { get; set; }



    }
}
