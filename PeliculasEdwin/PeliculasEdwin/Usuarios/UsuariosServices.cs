using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin.Usuarios
{
    public class UsuariosServices
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<SelectListItem> ObtenerUsuarios()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
           var usuarios = userManager.Users;

            List<SelectListItem> ListadoDeUsuarios = new List<SelectListItem>();
            foreach(var user in usuarios)
            {
               
                    var usuario = new SelectListItem()
                    {

                        Text = user.UserName,
                        Value = user.Id
                        
                    
                    };
                    ListadoDeUsuarios.Add(usuario);
               
                
               
            }
            return ListadoDeUsuarios;

        }

        public List<ApplicationUser> Users()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var usuarios = userManager.Users.ToList();
            return usuarios;

        }
    }
}