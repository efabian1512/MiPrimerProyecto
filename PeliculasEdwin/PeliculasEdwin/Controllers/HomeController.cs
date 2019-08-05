using PeliculasEdwin.Models;
using PeliculasEdwin.PeliculasServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin.Controllers
{
    //[Authorize(Roles="Usuario,Editor,Administrador")]
    public class HomeController : Controller
    {
        //Declaración objeto dbcontext
        ApplicationDbContext db = new ApplicationDbContext();

        //Método get del index donde se cargan todas las películas
        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "Usuario,Editor,Administrador")]
        public ActionResult Index()
        {
            AnoServices year = new AnoServices();
            ViewBag.Ano = year.ObtenerAno();
            ViewBag.Inicio = " ";
            var ModeloPeliculas = db.PeliculasEdwin.ToList();
            return View(ModeloPeliculas);
        }
        //Método para filtrar películas por título
        public ActionResult Index1()
        {
           
            var ModeloPeliculas = db.PeliculasEdwin.ToList();
            return View(ModeloPeliculas);
        }
         public ActionResult BuscarPorAno(string ano)
        {
            var modelo = db.PeliculasEdwin.Where(x => x.Año == ano).ToList();
            return Json(modelo,JsonRequestBehavior.AllowGet);

        }

        public ActionResult BuscarGenero(string Genero)
        {
            
            var modelo1 = db.PeliculasEdwin.ToList();
            var modelo = db.PeliculasEdwin.Where(x => x.Género == Genero).ToList();
            if (modelo.Count > 0)
            {
                ViewBag.Genero = "Películas de " + Genero;
                ViewBag.Inicio = "_Inicio_Partial";
            }else
            {
                ViewBag.Genero = "No hubo ninguna coincidencia.";
                ViewBag.Inicio = "_Inicio_Partial";
                return View("Index",modelo1);
            }
           
            return View("Index",modelo);

        }
        public JsonResult BuscandoPeliculas(string ValorBusqueda)
        {
            var modelo = db.PeliculasEdwin.Where(x => x.Título.Contains(ValorBusqueda) || ValorBusqueda == null).ToList();
            return Json(modelo, JsonRequestBehavior.AllowGet);

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
        //Método get de la vista donde se registran las películas
        //[Authorize(Roles = "Editor,Administrador")]
        
       
        //Método get de la vista que presenta los detalles de las películas y permite reproducirlas
       // [Authorize(Roles = "Usuario,Editor,Administrador")]
        [HttpGet]
        public ActionResult Ver([Bind(Include = "Id")]Pelicula peliculaDetalles)
        {

            var ModeloPelicula = db.PeliculasEdwin.Include("Comentarios").Where(x => x.Id == peliculaDetalles.Id).FirstOrDefault();
            return View(ModeloPelicula);
        }
        //Método get de la vsta para editar películas
       // [Authorize(Roles = "Editor,Administrador")]//prueba
        [HttpGet]
        public ActionResult Editar(int? id)
        {
            // Si el id es nulo redirige a vista index (inicio)
            if (id.Equals(null))
            {
                return RedirectToAction("Index");
            }
            var pelicula = db.PeliculasEdwin.Where(x => x.Id == id).FirstOrDefault();

            return View(pelicula);
        }

        //Método post de la vista para editar películas
        //[Authorize(Roles = "Editor,Administrador")]
        [HttpPost]
        public ActionResult Editar([Bind(Include = "Id,Título,Género,Duración,País,Año,EnCarTelera,Sinopsis,ArchivoDeImagen,RutaDeImagen,ArchivoDeVideo,RutaDeVideo")]Pelicula pelicula)
        {
            //Si se cambió el archivo de imagen se procede a guardar datos nueva imagen
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
            //Si se cambio el video se guardan todos los datos del nuevo video
            if (pelicula.ArchivoDeVideo != null)
            {

                string fileName = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeVideo.FileName);
                string extension = Path.GetExtension(pelicula.ArchivoDeVideo.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                pelicula.RutaDeVideo = "~/Video/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Video/"), fileName);
                pelicula.ArchivoDeVideo.SaveAs(fileName);

                db.Entry(pelicula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            //Si no se cambia la imagen, se guardan los cambios realizados y se redirige  a index(inicio)
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
        //Comentarios (en construcción)

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
        // Método get para vista de eliminar peliculas
        [Authorize(Roles = "Editor,Administrador")]
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var pelicula = db.PeliculasEdwin.Where(x => x.Id == id).FirstOrDefault();

            return View(pelicula);
        }

        //Método post para eliminar
        [Authorize(Roles = "Editor,Administrador")]  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int? id)
        {
            Pelicula pelicula = db.PeliculasEdwin.Find(id);
            db.PeliculasEdwin.Remove(pelicula);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}