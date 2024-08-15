using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Componentes
{
    public class CatLocacionesController : BaseController
    {


        // GET: CatLocaciones
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getDataTable(string descripcion, bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
               // var tblRes = null;

                result.Add("tblRes", null);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDataRegistro(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate(dynamic obj, int accion)
        {
            var result = new Dictionary<string, object>();
            try
            {

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