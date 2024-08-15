using Core.DTO;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Componentes
{
    public class CatConjuntosController : BaseController
    {



        #region Factory
        ConjuntoFactoryServices conjuntoFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            conjuntoFactoryServices = new ConjuntoFactoryServices();
            base.OnActionExecuting(filterContext);

        }

        // GET: Conjuntos
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

        public ActionResult FillGrid_Conjunto(tblM_CatConjunto conjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var listResult = conjuntoFactoryServices.getConjuntoService().FillGridConjunto(conjunto).Select(x => new { id = x.id, descripcion = x.descripcion, prefijo = x.prefijo, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });

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
        public ActionResult SaveOrUpdate_Conjunto(tblM_CatConjunto obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                conjuntoFactoryServices.getConjuntoService().Guardar(obj);
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