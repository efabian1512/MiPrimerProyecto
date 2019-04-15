using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            //Pelicula prueba = new Pelicula()
            //{
            //    Título = "Avengers",
            //    Duración = "3 hrs",
            //    Año = "2018",
            //    Género = "Accion",
            //    Sinopsis = "Pelicula se super herores.",
            //    EnCarTelera = false,
            //    País = "USA"
            //};

            //db.PeliculasEdwin.Add(prueba);
            //db.SaveChanges();
            //var prueba1 = db.PeliculasEdwin.Where(x => x.Id == 1).FirstOrDefault();
            var ModeloPeliculas =db.PeliculasEdwin.ToList();
            return View(ModeloPeliculas);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult RegistrarPelicula()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarPelicula([Bind(Include = "Id,Título,Género,Duración,País,Año,EnCarTelera,Sinopsis,ArchivoDeImagen")]Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeImagen.FileName);
                string extension = Path.GetExtension(pelicula.ArchivoDeImagen.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                pelicula.RutaDeImagen = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                pelicula.ArchivoDeImagen.SaveAs(fileName);
                db.PeliculasEdwin.Add(pelicula);
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            ModelState.Clear();
            return View();
        }
        [HttpGet]
        public ActionResult Ver([Bind(Include ="Id")]Pelicula peliculaDetalles)
        {

           var  ModeloPelicula = db.PeliculasEdwin.Where(x => x.Id == peliculaDetalles.Id).FirstOrDefault();
            return View(ModeloPelicula);
        }
    }
}