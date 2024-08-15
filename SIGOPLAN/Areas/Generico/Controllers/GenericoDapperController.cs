using Core.DAO.GenericoDapper;
using Data.Factory.GenericoDapper;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Generico.Controllers
{
    public class GenericoDapperController : BaseController
    {
        IGenericoDapper DapperDAO;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DapperDAO = new GenericoDapperFactoryServices().GenericoDapperDAO();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ActivarDesactivar(string tabla,bool ActDesc,int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var select = DapperDAO.ActivarDesactivar(tabla,ActDesc,id);
                result.Add(ITEMS, select);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }
        public ActionResult obtenerConsultaEnkontrol()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var select = DapperDAO.obtenerConsultaEnkontrol();
                result.Add(ITEMS, select);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            return json;
        }
        

    }
}
