using Core.DTO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Data.Factory.Administracion.Seguridad.Agrupacion;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class AgrupacionCCController : BaseController
    {
        private AgrupacionCCFactoryService AgrupacionCC;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AgrupacionCC = new AgrupacionCCFactoryService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getCC()
        {
            try
            {
                var lstClaveDepto = AgrupacionCC.getAgrupacionCCFactoryService().getCC();
                result.Add(ITEMS, lstClaveDepto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCCTodos(int idAgrupacionCC)
        {
            try
            {
                var lstClaveDepto = AgrupacionCC.getAgrupacionCCFactoryService().getCCTodos(idAgrupacionCC);
                result.Add(ITEMS, lstClaveDepto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAgrupacion()
        {
            try
            {
                List<ComboDTO> lstAgrupacion = AgrupacionCC.getAgrupacionCCFactoryService().ObtnerAgrupacion();
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetalleAgrupacion(int idAgrupacionCC)
        {
            try
            {
                var lstAgrupacion = AgrupacionCC.getAgrupacionCCFactoryService().GetDetalleAgrupacion(idAgrupacionCC);
                var lst = lstAgrupacion.Select(x => new
                {
                    id = x.id,
                    nombAgrupacion = x.nomAgrupacion,
                    lstCC = x.lstDatos
                });
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearAgrupacion(AgrupacionCCDTO objAgrupaciones, List<tblS_IncidentesAgrupacionCCDet> lstAgrupaciones)
        {
            try
            {
                AgrupacionCCDTO objCrear = AgrupacionCC.getAgrupacionCCFactoryService().CrearAgrupacion(objAgrupaciones, lstAgrupaciones);
                result.Add(ITEMS, objCrear);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarAgrupacion(int id, string NuevoNombre, string[] lstAgrupacion)
        {
            try
            {
                bool objCrear = AgrupacionCC.getAgrupacionCCFactoryService().EditarAgrupacion(id, NuevoNombre, lstAgrupacion);
                if (!objCrear)
                    throw new Exception("Ya existe una agrupación con este nombre.");

                result.Add(ITEMS, objCrear);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAgrupacion(int id, int esActivo)
        {
            try
            {
                bool objCrear = AgrupacionCC.getAgrupacionCCFactoryService().EliminarAgrupacion(id, esActivo);
                result.Add(ITEMS, objCrear);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerAgrupacionCombo()
        {
            try
            {
                var lstAgrupacion = AgrupacionCC.getAgrupacionCCFactoryService().obtenerAgrupacionCombo();
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region Contratistas
         public ActionResult getCCContratista()
        {
            try
            {
                var lstClaveDepto = AgrupacionCC.getAgrupacionCCFactoryService().getCC();
                result.Add(ITEMS, lstClaveDepto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCCTodosContratista(int idAgrupacionCC)
        {
            try
            {
                var lstClaveDepto = AgrupacionCC.getAgrupacionCCFactoryService().getCCTodos(idAgrupacionCC);
                result.Add(ITEMS, lstClaveDepto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAgrupacionContratista()
        {
            try
            {
                List<ComboDTO> lstAgrupacion = AgrupacionCC.getAgrupacionCCFactoryService().ObtnerAgrupacion();
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetalleAgrupacionContratista(int idAgrupacionCC)
        {
            try
            {
                var lstAgrupacion = AgrupacionCC.getAgrupacionCCFactoryService().GetDetalleAgrupacion(idAgrupacionCC);
                var lst = lstAgrupacion.Select(x => new
                {
                    id = x.id,
                    nombAgrupacion = x.nomAgrupacion,
                    lstCC = x.lstDatos
                });
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearAgrupacionContratista(AgrupacionCCDTO objAgrupaciones, List<tblS_IncidentesAgrupacionCCDet> lstAgrupaciones)
        {
            try
            {
                AgrupacionCCDTO objCrear = AgrupacionCC.getAgrupacionCCFactoryService().CrearAgrupacion(objAgrupaciones, lstAgrupaciones);
                result.Add(ITEMS, objCrear);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarAgrupacionContratista(int id, string NuevoNombre, string[] lstAgrupacion)
        {
            try
            {
                bool objCrear = AgrupacionCC.getAgrupacionCCFactoryService().EditarAgrupacion(id, NuevoNombre, lstAgrupacion);
                if (!objCrear)
                    throw new Exception("Ya existe una agrupación con este nombre.");

                result.Add(ITEMS, objCrear);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAgrupacionContratista(int id, int esActivo)
        {
            try
            {
                bool objCrear = AgrupacionCC.getAgrupacionCCFactoryService().EliminarAgrupacion(id, esActivo);
                result.Add(ITEMS, objCrear);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerAgrupacionComboContratista()
        {
            try
            {
                var lstAgrupacion = AgrupacionCC.getAgrupacionCCFactoryService().obtenerAgrupacionCombo();
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}