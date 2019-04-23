using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            //var pelicula = db.PeliculasEdwin.Where(x => x.Id == 1).FirstOrDefault();
            ////var pelicula1 = db.PeliculasEdwin.Where(x => x.Id == 2).FirstOrDefault();
            //pelicula.RutaDeImagen = "~/Images/268x0w192139811.png";
            //pelicula1.RutaDeImagen = "~/Images/A1t8xCe9jwL._SY679_190159757.jpg";
            //db.SaveChanges();
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
        public ActionResult RegistrarPelicula([Bind(Include = "Id,Título,Género,Duración,País,Año,EnCarTelera,Sinopsis,ArchivoDeImagen,ArchivoDeVideo")]Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeImagen.FileName);
                string extension = Path.GetExtension(pelicula.ArchivoDeImagen.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                pelicula.RutaDeImagen = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                pelicula.ArchivoDeImagen.SaveAs(fileName);
                string fileNameVideo = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeVideo.FileName);
                string extensionVideo = Path.GetExtension(pelicula.ArchivoDeVideo.FileName);
                fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extension;
                pelicula.RutaDeVideo = "~/Video/" + fileNameVideo;
                fileNameVideo = Path.Combine(Server.MapPath("~/Video/"), fileNameVideo);
                pelicula.ArchivoDeVideo.SaveAs(fileNameVideo);


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
        [HttpGet]
        public ActionResult Editar(int ?id )
        {
            if (id.Equals(null)) {
                return RedirectToAction("Index");
            }
            var pelicula = db.PeliculasEdwin.Where(x => x.Id == id).FirstOrDefault();

            return View(pelicula);
        }
       

        [HttpPost]
        public ActionResult Editar([Bind(Include = "Id,Título,Género,Duración,País,Año,EnCarTelera,Sinopsis,ArchivoDeImagen,RutaDeImagen")]Pelicula pelicula)
        {
            if (pelicula.ArchivoDeImagen != null) {

                string fileName = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeImagen.FileName);
                string extension = Path.GetExtension(pelicula.ArchivoDeImagen.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                pelicula.RutaDeImagen = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                pelicula.ArchivoDeImagen.SaveAs(fileName);
               
                db.Entry(pelicula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            db.Entry(pelicula).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}