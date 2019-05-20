﻿using PeliculasEdwin.Models;
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
        [HttpGet]
        public ActionResult Index()
        {
            //var estudiantes = db.EstudiantePrueba.ToList();
            //var estudiante1 = new EstudiantePrueba() { Nombre = "Edwin Fabian", semestre = 1 };
            //var estudiante2 = new EstudiantePrueba() { Nombre = "Ramon Fabian", semestre = 2 };
            //var estudiante3 = new EstudiantePrueba() { Nombre = "Frankely Fabian", semestre = 3 };
            //db.EstudiantePrueba.Add(estudiante1);
            //db.EstudiantePrueba.Add(estudiante2);
            //db.EstudiantePrueba.Add(estudiante3);
            //db.SaveChanges();

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
            var ModeloPeliculas = db.PeliculasEdwin.ToList();
            return View(ModeloPeliculas);
        }
        public ActionResult Estudiantes()
        {
            var modelo = db.PeliculasEdwin.ToList();
            return View(modelo);
        }
        public JsonResult BuscandoEstudiantes(string ValorBusqueda)
        {
            var estudiantes = db.PeliculasEdwin.Where(x => x.Título.Contains(ValorBusqueda) || ValorBusqueda == null).ToList();

            return Json(estudiantes, JsonRequestBehavior.AllowGet);

        }
        
        //public ActionResult BuscandoPeliculas(string ValorBusqueda)
        
        public JsonResult BuscandoPeliculas(string ValorBusqueda)
        {
            var modelo = db.PeliculasEdwin.Where(x => x.Título.Contains(ValorBusqueda) || ValorBusqueda == null).ToList();
            return Json(modelo, JsonRequestBehavior.AllowGet);
            
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index([Bind(Include = "Título")]Pelicula pelicula)
        //{ 
        ////public ActionResult Index(string term)
        ////{
        //    var modelo = db.PeliculasEdwin.Where(x => x.Título.Contains(pelicula.Título)).ToList();
        //    //db.Dispose();
        //    return View(modelo);

        //}
        public ActionResult Pruebas()
        {
            return View();
        }
       // [HttpPost]
       //// [ValidateAntiForgeryToken]
       // public JsonResult BuscarPeliculas(string valorBusqueda)
       // {
       //     var pelicula =db.PeliculasEdwin.Where(x => x.Título.Contains(valorBusqueda)).FirstOrDefault();
       //     return Json(pelicula.Título);
       // }
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
                fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extensionVideo;
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
        public ActionResult Ver([Bind(Include = "Id")]Pelicula peliculaDetalles)
        {

            var ModeloPelicula = db.PeliculasEdwin.Include("Comentarios").Where(x => x.Id == peliculaDetalles.Id).FirstOrDefault();
            return View(ModeloPelicula);
        }
        [HttpGet]
        public ActionResult Editar(int? id)
        {
            if (id.Equals(null))
            {
                return RedirectToAction("Index");
            }
            var pelicula = db.PeliculasEdwin.Where(x => x.Id == id).FirstOrDefault();

            return View(pelicula);
        }


        [HttpPost]
        public ActionResult Editar([Bind(Include = "Id,Título,Género,Duración,País,Año,EnCarTelera,Sinopsis,ArchivoDeImagen,RutaDeImagen")]Pelicula pelicula)
        {
            if (pelicula.ArchivoDeImagen != null)
            {

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
        //[HttpPost]
        //public ActionResult Comentarios([Bind(Include ="Contenido")] Comentario comentario, int idPelicula)
        //{
        //    var pelicula = db.PeliculasEdwin.Where(x => x.Id == idPelicula).FirstOrDefault();
        //    comentario.Pelicula = pelicula;
        //    var datos =  db.Comentarios.Add(comentario);
        //    db.SaveChanges();
        //    var PeliculaInfo = comentario.Pelicula;
        //    return View("Ver", PeliculaInfo);

        //}
        [HttpPost]
        public JsonResult Comentarios([Bind(Include = "Contenido")] Comentario comentario, int idPelicula)
        {
            var pelicula = db.PeliculasEdwin.Where(x => x.Id == idPelicula).FirstOrDefault();
            comentario.Pelicula = pelicula;
            var datos = db.Comentarios.Add(comentario);

            db.SaveChanges();
            var comentarios = db.Comentarios.Where(x => x.Id == idPelicula).ToList();
            var PeliculaInfo = comentario.Pelicula;

            //return Json(comentarios);
            return Json(comentario.Contenido);

        }
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var pelicula = db.PeliculasEdwin.Where(x => x.Id == id).FirstOrDefault();

            return View(pelicula);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int? id)
        {
            Pelicula pelicula = db.PeliculasEdwin.Find(id);
            db.PeliculasEdwin.Remove(pelicula);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult BuscarPelicula(string valorBusqueda)
        //{
        //    var modelo = db.PeliculasEdwin.Where(x => x.Título.Contains(valorBusqueda)).ToList();
        //    //db.Dispose();
        //    return View(modelo);


        //}
    }
}