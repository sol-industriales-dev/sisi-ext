using Data.Factory.Kubrix;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Kubrix.Controllers
{
    public class AnalisisController : BaseController
    {
        #region Factory
        AnalisisFactoryServices AnalisisFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AnalisisFS = new AnalisisFactoryServices();
        }
        #endregion
        // GET: Kubrix/Analisis
        public ActionResult Division()
        {
            return View();
        }
        public ActionResult _tblObras()
        {
            return View();
        }
        public ActionResult getCboDivision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, AnalisisFS.getAnalissService().getCboDivision());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}