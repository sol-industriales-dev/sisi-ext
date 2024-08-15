using Core.DTO;
using Core.Entity.Maquinaria.Catalogo;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Maquinaria
{
    public class CatTiposController : BaseController
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
        TipoMaquinariaFactoryServices tipoMaquinariaFactoryServices;
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            tipoMaquinariaFactoryServices = new TipoMaquinariaFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: CatTipos
        public ActionResult Index()
        {

            var usuarioDTO = vSesiones.sesionUsuarioDTO;

            if (usuarioDTO != null)
            {
                ViewBag.pagina = "catalogo";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }

        }

        public ActionResult FillGrid_TipoMaquinaria(tblM_CatTipoMaquinaria tipoMaquinaria)
        {
            var result = new Dictionary<string, object>();
            var listResult = tipoMaquinariaFactoryServices.getTipoMaquinariaService().FillGridTipoMaquinaria(tipoMaquinaria).Select(x => new { id = x.id, descripcion = x.descripcion, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });

            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_TipoMaquinaria(tblM_CatTipoMaquinaria obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tipoMaquinariaFactoryServices.getTipoMaquinariaService().Guardar(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
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