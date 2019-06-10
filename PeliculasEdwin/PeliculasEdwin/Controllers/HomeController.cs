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
        [Authorize(Roles = "Usuario,Editor,Administrador")]
        public ActionResult Index()
        {
            var ModeloPeliculas = db.PeliculasEdwin.ToList();
            return View(ModeloPeliculas);
        }
        //Método para filtrar películas por título
        
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
        [Authorize(Roles = "Editor,Administrador")]
        
        [HttpGet]
        public ActionResult RegistrarPelicula()
        {
            return View();
        }
        //Método post de la vista donde se registran las películas
        [Authorize(Roles = "Editor,Administrador")]
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
                //string fileNameVideo = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeVideo.FileName);
                //string extensionVideo = Path.GetExtension(pelicula.ArchivoDeVideo.FileName);
                //fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extensionVideo;
                //pelicula.RutaDeVideo = "~/Video/" + fileNameVideo;
                //fileNameVideo = Path.Combine(Server.MapPath("~/Video/"), fileNameVideo);
                //pelicula.ArchivoDeVideo.SaveAs(fileNameVideo);


                db.PeliculasEdwin.Add(pelicula);


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Clear();
            return View();
        }
        public ActionResult CargarVideo()
        {
            //var pelicula = new PeliculaViewModel();
            var modelo = new PeliculasService();
            modelo.ObtenerPeliculas();
            ViewBag.Peliculas = modelo.ObtenerPeliculas1();
            ViewBag.Partial = "";
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarVideo(PeliculaViewModel pelicula)
        {
            if (!ModelState.IsValid)
            {
                if (pelicula.Id.Equals(0))
                {
                    return new JsonResult { Data = "Debe selecionar una pelicula." };
                }
                if(pelicula.ArchivoDeVideo==null)
                {
                    return new JsonResult { Data = "Debe selecionar un arhico de video." };
                }
                
                //var modelo = new PeliculasService();
                //modelo.ObtenerPeliculas();
                //ViewBag.Peliculas = modelo.ObtenerPeliculas1();
                //return View();

            }
            System.Threading.Thread.Sleep(1500);
            var pelicula1 = db.PeliculasEdwin.Where(x => x.Id == pelicula.Id).FirstOrDefault();
            //string fileNameVideo = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeVideo.FileName);
            //string extensionVideo = Path.GetExtension(pelicula.ArchivoDeVideo.FileName);
            //fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extensionVideo;
            //pelicula1.RutaDeVideo = "~/Video/" + fileNameVideo;
            //fileNameVideo = Path.Combine(Server.MapPath("~/Video/"), fileNameVideo);
            //pelicula1.ArchivoDeVideo.SaveAs(fileNameVideo);
           if(pelicula.ArchivoDeVideo !=null && pelicula.ArchivoDeVideo.ContentLength > 0) {
            string fileNameVideo = Path.GetFileNameWithoutExtension(pelicula.ArchivoDeVideo.FileName);
            string extensionVideo = Path.GetExtension(pelicula.ArchivoDeVideo.FileName);
            fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extensionVideo;
            pelicula1.RutaDeVideo = "~/Video/" + fileNameVideo;
            fileNameVideo = Path.Combine(Server.MapPath("~/Video/"), fileNameVideo);
            pelicula.ArchivoDeVideo.SaveAs(fileNameVideo);
           
            db.Entry(pelicula1).State = EntityState.Modified;
            db.SaveChanges();
            }
            //return RedirectToAction("Index");
            ViewBag.Partial = "_AgregarMasVideosPartial";
            //var pelicula1 = db.PeliculasEdwin.Where(x => x.Id == pelicula.Id).FirstOrDefault();
            return new JsonResult { Data = "Su archivo de video se cargo de manera satisfactoria." };
            //if (context.Request.Files.Count > 0)
            //{
            //    HttpFileCollection files = context.Request.Files;
            //    for (int i = 0; i < files.Count; i++)
            //    {
            //        System.Threading.Thread.Sleep(1000);
            //        pelicula1.ArchivoDeVideo = files[i];
            //        string fileNameVideo = Path.GetFileNameWithoutExtension(pelicula1.ArchivoDeVideo.FileName);
            //        string extensionVideo = Path.GetExtension(pelicula.ArchivoDeVideo.FileName);
            //        fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extensionVideo;
            //        pelicula.RutaDeVideo = "~/Video/" + fileNameVideo;
            //        fileNameVideo = Path.Combine(context.Server.MapPath("~/Video/"), fileNameVideo);

            //        //string fileName = context.Server.MapPath("~/Video/" + Path.GetFileNameWithoutExtension(pelicula1.ArchivoDeVideo.FileName));
            //        pelicula1.ArchivoDeVideo.SaveAs(fileNameVideo);
            //        db.Entry(pelicula1).State = EntityState.Modified;
            //        db.SaveChanges();

            //    }
            //}
           
            
            //pelicula.RutaDeVideo = context.Server.MapPath("~/Video/" + Path.GetFileName(context.FileName));
            //db.Entry(pelicula).State = EntityState.Modified;
            //db.SaveChanges();
           // return RedirectToAction("Index");

        }
        //Método get de la vista que presenta los detalles de las películas y permite reproducirlas
        [Authorize(Roles = "Usuario,Editor,Administrador")]
        [HttpGet]
        public ActionResult Ver([Bind(Include = "Id")]Pelicula peliculaDetalles)
        {

            var ModeloPelicula = db.PeliculasEdwin.Include("Comentarios").Where(x => x.Id == peliculaDetalles.Id).FirstOrDefault();
            return View(ModeloPelicula);
        }
        //Método get de la vsta para editar películas
        [Authorize(Roles = "Editor,Administrador")]
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
        [Authorize(Roles = "Editor,Administrador")]
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