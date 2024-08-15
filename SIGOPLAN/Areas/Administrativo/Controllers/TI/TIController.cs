using Core.DAO.Administracion.TI;
using Data.Factory.Administracion.TI;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.TI
{
    public class TIController : BaseController
    {
        #region INIT
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        public ITIDAO _TI_FACTORY_SERVICE;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _TI_FACTORY_SERVICE = new TIFactoryService().GetTI();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region RETURN VIEWS
        public ActionResult Soluciones()
        {
            return View();
        }
        #endregion
    }
}