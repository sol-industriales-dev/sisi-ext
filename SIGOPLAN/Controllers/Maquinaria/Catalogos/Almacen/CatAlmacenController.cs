using Core.Entity.Maquinaria.Catalogo;
using Data.DAO.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Catalogos.Almacen
{
    public class CatAlmacenController : BaseController
    {
        #region Atributos
        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        #endregion

        #region Factory
        AlmacenFactoryServices almacenFactoryServices;

        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            almacenFactoryServices = new AlmacenFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        // GET: CatObra
        public ActionResult Index()
        {
            ViewBag.pagina = "catalogo";
            return View();
        }

        public ActionResult FillGridAlmacen(tblM_Almacen obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                AlmacenDAO almacen = new AlmacenDAO();
                var listResult = almacen.FillGridAlmacen(obj);

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                result.Add("rows", listResult);
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