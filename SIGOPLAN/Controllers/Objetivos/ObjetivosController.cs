using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Objetivos
{
    public class ObjetivosController : BaseController
    {
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        #region Vistas
        public ActionResult Escritorio()
        {
            return View();
        }

        public ActionResult Calendario()
        {
            return View();
        }
        #endregion
    }
}