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
    public class CatGruposController : BaseController
    {


        #region Factory
        GrupoMaquinariaFactoryServices grupoMaquinariaFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            grupoMaquinariaFactoryServices = new GrupoMaquinariaFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: CatGrupos
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

        public ActionResult FillGrid_TipoMaquinaria(tblM_CatGrupoMaquinaria obj)
        {

            var result = new Dictionary<string, object>();
            var listResult = grupoMaquinariaFactoryServices.getGrupoMaquinariaService().FillGridGrupoMaquinaria(obj).Select(x => new { id = x.id, idTipo = x.tipoEquipoID, tipo = x.tipoEquipo.descripcion, descripcion = x.descripcion, prefijo = x.prefijo, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO", dn = x.dn, sos = x.sos, bitacora = x.bitacora });

            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoMaquinaria(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, grupoMaquinariaFactoryServices.getGrupoMaquinariaService().FillCboTipoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_GrupoMaquinaria(tblM_CatGrupoMaquinaria obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                grupoMaquinariaFactoryServices.getGrupoMaquinariaService().Guardar(obj);
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

        public ActionResult FillCboGrupoMaquina(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, grupoMaquinariaFactoryServices.getGrupoMaquinariaService().FillCboGrupoMaquinaria(obj).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = (x.prefijo + "-" + x.noEco.ToString().PadLeft(3, '0')) }));
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