using Core.DAO.Contabilidad.Presupuesto;
using Core.DTO.Contabilidad.ControlPresupuestal;
using Core.DTO.Contabilidad.Presupuesto;
using Core.Enum.Maquinaria;
using Data.Factory.Contabilidad.Presupuesto;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Presupuesto
{
    public class ControlPresupuestalController : BaseController
    {
        private IControlPresupuestalDAO controlPresupuestalService;
        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controlPresupuestalService = new ControlPresupuestoFactoryService().GetControlPresupuestalService();

            result.Clear();

            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ControlPresupuestal()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult FillCboModeloEquipo(int? idGrupo)
        {
            return Json(controlPresupuestalService.FillCboModeloEquipo(idGrupo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboModeloEquipoMultiple(List<int> listaGrupos)
        {
            return Json(controlPresupuestalService.FillCboModeloEquipoMultiple(listaGrupos), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetComboEconomicos(string AreaCuenta, int? modelo)
        {
            return Json(controlPresupuestalService.getComboEconomicos(AreaCuenta, modelo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetComboEconomicosMultiple(string AreaCuenta, List<int> listaModelos)
        {
            return Json(controlPresupuestalService.getComboEconomicosMultiple(AreaCuenta, listaModelos), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarControlPresupuestal(FiltrosControlPresupuestalDTO filtros)
        {
            return Json(controlPresupuestalService.cargarControlPresupuestal(filtros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarControlPresupuestal_Solo_Grafica(FiltrosControlPresupuestalDTO filtros)
        {
            return Json(controlPresupuestalService.cargarControlPresupuestal_Solo_Grafica(filtros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarDetalleAgrupado(FiltrosControlPresupuestalDTO filtros, string economico, int concepto)
        {
            return Json(controlPresupuestalService.cargarDetalleAgrupado(filtros, economico, concepto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarDetallePresupuestal(FiltrosControlPresupuestalDTO filtros, string economico, int concepto)
        {
            return Json(controlPresupuestalService.CargarDetallePresupuestal(filtros, economico, concepto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarDetalleMovimientos(FiltrosControlPresupuestalDTO filtros, string economico, int concepto, int cta, int scta, int sscta)
        {
            return Json(controlPresupuestalService.cargarDetalleMovimientos(filtros, economico, concepto, cta, scta, sscta), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboard(FiltrosDashboardDTO filtros)
        {
            return Json(controlPresupuestalService.cargarDashboard(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComboDivision()
        {
            return Json(controlPresupuestalService.getComboDivision(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComboConcepto()
        {
            return Json(controlPresupuestalService.getComboConcepto(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerGruposMaquinaria(int idTipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = controlPresupuestalService.obtenerGruposMaquinaria(idTipo);

                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboCC(List<int> divisionesIDs)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, controlPresupuestalService.getCboCC(divisionesIDs));
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
