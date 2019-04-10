using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
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
            return View();
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

        public ActionResult RegistrarPelicula([Bind(Include ="Id,Título,Género,Duración,País,EnCarTelera,Sinopsis")]Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                db.PeliculasEdwin.Add(pelicula);
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            return View();
        }
    }
}