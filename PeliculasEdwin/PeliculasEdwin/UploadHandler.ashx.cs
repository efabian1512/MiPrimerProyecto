using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PeliculasEdwin
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    //public class UploadHandler : IHttpHandler
        public class UploadHandler : IHttpHandler1
    {

        public void ProcessRequest(HttpContext context, string Id)
        {
            //var PeliculaId = int.Parse(Id);
            ApplicationDbContext db = new ApplicationDbContext();
            if (context.Request.Files.Count > 0)
            {
                //var pelicula = db.PeliculasEdwin.Where(x => x.Id == PeliculaId).FirstOrDefault();
                HttpFileCollection files = context.Request.Files;
                for(int i = 0; i < files.Count; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    HttpPostedFile file = files[i];
                    string fileNameVideo = Path.GetFileNameWithoutExtension(file.FileName);
                    string extensionVideo = Path.GetExtension(file.FileName);
                    fileNameVideo = fileNameVideo + DateTime.Now.ToString("yymmssff") + extensionVideo;
                    //pelicula.RutaDeVideo = "~/Video/" + fileNameVideo;
                    fileNameVideo = Path.Combine(context.Server.MapPath("~/Video/"), fileNameVideo);
                    //string fileName = context.Server.MapPath("~/Video/" +Path.GetFileName(file.FileName));
                    file.SaveAs(fileNameVideo);

                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}