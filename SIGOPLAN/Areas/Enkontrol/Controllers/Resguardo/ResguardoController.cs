using Data.Factory.Enkontrol.Resguardo;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Resguardo
{
    public class ResguardoController : BaseController
    {
        private ResguardoFactoryService resguardoFactoryService;
        //private UsuarioFactoryServices ufs;
        Dictionary<string, object> result;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resguardoFactoryService = new ResguardoFactoryService();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Asignacion()
        {
            return View();
        }


    }
}