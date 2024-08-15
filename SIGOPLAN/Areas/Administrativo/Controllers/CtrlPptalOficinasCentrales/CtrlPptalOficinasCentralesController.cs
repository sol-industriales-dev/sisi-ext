using Core.DAO.Administracion.CtrlPresupuestalOficinasCentrales;
using Core.DTO.Administracion.CtrlPptalOficinasCentrales;
using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using Data.Factory.Administracion.CtrlPresupuestalOficinasCentrales;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.CtrlPresupuestalOficinasCentrales
{
    public class CtrlPptalOficinasCentralesController : BaseController
    {
        #region INIT
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        public ICtrlPresupuestalOficinasCentralesDAO r_controlImpactosDAO;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            r_controlImpactosDAO = new CtrlPresupuestalOficinasCentralesFactoryService().getCtrlPresupuestalOficinasCentrales();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region RETURN VISTAS
        public ActionResult ControlImpactos()
        {
            return View();
        }

        public ActionResult MatrizEstrategias()
        {
            return View();
        }

        public ActionResult Conceptos()
        {
            return View();
        }

        public ActionResult Agrupaciones()
        {
            return View();
        }

        public ActionResult PresupuestosAnuales()
        {
            return View();
        }

        public ActionResult CapturaPptal()
        {
            return View();
        }

        public ActionResult Capturas(int? idPresupuesto)
        {
            if (idPresupuesto.HasValue)
            {
                r_controlImpactosDAO.QuitarAlerta(idPresupuesto.Value);
            }
            return View();
        }

        public ActionResult index()
        {
            return View();
        }

        public ActionResult ReporteCont()
        {
            return View();
        }

        public ActionResult PresupuestosGastos()
        {
            return View();
        }

        public ActionResult PNAgrupaciones()
        {
            return View();
        }

        public ActionResult PlanMaestro()
        {
            return View();
        }

        public ActionResult AutorizacionesAditivas()
        {
            return View();
        }

        public ActionResult _AutorizacionesAditivas()
        {
            return PartialView("_AutorizacionesAditivas");
        }

        public ActionResult Autorizantes()
        {
            return View();
        }

        public ActionResult Autorizaciones()
        {
            return View();
        }

        public ActionResult EstimadoRealMensual()
        {
            return View();
        }
        #endregion

        #region CONTROL IMPACTOS
        public ActionResult GetControlImpactos()
        {
            return Json(r_controlImpactosDAO.GetControlImpactos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarControlImpacto(ControlImpactoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarControlImpacto(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarControlImpacto(int idControlImpacto)
        {
            return Json(r_controlImpactosDAO.EliminarControlImpacto(idControlImpacto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarControlImpacto(int idControlImpacto)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarControlImpacto(idControlImpacto), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CONCEPTOS
        public ActionResult GetConceptos(int idAgrupacion)
        {
            return Json(r_controlImpactosDAO.GetConceptos(idAgrupacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarConcepto(ConceptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarConcepto(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarConcepto(int idConcepto)
        {
            return Json(r_controlImpactosDAO.EliminarConcepto(idConcepto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarConcepto(int idConcepto)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarConcepto(idConcepto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoDescripcion(int insumo)
        {
            return Json(r_controlImpactosDAO.GetInsumoDescripcion(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumosAutocomplete(string term)
        {
            return Json(r_controlImpactosDAO.GetInsumosAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCuentaDescripcion(int cta, int scta, int sscta)
        {
            return Json(r_controlImpactosDAO.GetCuentaDescripcion(cta, scta, sscta), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCuentasAutocomplete(string term)
        {
            return Json(r_controlImpactosDAO.GetCuentasAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCtaInsumoConcepto(int idConcepto)
        {
            return Json(r_controlImpactosDAO.GetCtaInsumoConcepto(idConcepto), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CAPTURA PPTOS
        public ActionResult GetCapturas(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetCapturas(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapturasContable(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetCapturasContable(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarCaptura(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarCaptura(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCaptura(int idCaptura)
        {
            return Json(r_controlImpactosDAO.EliminarCaptura(idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCaptura(int idCaptura)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarCaptura(idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapturasPorMes(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetCapturasPorMes(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCapturasPorMesContable(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetCapturasPorMesContable(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumoCuenta(int idConcepto)
        {
            return Json(r_controlImpactosDAO.GetInsumoCuenta(idConcepto), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAditivas(int idPresupuesto)
        {
            return Json(r_controlImpactosDAO.GetAditivas(idPresupuesto), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAditivasNuevas(List<AditivaDTO> aditivas, int idPresupuesto)
        {
            return Json(r_controlImpactosDAO.GuardarAditivasNuevas(aditivas, idPresupuesto));
        }

        public ActionResult NotificarPptoCapturado(int anio, string idCC)
        {
            return Json(r_controlImpactosDAO.NotificarPptoCapturado(anio, idCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarPptoNotificado(int anio, string idCC)
        {
            return Json(r_controlImpactosDAO.VerificarPptoNotificado(anio, idCC), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAditivasAAutorizar(int year, bool? esAutorizacion, int cc = -1)
        {
            return Json(r_controlImpactosDAO.GetAditivasAAutorizar(cc, year, esAutorizacion), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetalleAutorizacionAditiva(int aditivaId)
        {
            return Json(r_controlImpactosDAO.GetDetalleAutorizacionAditiva(aditivaId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutorizarAditiva(int aditivaId)
        {
            return Json(r_controlImpactosDAO.AutorizarAditiva(aditivaId));
        }

        [HttpPost]
        public JsonResult RechazarAditiva(int aditivaId, string comentario)
        {
            return Json(r_controlImpactosDAO.RechazarAditiva(aditivaId, comentario));
        }

        public ActionResult MatchPptoVsPlanMaestro(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.MatchPptoVsPlanMaestro(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarPlanMaestroCapturado(int anio, int idCC)
        {
            return Json(r_controlImpactosDAO.VerificarPlanMaestroCapturado(anio, idCC), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region INDEX
        public ActionResult GetSumaCapturas(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetSumaCapturas(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAditivasPorCaptura(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetAditivasPorCaptura(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlanMaestroRelReportePpto(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetPlanMaestroRelReportePpto(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FILL COMBOS
        public ActionResult FillEstrategias()
        {
            return Json(r_controlImpactosDAO.FillEstrategias(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillResponsablesCuentasLider()
        {
            return Json(r_controlImpactosDAO.FillResponsablesCuentasLider(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillConceptos(int idAgrupacion)
        {
            return Json(r_controlImpactosDAO.FillConceptos(idAgrupacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillAgrupaciones(int anio, int idCC)
        {
            return Json(r_controlImpactosDAO.FillAgrupaciones(anio, idCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillUsuarios()
        {
            return Json(r_controlImpactosDAO.FillUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCC()
        {
            return Json(r_controlImpactosDAO.FillCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillAnios()
        {
            return Json(r_controlImpactosDAO.FillAnios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillUsuarioRelCC(int anio)
        {
            return Json(r_controlImpactosDAO.FillUsuarioRelCC(anio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillUsuarioRelCCPptosAutorizados(int anio, int idEmpresa = -1)
        {
            return Json(r_controlImpactosDAO.FillUsuarioRelCCPptosAutorizados(anio, idEmpresa), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDivisiones(int anio)
        {
            return Json(r_controlImpactosDAO.FillDivisiones(anio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCCRelDivisiones(int anio, int divisionID)
        {
            return Json(r_controlImpactosDAO.FillCCRelDivisiones(anio, divisionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillConceptosRelCtasInsumos()
        {
            return Json(r_controlImpactosDAO.FillConceptosRelCtasInsumos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillConceptosDeCapturas(int idCC, int idAgrupacion)
        {
            return Json(r_controlImpactosDAO.FillConceptosDeCapturas(idCC, idAgrupacion), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATÁLOGOS
        #region AGRUPACIONES
        public ActionResult GetAgrupaciones(tblAF_CtrllPptalOfCe_CatAgrupaciones objFiltro)
        {
            return Json(r_controlImpactosDAO.GetAgrupaciones(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion)
        {
            return Json(r_controlImpactosDAO.GuardarNuevaAgrupacion(agrupacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion)
        {
            return Json(r_controlImpactosDAO.EditarAgrupacion(agrupacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion)
        {
            return Json(r_controlImpactosDAO.EliminarAgrupacion(agrupacion), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PRESUPUESTO ANUAL
        public ActionResult GetPresupuestosAnuales()
        {
            return Json(r_controlImpactosDAO.GetPresupuestosAnuales(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto)
        {
            return Json(r_controlImpactosDAO.GuardarNuevoPresupuestoAnual(presupuesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto)
        {
            return Json(r_controlImpactosDAO.EditarPresupuestoAnual(presupuesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto)
        {
            return Json(r_controlImpactosDAO.EliminarPresupuestoAnual(presupuesto), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AUTORIZANTES
        
        public ActionResult GetAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO)
        {
            return Json(r_controlImpactosDAO.GetAutorizantes(objDTO), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCCAutorizantes()
        {
            return Json(r_controlImpactosDAO.FillCCAutorizantes(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEditAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO)
        {
            return Json(r_controlImpactosDAO.AddEditAutorizantes(objDTO), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO)
        {
            return Json(r_controlImpactosDAO.DeleteAutorizantes(objDTO), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutorizantesCombo()
        {
            return Json(r_controlImpactosDAO.GetAutorizantesCombo(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCCAutorizantes()
        {
            return Json(r_controlImpactosDAO.GetCCAutorizantes(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region AUTORIZACIONES
        

        public JsonResult GetPresupuestosAEvaluar(string cc, int year, bool? estatus)
        {
            return Json(r_controlImpactosDAO.GetPresupuestosAEvaluar(cc, year, estatus), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Autorizar(int presupuestoId)
        {
            return Json(r_controlImpactosDAO.Autorizar(presupuestoId));
        }

        public JsonResult RechazarPpto(int idPptoAnual, string comentarioRechazo)
        {
            return Json(r_controlImpactosDAO.RechazarPpto(idPptoAnual, comentarioRechazo));
        }

        public JsonResult GetDetalleAutorizacion(int presupuestoId)
        {
            return Json(r_controlImpactosDAO.GetDetalleAutorizacion(presupuestoId), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PPTO INICIAL
        public ActionResult GetPptosIniciales()
        {
            return Json(r_controlImpactosDAO.GetPptosIniciales(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarPptoInicial(PptoInicialDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarPptoInicial(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPptoInicial(int idPptoInicial)
        {
            return Json(r_controlImpactosDAO.EliminarPptoInicial(idPptoInicial), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCCRelPpto(int idPptoInicial)
        {
            return Json(r_controlImpactosDAO.GetCCRelPpto(idPptoInicial), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgregarCC(int idPptoInicial, int idCC)
        {
            return Json(r_controlImpactosDAO.AgregarCC(idPptoInicial, idCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCCFaltantes(int idPptoInicial)
        {
            return Json(r_controlImpactosDAO.FillCboCCFaltantes(idPptoInicial), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE CONTABLE
        public ActionResult GetSumaCapturasContable(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetSumaCapturasContable(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEmpresas()
        {
            return Json(r_controlImpactosDAO.FillCboEmpresas(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PLAN MAESTRO
        public ActionResult GetPlanMaestro(PlanMaestroDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetPlanMaestro(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarPlanMaestro(PlanMaestroDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarPlanMaestro(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPlanMaestro(int idPlanMaestro)
        {
            return Json(r_controlImpactosDAO.EliminarPlanMaestro(idPlanMaestro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarPlanMaestro(int idPlanMaestro)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarPlanMaestro(idPlanMaestro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCantAgrupacionesConceptos(PlanMaestroDTO objDTO)
        {
            //Dictionary<string, object> resultados = new Dictionary<string, object>();
            //Dictionary<string, object> lstAgrupaciones = r_controlImpactosDAO.GetCantAgrupacionesConceptos(objDTO);
            //resultados.Add("lstAgrupaciones", lstAgrupaciones["lstAgrupaciones"]);
            //return Json(resultados, JsonRequestBehavior.AllowGet);
            return Json(r_controlImpactosDAO.GetCantAgrupacionesConceptos(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarMedicionIndicador(int idMedicionIndicador)
        {
            return Json(r_controlImpactosDAO.EliminarMedicionIndicador(idMedicionIndicador), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarMedicionIndicador(MedicionIndicadorDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarMedicionIndicador(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PRESUPUESTOS GASTOS
        public ActionResult GetSumaCapturasPptosGastos(CapPptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetSumaCapturasPptosGastos(objDTO), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataGraficasDashboard(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora, bool costosAdministrativos)
        {
            return Json(r_controlImpactosDAO.GetDataGraficasDashboard(year, mes, arrConstruplan, arrArrendadora, costosAdministrativos));
        }

        public JsonResult GetDataGraficasInforme(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora, bool costosAdministrativos)
        {
            return Json(r_controlImpactosDAO.GetDataGraficasInforme(year, mes, arrConstruplan, arrArrendadora, costosAdministrativos));
        }

        public JsonResult GetDetalleNivelDosIngresoGasto(int year, int mes, List<int> listaCC)
        {
            return Json(r_controlImpactosDAO.GetDetalleNivelDosIngresoGasto(year, mes, listaCC), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetalleNivelTresIngresoGasto(int year, int cc)
        {
            return Json(r_controlImpactosDAO.GetDetalleNivelTresIngresoGasto(year, cc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetalleNivelDosPresupuestoGasto(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora)
        {
            return Json(r_controlImpactosDAO.GetDetalleNivelDosPresupuestoGasto(year, mes, arrConstruplan, arrArrendadora), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetalleNivelCuatroPresupuestoGasto(CapPptoDTO objDTO, int gasto, int mes)
        {
            return Json(r_controlImpactosDAO.GetDetalleNivelCuatroPresupuestoGasto(objDTO,gasto, mes), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetalleAgrupadorConcepto(ConceptoDTO objDTO)
        {
            if (objDTO.empresa <= 0) { objDTO.empresa = 1; }
            return Json(r_controlImpactosDAO.GetDetalleAgrupadorConcepto(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetalleAgrupadorConceptoCC(ConceptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetDetalleAgrupadorConceptoCC(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RECURSOS NECESARIOS
        #region AGRUPACIONES
        public ActionResult GetRNAgrupaciones(RNAgrupacionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetRNAgrupaciones(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarRNAgrupacion(RNAgrupacionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarRNAgrupacion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRNAgrupacion(int idAgrupacion)
        {
            return Json(r_controlImpactosDAO.EliminarRNAgrupacion(idAgrupacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarRNAgrupacion(int idAgrupacion)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarRNAgrupacion(idAgrupacion), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CONCEPTOS
        public ActionResult GetRNConceptos(RNConceptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetRNConceptos(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarRNConcepto(RNConceptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearEditarRNConcepto(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRNConcepto(int idConcepto)
        {
            return Json(r_controlImpactosDAO.EliminarRNConcepto(idConcepto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarRNConcepto(int idConcepto)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarRNConcepto(idConcepto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillRNAgrupaciones(RNConceptoDTO objDTO)
        {
            return Json(r_controlImpactosDAO.FillRNAgrupaciones(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        public ActionResult GetDetallePorAgrupacion(int anio, int mes, int idCC)
        {
            return Json(r_controlImpactosDAO.GetDetallePorAgrupacion(anio, mes, idCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetalleAnual(int anio, int mes, int idCC, int tipo, int id)
        {
            return Json(r_controlImpactosDAO.GetDetalleAnual(anio, mes, idCC, tipo, id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearComentario(ComentarioDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CrearComentario(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComentarios(ComentarioDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetComentarios(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCtasPolizas(int idCC, int anio, int idMes, int idConcepto, int empresa = 1, bool costosAdministrativos = true, bool esConsultaMensual = true)
        {
            return Json(r_controlImpactosDAO.GetCtasPolizas(idCC, anio, idMes, idConcepto, empresa, costosAdministrativos, esConsultaMensual), JsonRequestBehavior.AllowGet);
        }

        #region PLAN DE ACCIÓN
        public ActionResult GetPlanAccion(PlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetPlanAccion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEPlanAccion(PlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CEPlanAccion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarPlanAccion(PlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.CerrarPlanAccion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarExistenciaPlanAccion(PlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.VerificarExistenciaPlanAccion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndicarVobo(PlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.IndicarVobo(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE (HTML) PLAN DE ACCIÓN
        public ActionResult GetReportePlanAcciones(ReportePlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetReportePlanAcciones(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoReportePlanAccion(ReportePlanAccionDTO objDTO)
        {
            return Json(r_controlImpactosDAO.EnviarCorreoReportePlanAccion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarReportePlanAccion(ReportePlanAccionDTO objDTO)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                Session["graficaCumplimientoPresupuestoAcumulado"] = objDTO.graficaCumplimientoPresupuestoAcumulado;
                Session["graficaCumplimientoPresupuestoMensual"] = objDTO.graficaCumplimientoPresupuestoMensual;
                Session["graficaProyeccion"] = objDTO.graficaProyeccion;

                Session["acumuladoGasto"] = objDTO.acumuladoGasto;
                Session["acumuladoIngreso"] = objDTO.acumuladoIngreso;
                Session["acumuladoObjetivo"] = objDTO.acumuladoObjetivo;
                Session["acumuladoReal"] = objDTO.acumuladoReal;
                Session["acumuladoCumplimiento"] = objDTO.acumuladoCumplimiento;
                Session["mensualGasto"] = objDTO.mensualGasto;
                Session["mensualIngreso"] = objDTO.mensualIngreso;
                Session["mensualObjetivo"] = objDTO.mensualObjetivo;
                Session["mensualReal"] = objDTO.mensualReal;
                Session["mensualCumplimiento"] = objDTO.mensualCumplimiento;

                Session["anio"] = objDTO.anio;
                Session["idMes"] = objDTO.idMes;
                Session["idEmpresa"] = objDTO.idEmpresa;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al generar el reporte.");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ENVIO INFORME
        public ActionResult GetEnvioInforme(EnvioInformeDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetEnvioInforme(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPermisoVisualizarEnvioInformes()
        {
            return Json(r_controlImpactosDAO.GetPermisoVisualizarEnvioInformes(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetConceptosRelCC(ConceptosRelCCDTO objDTO)
        {
            return Json(r_controlImpactosDAO.GetConceptosRelCC(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMensajesDashboard(int anio, int mes, int idCC, int empresa) 
        {
            return Json(r_controlImpactosDAO.CargarMensajes(anio, mes, idCC, empresa), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarMensajes(tblAF_CtrlPptalOfCe_Mensaje obj, int empresa = 1)
        {
            return Json(r_controlImpactosDAO.GuardarMensajes(obj, empresa), JsonRequestBehavior.AllowGet);
        }

        #region ESTIMADO REAL MENSUAL
        public ActionResult GetListadoEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO)
        {
            return Json(r_controlImpactosDAO.GetListadoEstimadoRealMensual(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO)
        {
            return Json(r_controlImpactosDAO.CEEstimadoRealMensual(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO)
        {
            return Json(r_controlImpactosDAO.GetDatosActualizarEstimadoRealMensual(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}