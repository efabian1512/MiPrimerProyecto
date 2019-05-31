using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin.Roles
{
    public class RolesServices
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<SelectListItem> ObtenerRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var RolesList = roleManager.Roles;


            List<SelectListItem> ListaDeRoles = new List<SelectListItem>();

            foreach (var rol in RolesList)
            {
                
                var role =new SelectListItem()
                {
                    Text = rol.Name,
                    Value = rol.Id
                   
                };
                ListaDeRoles.Add(role);
                
            }

            return ListaDeRoles;
       

            
        }

        public List<IdentityRole> Roles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles= roleManager.Roles.ToList();
            return roles;

        }
    }
            

           

    
    }

    
