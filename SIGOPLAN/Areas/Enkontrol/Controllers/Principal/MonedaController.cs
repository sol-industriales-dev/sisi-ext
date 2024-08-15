using Data.Factory.Enkontrol.Principal;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Principal.Moneda
{
    public class MonedaController : BaseController
    {
        MonedaFactoryService mfs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            mfs = new MonedaFactoryService();
            base.OnActionExecuting(filterContext);
        }
        // GET: Enkontrol/Principal
        public ActionResult _mdlGuardar()
        {
            return PartialView();
        }
        public ActionResult guardarTC(int moneda, decimal tc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                mfs.getMonedaService().guardarTC(moneda, tc);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult isUsuarioCambiarTC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("isUsuarioCambiarTC", mfs.getMonedaService().isUsuarioCambiarTC());
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region combobox
        public ActionResult FillComboMonedaHoy()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, mfs.getMonedaService().FillComboMonedaHoy());
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}