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
    public class CatAseguradorasController : BaseController
    {


        #region Factory
        AseguradoraFactoryServices aseguradoraFactoryServices;
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            aseguradoraFactoryServices = new AseguradoraFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Aseguradoras
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
        public ActionResult FillGrid_Aseguradora(tblM_CatAseguradora Aseguradora)
        {
            var result = new Dictionary<string, object>();
            var listResult = aseguradoraFactoryServices.GetAseguradoraService().FillGridAseguradora(Aseguradora).Select(x => new { id = x.id, descripcion = x.descripcion, estatus = x.estatus == true ? "ACTIVO" : "INACTIVO" });
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_Aseguradora(tblM_CatAseguradora obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                aseguradoraFactoryServices.GetAseguradoraService().Guardar(obj);
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