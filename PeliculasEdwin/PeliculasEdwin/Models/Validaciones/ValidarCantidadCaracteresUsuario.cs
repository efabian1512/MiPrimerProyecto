using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Models.Validaciones
{
    public class ValidarCantidadCaracteresUsuario: ValidationAttribute
    {
       

        ApplicationDbContext db;

        public ValidarCantidadCaracteresUsuario()
        :base("El {0} es invalido.")
        {
           
            db = new ApplicationDbContext();



        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cadena = (string)value;
            var mensajeDeError = FormatErrorMessage(validationContext.DisplayName);
            if(value != null)
            {
                if (cadena.Length < 6) { 
               
                    return new ValidationResult(mensajeDeError);
               }
               
            }
            return ValidationResult.Success;

            //return base.IsValid(value, validationContext);
        }
    }
}