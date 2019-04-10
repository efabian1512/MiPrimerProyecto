using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models
{
    public class Pelicula
    {



        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {} es requerido")]
        public string Título { get; set; }
        [StringLength(20, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {} es requerido.")]
        public string Género { get; set; }
        [StringLength(10, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {} es requerido.")]

        public string Duración { get; set; }

        [StringLength(20, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        public string País { get; set; }

        [Display(Name = "En Cartelera")]
        [Required(ErrorMessage = "El campo {} es requerido.")]

        public bool EnCarTelera { get; set; }
        [StringLength(500, ErrorMessage = "Ha sobrepasado el maximo de caracteres permitidos.")]
        [Required(ErrorMessage = "El campo {} es requerido.")]
        [DataType(DataType.MultilineText)]
        public string Sinopsis { get; set; }

    }
}
