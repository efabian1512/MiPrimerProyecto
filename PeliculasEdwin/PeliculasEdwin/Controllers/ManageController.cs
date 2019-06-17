using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PeliculasEdwin.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using PeliculasEdwin.Roles;
using PeliculasEdwin.Usuarios;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Helpers;
using System.Data.Entity;

namespace PeliculasEdwin.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ActionResult prueba()
        {
            return View();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult CrearRol()
        {
            return View();
        }
        ApplicationDbContext db = new ApplicationDbContext();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearRol(CrearRolVieModel rol)
        {
            if (ModelState.IsValid)
            {

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                roleManager.Create(new IdentityRole(rol.Nombre));
                return RedirectToAction("Index", "Manage");
            }

            return View();
        }
        [HttpGet]
        public ActionResult AsignarRol()
        {
            var roles = new RolesServices();
            var usuarios = new UsuariosServices();
            ViewBag.ListaDoDeRoles = roles.ObtenerRoles();
            ViewBag.ListaDoDeUsuarios = usuarios.ObtenerUsuarios();
            ViewBag.Partial = "";
            return View();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarRol(AsignarRolViewModel informacion)
        {
            var Resultado = new BaseRespuesta();

            if (!ModelState.IsValid)
            {
                var rol = new RolesServices();
                var user = new UsuariosServices();
                ViewBag.ListaDoDeRoles = rol.ObtenerRoles();
                ViewBag.ListaDoDeUsuarios = user.ObtenerUsuarios();
                ViewBag.Partial = "";
                return View();
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));



            foreach (var i in informacion.roles)
            {
                var rol1 = roleManager.FindById(i);
                userManager.AddToRole(informacion.usuario, rol1.Name);
            }

            ViewBag.Mensaje = "Roles asignados correctamente.";
            var roles = new RolesServices();
            var usuarios = new UsuariosServices();
            ViewBag.ListaDoDeRoles = roles.ObtenerRoles();
            ViewBag.ListaDoDeUsuarios = usuarios.ObtenerUsuarios();
            ViewBag.Partial = "_AgregarMasRolesPartial";

            return View();
        }

        [HttpGet]
        public ActionResult DesAsignarRol()
        {
            var roles = new RolesServices();
            var usuarios = new UsuariosServices();
            ViewBag.ListaDoDeRoles = roles.ObtenerRoles();
            ViewBag.ListaDoDeUsuarios = usuarios.ObtenerUsuarios();
            ViewBag.Partial = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DesAsignarRol(DesasignarRolViewModel informacion)
        {
            //var Resultado = new BaseRespuesta();

            if (!ModelState.IsValid)
            {
                var rol = new RolesServices();
                var user = new UsuariosServices();
                ViewBag.ListaDoDeRoles = rol.ObtenerRoles();
                ViewBag.ListaDoDeUsuarios = user.ObtenerUsuarios();
                ViewBag.Partial = "";
                return View();
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            //var modelo = new ConfirmarDesAsignacionRolViewModel();
            //modelo.NombreRoles = new List<string>();
            //modelo.IdRol = new List<string>();

            foreach (var i in informacion.roles)
            {
                var rol1 = roleManager.FindById(i);
                userManager.RemoveFromRole(informacion.usuario, rol1.Name);

            }
            //modelo.usuario = informacion.usuario;
            //var user1 = userManager.FindById(informacion.usuario);
            //modelo.NombreUsuario = user1.UserName;
            //foreach (var i in informacion.roles)
            //{

            //    var rol1 = roleManager.FindById(i);
            //    modelo.NombreRoles.Add(rol1.Name);
            //    modelo.IdRol.Add(rol1.Id);

            //    //userManager.RemoveFromRole(informacion.usuario, rol1.Name);

            //}

            ViewBag.Mensaje = "Roles desasignados correctamente.";
            var roles = new RolesServices();
            var usuarios = new UsuariosServices();
            ViewBag.ListaDoDeRoles = roles.ObtenerRoles();
            ViewBag.ListaDoDeUsuarios = usuarios.ObtenerUsuarios();
            ViewBag.Partial = "_RetirarMasRolesPartial";


            //return View("ConfirmarDesAsignacionRol",modelo);
            return View();
            //RouteValueDictionary Prueba1 = new RouteValueDictionary();

            //foreach(var rolId in modelo.IdRol)
            //{
            //    Prueba1.Add(rolId, Request.QueryString["rolId"]);
            //}
            // RouteValueDictionary routeData = new RouteValueDictionary();
            //routeData.Add("IdRol", Prueba1);
            //routeData.Add("NombreRoles", modelo.NombreRoles);
            //routeData.Add("NombreUsuario", modelo.NombreUsuario);
            //routeData.Add("usuario", modelo.usuario);

            //return RedirectToAction("ConfirmarDesAsignacionRol", routeData);
            


        }

        public ActionResult Usuarios()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var usuarios = db.Users.ToList();
           
            List<RegisterViewModel> variable = new List<RegisterViewModel>(usuarios.Count);
            
            var roles = db.Roles.ToList();


            for (int i = 0; i < usuarios.Count; i++)
            {
                variable.Add(new RegisterViewModel()
                {
                    Id = usuarios[i].Id,
                    NombreUsuario = usuarios[i].UserName,
                    Nombre = usuarios[i].Nombre

                });
                
            }
            for (int i = 0; i < variable.Count; i++) { 
                variable[i].NombreRoles = new List<string>();
            }
            for (int i = 0; i < variable.Count; i++) { 
                foreach (var rol in roles) {
                //foreach (var user in usuarios)
                //{
                    if (userManager.IsInRole(variable[i].Id, rol.Name))
                    {
                        variable[i].NombreRoles.Add(rol.Name);
                    }

            //}
            }
            }



            //foreach (var usuario in usuarios)
            for(int i=0;i<variable.Count;i++)
            {
               var UserId= User.Identity.GetUserId();
                if (UserId == variable[i].Id)
                {
                   // var user = userManager.FindById(usuarios[i].Id);
                    variable[i].Estado = true;
                    //db.Entry(user).State = EntityState.Modified;
                    //db.SaveChanges();

                }
                
                    //var user1 = UserManager.FindById(usuario.Id);
                    //user1.Estado = false;
                    //db.Entry(user1).State = EntityState.Modified;
                    //db.SaveChanges();

                
            }
            //User.Identity.IsAuthenticated

            return View(variable);
        }

        [HttpGet]
        public ActionResult EliminarUsuario(string Id)
        {
            var usuarioEliminar=db.Users.Where(x => x.Id == Id).FirstOrDefault();
           

            return View(usuarioEliminar);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario1(string Id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var usuario = db.Users.Where(x => x.Id == Id).FirstOrDefault();
            userManager.Delete(usuario);
            ViewBag.Mensaje = "Usuario eliminado correctamente.";
            ViewBag.Partial = "_EliminarUsuarioPartial";
            return View();
        }
        //[HttpGet]
        //public ActionResult ConfirmarDesAsignacionRol(ConfirmarDesAsignacionRolViewModel modelo)
        ////public ActionResult ConfirmarDesAsignacionRol(string usuario,string IdRol, string NombreRoles,string NombreUsuario)
        // { 
        //    ViewBag.Partial = "";
        //    return View(modelo);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ConfirmarDesAsignacionRol(string usuario, List<string> roles)
        //{
        //    ViewBag.Mensaje = "Roles desasignados correctamente.";
        //    //var roles = new RolesServices();
        //    //var usuarios = new UsuariosServices();
        //    //ViewBag.ListaDoDeRoles = roles.ObtenerRoles();
        //    //ViewBag.ListaDoDeUsuarios = usuarios.ObtenerUsuarios();

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

        //    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //    foreach (var i in roles)
        //    {
        //        var rol1 = roleManager.FindById(i);
        //        userManager.RemoveFromRole(usuario, rol1.Name);

        //    }

        //    ViewBag.Partial = "_RetirarMasRolesPartial";
        //    return View();
        //}

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }


        //foreach (var i in informacion.roles)
        //{
        //    var rol1 = roleManager.FindById(i);
        //    userManager.RemoveFromRole(informacion.usuario, rol1.Name);

        //}


        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}