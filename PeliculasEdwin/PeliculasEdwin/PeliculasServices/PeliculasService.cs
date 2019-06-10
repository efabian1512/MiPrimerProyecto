using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin.PeliculasServices
{
    
    public class PeliculasService
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<Pelicula> ObtenerPeliculas()
        {
            var peliculas = db.PeliculasEdwin.ToList();
            return peliculas;
        }

        public List<SelectListItem> ObtenerPeliculas1()
        {
            var peliculas = db.PeliculasEdwin.Where(x=>x.RutaDeVideo==null).ToList();
        

            List<SelectListItem> ListadoDePeliculas= new List<SelectListItem>();
            foreach (var pelicula in peliculas)
            {

                var pelicula1 = new SelectListItem()
                {

                    Text = pelicula.Título,
                    Value = pelicula.Id.ToString()


                };
                ListadoDePeliculas.Add(pelicula1);



            }
            return ListadoDePeliculas;

        }


    }
}