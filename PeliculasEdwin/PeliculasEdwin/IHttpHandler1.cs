#region Assembly System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.Web.dll
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Web;

namespace PeliculasEdwin
{
    public interface IHttpHandler1
    {
       
            //
            // Summary:
            //     Gets a value indicating whether another request can use the System.Web.IHttpHandler
            //     instance.
            //
            // Returns:
            //     true if the System.Web.IHttpHandler instance is reusable; otherwise, false.
            bool IsReusable { get; }

            //
            // Summary:
            //     Enables processing of HTTP Web requests by a custom HttpHandler that implements
            //     the System.Web.IHttpHandler interface.
            //
            // Parameters:
            //   context:
            //     An System.Web.HttpContext object that provides references to the intrinsic server
            //     objects (for example, Request, Response, Session, and Server) used to service
            //     HTTP requests.
            void ProcessRequest(HttpContext context, string Id);
        }
    }

