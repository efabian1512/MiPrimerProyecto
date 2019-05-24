using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Registrar()
        {
            var usuarios = db.Usuarios.ToList();
            
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "Id,Nombre,Apellidos,NombreUsuario,CorreoElectronico,Telefono,Password")] Usuario usuario)
        {
            if (ModelState.IsValid) {
            db.Usuarios.Add(usuario);
            db.SaveChanges();

            }
            return RedirectToAction("Index","Home");
            

        }

    }
}
