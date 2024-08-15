using Core.DTO;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIGOPLAN
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Database.SetInitializer<MainContext>(null);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            
            //if (System.Web.HttpContext.Current.Session != null)
            //{
            //    if (vSesiones.sesionServerTime != null)
            //        vSesiones.sesionUpdateSession = vSesiones.sesionServerTime;

            //    vSesiones.sesionServerTime = DateTime.Now;
            //}
            if (System.Web.HttpContext.Current.Session != null)
            {
                if (Session["serverTime"] != null)
                    Session["updateSession"] = Session["serverTime"];
  
                Session["serverTime"] = DateTime.Now;
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            //vSesiones.sesionUpdateSession = DateTime.Now;
            System.Web.HttpContext.Current.Session["updateSession"] = DateTime.Now;
        }
    }
}
