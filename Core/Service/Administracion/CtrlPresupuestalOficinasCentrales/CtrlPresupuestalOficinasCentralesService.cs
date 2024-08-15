using Core.DAO.Administracion.CtrlPresupuestalOficinasCentrales;
using Core.DTO.Administracion.CtrlPptalOficinasCentrales;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.CtrlPresupuestalOficinasCentrales
{
    public class CtrlPresupuestalOficinasCentralesService : ICtrlPresupuestalOficinasCentralesDAO
    {
        #region INIT
        public ICtrlPresupuestalOficinasCentralesDAO r_controlImpactosDAO { get; set; }
        public ICtrlPresupuestalOficinasCentralesDAO ControlImpactosDAO
        {
            get { return r_controlImpactosDAO; }
            set { r_controlImpactosDAO = value; }
        }
        public CtrlPresupuestalOficinasCentralesService(ICtrlPresupuestalOficinasCentralesDAO ControlImpactos)
        {
            this.ControlImpactosDAO = ControlImpactos;
        }
        #endregion

        #region CONTROL IMPACTOS
        public Dictionary<string, object> GetControlImpactos()
        {
            return r_controlImpactosDAO.GetControlImpactos();
        }

        public Dictionary<string, object> CrearEditarControlImpacto(ControlImpactoDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarControlImpacto(objDTO);
        }

        public Dictionary<string, object> EliminarControlImpacto(int idControlImpacto)
        {
            return r_controlImpactosDAO.EliminarControlImpacto(idControlImpacto);
        }

        public Dictionary<string, object> GetDatosActualizarControlImpacto(int idControlImpacto)
        {
            return r_controlImpactosDAO.GetDatosActualizarControlImpacto(idControlImpacto);
        }
        #endregion

        #region CONCEPTOS
        public Dictionary<string, object> GetConceptos(int idAgrupacion)
        {
            return r_controlImpactosDAO.GetConceptos(idAgrupacion);
        }

        public Dictionary<string, object> CrearEditarConcepto(ConceptoDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarConcepto(objDTO);
        }

        public Dictionary<string, object> EliminarConcepto(int idConcepto)
        {
            return r_controlImpactosDAO.EliminarConcepto(idConcepto);
        }

        public Dictionary<string, object> GetDatosActualizarConcepto(int idConcepto)
        {
            return r_controlImpactosDAO.GetDatosActualizarConcepto(idConcepto);
        }

        public Dictionary<string, object> GetInsumoDescripcion(int insumo)
        {
            return r_controlImpactosDAO.GetInsumoDescripcion(insumo);
        }

        public List<AutocompletadoDTO> GetInsumosAutocomplete(string term)
        {
            return r_controlImpactosDAO.GetInsumosAutocomplete(term);
        }

        public Dictionary<string, object> GetCuentaDescripcion(int cta, int scta, int sscta)
        {
            return r_controlImpactosDAO.GetCuentaDescripcion(cta, scta, sscta);
        }

        public List<AutocompletadoDTO> GetCuentasAutocomplete(string term)
        {
            return r_controlImpactosDAO.GetCuentasAutocomplete(term);
        }

        public Dictionary<string, object> GetCtaInsumoConcepto(int idConcepto)
        {
            return r_controlImpactosDAO.GetCtaInsumoConcepto(idConcepto);
        }
        #endregion

        #region CAPTURA PPTOS
        public Dictionary<string, object> GetCapturas(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetCapturas(objDTO);
        }

        public Dictionary<string, object> GetCapturasContable(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetCapturasContable(objDTO);
        }

        public Dictionary<string, object> CrearEditarCaptura(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarCaptura(objDTO);
        }

        public Dictionary<string, object> EliminarCaptura(int idCaptura)
        {
            return r_controlImpactosDAO.EliminarCaptura(idCaptura);
        }

        public Dictionary<string, object> GetDatosActualizarCaptura(int idCaptura)
        {
            return r_controlImpactosDAO.GetDatosActualizarCaptura(idCaptura);
        }

        public Dictionary<string, object> GetCapturasPorMes(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetCapturasPorMes(objDTO);
        }

        public Dictionary<string, object> GetCapturasPorMesContable(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetCapturasPorMesContable(objDTO);
        }

        public Dictionary<string, object> GetInsumoCuenta(int idConcepto)
        {
            return r_controlImpactosDAO.GetInsumoCuenta(idConcepto);
        }

        public Dictionary<string, object> GetAditivas(int idPresupuesto)
        {
            return r_controlImpactosDAO.GetAditivas(idPresupuesto);
        }

        public Dictionary<string, object> GuardarAditivasNuevas(List<AditivaDTO> aditivas, int idPresupuesto)
        {
            return r_controlImpactosDAO.GuardarAditivasNuevas(aditivas, idPresupuesto);
        }

        public Dictionary<string, object> NotificarPptoCapturado(int anio, string idCC)
        {
            return r_controlImpactosDAO.NotificarPptoCapturado(anio, idCC);
        }

        public Dictionary<string, object> VerificarPptoNotificado(int anio, string idCC)
        {
            return r_controlImpactosDAO.VerificarPptoNotificado(anio, idCC);
        }

        public Dictionary<string, object> GetAditivasAAutorizar(int cc, int year, bool? esAutorizacion)
        {
            return r_controlImpactosDAO.GetAditivasAAutorizar(cc, year, esAutorizacion);
        }

        public Dictionary<string, object> GetDetalleAutorizacionAditiva(int aditivaId)
        {
            return r_controlImpactosDAO.GetDetalleAutorizacionAditiva(aditivaId);
        }

        public Dictionary<string, object> AutorizarAditiva(int aditivaId)
        {
            return r_controlImpactosDAO.AutorizarAditiva(aditivaId);
        }

        public Dictionary<string, object> RechazarAditiva(int aditivaId, string comentario)
        {
            return r_controlImpactosDAO.RechazarAditiva(aditivaId, comentario);
        }

        public Dictionary<string, object> MatchPptoVsPlanMaestro(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.MatchPptoVsPlanMaestro(objDTO);
        }

        public Dictionary<string, object> VerificarPlanMaestroCapturado(int anio, int idCC)
        {
            return r_controlImpactosDAO.VerificarPlanMaestroCapturado(anio, idCC);
        }
        #endregion

        #region INDEX
        public Dictionary<string, object> GetSumaCapturas(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetSumaCapturas(objDTO);
        }

        public Dictionary<string, object> GetAditivasPorCaptura(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetAditivasPorCaptura(objDTO);
        }

        public Dictionary<string, object> GetPlanMaestroRelReportePpto(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetPlanMaestroRelReportePpto(objDTO);
        }
        #endregion

        #region FILL COMBOS
        public Dictionary<string, object> FillEstrategias()
        {
            return r_controlImpactosDAO.FillEstrategias();
        }

        public Dictionary<string, object> FillResponsablesCuentasLider()
        {
            return r_controlImpactosDAO.FillResponsablesCuentasLider();
        }

        public Dictionary<string, object> FillConceptos(int idAgrupacion)
        {
            return r_controlImpactosDAO.FillConceptos(idAgrupacion);
        }

        public Dictionary<string, object> FillAgrupaciones(int anio, int idCC)
        {
            return r_controlImpactosDAO.FillAgrupaciones(anio, idCC);
        }

        public Dictionary<string, object> FillUsuarios()
        {
            return r_controlImpactosDAO.FillUsuarios();
        }

        public Dictionary<string, object> FillCC()
        {
            return r_controlImpactosDAO.FillCC();
        }

        public Dictionary<string, object> FillAnios()
        {
            return r_controlImpactosDAO.FillAnios();
        }

        public Dictionary<string, object> FillUsuarioRelCC(int anio)
        {
            return r_controlImpactosDAO.FillUsuarioRelCC(anio);
        }

        public Dictionary<string, object> FillUsuarioRelCCPptosAutorizados(int anio, int idEmpresa)
        {
            return r_controlImpactosDAO.FillUsuarioRelCCPptosAutorizados(anio, idEmpresa);
        }

        public Dictionary<string, object> FillDivisiones(int anio)
        {
            return r_controlImpactosDAO.FillDivisiones(anio);
        }

        public Dictionary<string, object> FillCCRelDivisiones(int anio, int divisionID)
        {
            return r_controlImpactosDAO.FillCCRelDivisiones(anio, divisionID);
        }

        public Dictionary<string, object> FillConceptosRelCtasInsumos()
        {
            return r_controlImpactosDAO.FillConceptosRelCtasInsumos();
        }

        public Dictionary<string, object> FillConceptosDeCapturas(int idCC, int idAgrupacion)
        {
            return r_controlImpactosDAO.FillConceptosDeCapturas(idCC, idAgrupacion);
        }
        #endregion

        #region CATÁLOGOS
        #region AGRUPACIONES
        public Dictionary<string, object> GetAgrupaciones(tblAF_CtrllPptalOfCe_CatAgrupaciones objFiltro)
        {
            return r_controlImpactosDAO.GetAgrupaciones(objFiltro);
        }

        public Dictionary<string, object> GuardarNuevaAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion)
        {
            return r_controlImpactosDAO.GuardarNuevaAgrupacion(agrupacion);
        }

        public Dictionary<string, object> EditarAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion)
        {
            return r_controlImpactosDAO.EditarAgrupacion(agrupacion);
        }

        public Dictionary<string, object> EliminarAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion)
        {
            return r_controlImpactosDAO.EliminarAgrupacion(agrupacion);
        }
        #endregion

        #region PRESUPUESTO ANUAL
        public Dictionary<string, object> GetPresupuestosAnuales()
        {
            return r_controlImpactosDAO.GetPresupuestosAnuales();
        }

        public Dictionary<string, object> GuardarNuevoPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto)
        {
            return r_controlImpactosDAO.GuardarNuevoPresupuestoAnual(presupuesto);
        }

        public Dictionary<string, object> EditarPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto)
        {
            return r_controlImpactosDAO.EditarPresupuestoAnual(presupuesto);
        }

        public Dictionary<string, object> EliminarPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto)
        {
            return r_controlImpactosDAO.EliminarPresupuestoAnual(presupuesto);
        }
        #endregion

        #region AUTORIZANTES
        public Dictionary<string, object> GetAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO)
        {
            return r_controlImpactosDAO.GetAutorizantes(objDTO);
        }
        public Dictionary<string, object> FillCCAutorizantes()
        {
            return r_controlImpactosDAO.FillCCAutorizantes();
        }
        public Dictionary<string, object> AddEditAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO)
        {
            return r_controlImpactosDAO.AddEditAutorizantes(objDTO);
        }
        public Dictionary<string, object> DeleteAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO)
        {
            return r_controlImpactosDAO.DeleteAutorizantes(objDTO);
        }
        public Dictionary<string, object> GetAutorizantesCombo()
        {
            return r_controlImpactosDAO.GetAutorizantesCombo();
        }
        public Dictionary<string, object> GetCCAutorizantes()
        {
            return r_controlImpactosDAO.GetCCAutorizantes();
        }
        #endregion
        #endregion

        #region AUTORIZACIONES
        public Dictionary<string, object> GetPresupuestosAEvaluar(string cc, int year, bool? estatus)
        {
            return r_controlImpactosDAO.GetPresupuestosAEvaluar(cc, year, estatus);
        }

        public Dictionary<string, object> Autorizar(int presupuestoId)
        {
            return r_controlImpactosDAO.Autorizar(presupuestoId);
        }

        public Dictionary<string, object> RechazarPpto(int idPptoAnual, string comentarioRechazo)
        {
            return r_controlImpactosDAO.RechazarPpto(idPptoAnual, comentarioRechazo);
        }

        public Dictionary<string, object> GetDetalleAutorizacion(int presupuestoId)
        {
            return r_controlImpactosDAO.GetDetalleAutorizacion(presupuestoId);
        }
        #endregion

        #region PPTO INICIAL
        public Dictionary<string, object> GetPptosIniciales()
        {
            return r_controlImpactosDAO.GetPptosIniciales();
        }

        public Dictionary<string, object> CrearEditarPptoInicial(PptoInicialDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarPptoInicial(objDTO);
        }

        public Dictionary<string, object> EliminarPptoInicial(int idPptoInicial)
        {
            return r_controlImpactosDAO.EliminarPptoInicial(idPptoInicial);
        }

        public Dictionary<string, object> GetCCRelPpto(int idPptoInicial)
        {
            return r_controlImpactosDAO.GetCCRelPpto(idPptoInicial);
        }

        public Dictionary<string, object> AgregarCC(int idPptoInicial, int idCC)
        {
            return r_controlImpactosDAO.AgregarCC(idPptoInicial, idCC);
        }

        public Dictionary<string, object> FillCboCCFaltantes(int idPptoInicial)
        {
            return r_controlImpactosDAO.FillCboCCFaltantes(idPptoInicial);
        }
        #endregion

        #region REPORTE CONTABLE
        public Dictionary<string, object> GetSumaCapturasContable(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetSumaCapturasContable(objDTO);
        }

        public Dictionary<string, object> FillCboEmpresas()
        {
            return r_controlImpactosDAO.FillCboEmpresas();
        }
        #endregion

        #region PLAN MAESTRO
        public Dictionary<string, object> GetPlanMaestro(PlanMaestroDTO objDTO)
        {
            return r_controlImpactosDAO.GetPlanMaestro(objDTO);
        }

        public Dictionary<string, object> CrearEditarPlanMaestro(PlanMaestroDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarPlanMaestro(objDTO);
        }

        public Dictionary<string, object> EliminarPlanMaestro(int idPlanMaestro)
        {
            return r_controlImpactosDAO.EliminarPlanMaestro(idPlanMaestro);
        }

        public Dictionary<string, object> GetDatosActualizarPlanMaestro(int idPlanMaestro)
        {
            return r_controlImpactosDAO.GetDatosActualizarPlanMaestro(idPlanMaestro);
        }

        public Dictionary<string, object> GetCantAgrupacionesConceptos(PlanMaestroDTO objDTO)
        {
            return r_controlImpactosDAO.GetCantAgrupacionesConceptos(objDTO);
        }

        public Dictionary<string, object> EliminarMedicionIndicador(int idMedicionIndicador)
        {
            return r_controlImpactosDAO.EliminarMedicionIndicador(idMedicionIndicador);
        }

        public Dictionary<string, object> CrearEditarMedicionIndicador(MedicionIndicadorDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarMedicionIndicador(objDTO);
        }

        public Dictionary<string, object> GetRptPlanMaestro(int id)
        {
            return r_controlImpactosDAO.GetRptPlanMaestro(id);
        }
        #endregion

        #region PRESUPUESTOS GASTOS
        public Dictionary<string, object> GetSumaCapturasPptosGastos(CapPptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetSumaCapturasPptosGastos(objDTO);
        }

        public Dictionary<string, object> GetDataGraficasDashboard(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora, bool costosAdministrativos)
        {
            return r_controlImpactosDAO.GetDataGraficasDashboard(year, mes, arrConstruplan, arrArrendadora, costosAdministrativos);
        }

        public Dictionary<string, object> GetDataGraficasInforme(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora, bool costosAdministrativos)
        {
            return r_controlImpactosDAO.GetDataGraficasInforme(year, mes, arrConstruplan, arrArrendadora, costosAdministrativos);
        }

        public Dictionary<string, object> GetDetalleNivelDosIngresoGasto(int year, int mes, List<int> listaCC)
        {
            return r_controlImpactosDAO.GetDetalleNivelDosIngresoGasto(year, mes, listaCC);
        }
        public Dictionary<string, object> GetDetalleNivelTresIngresoGasto(int year, int cc)
        {
            return r_controlImpactosDAO.GetDetalleNivelTresIngresoGasto(year, cc);
        }
        public Dictionary<string, object> GetDetalleNivelDosPresupuestoGasto(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora)
        {
            return r_controlImpactosDAO.GetDetalleNivelDosPresupuestoGasto(year, mes, arrConstruplan, arrArrendadora);
        }

        public Dictionary<string, object> GetDetalleNivelCuatroPresupuestoGasto(CapPptoDTO objDTO, int gasto, int mes)
        {
            return r_controlImpactosDAO.GetDetalleNivelCuatroPresupuestoGasto(objDTO,gasto, mes);
        }

        public Dictionary<string, object> GetDetalleAgrupadorConcepto(ConceptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetDetalleAgrupadorConcepto(objDTO);
        }

        public Dictionary<string, object> GetDetalleAgrupadorConceptoCC(ConceptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetDetalleAgrupadorConceptoCC(objDTO);
        }
        #endregion

        #region RECURSOS NECESARIOS
        #region AGRUPACIONES
        public Dictionary<string, object> GetRNAgrupaciones(RNAgrupacionDTO objDTO) 
        {
            return r_controlImpactosDAO.GetRNAgrupaciones(objDTO);
        }

        public Dictionary<string, object> CrearEditarRNAgrupacion(RNAgrupacionDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarRNAgrupacion(objDTO);
        }

        public Dictionary<string, object> EliminarRNAgrupacion(int idAgrupacion)
        {
            return r_controlImpactosDAO.EliminarRNAgrupacion(idAgrupacion);
        }

        public Dictionary<string, object> GetDatosActualizarRNAgrupacion(int idAgrupacion)
        {
            return r_controlImpactosDAO.GetDatosActualizarRNAgrupacion(idAgrupacion);
        }
        #endregion

        #region CONCEPTOS
        public Dictionary<string, object> GetRNConceptos(RNConceptoDTO objDTO)
        {
            return r_controlImpactosDAO.GetRNConceptos(objDTO);
        }

        public Dictionary<string, object> CrearEditarRNConcepto(RNConceptoDTO objDTO)
        {
            return r_controlImpactosDAO.CrearEditarRNConcepto(objDTO);
        }

        public Dictionary<string, object> EliminarRNConcepto(int idConcepto)
        {
            return r_controlImpactosDAO.EliminarRNConcepto(idConcepto);
        }

        public Dictionary<string, object> GetDatosActualizarRNConcepto(int idConcepto)
        {
            return r_controlImpactosDAO.GetDatosActualizarRNConcepto(idConcepto);
        }

        public Dictionary<string, object> FillRNAgrupaciones(RNConceptoDTO objDTO)
        {
            return r_controlImpactosDAO.FillRNAgrupaciones(objDTO);
        }
        #endregion
        #endregion

        public Dictionary<string, object> GetDetallePorAgrupacion(int anio, int mes, int idCC)
        {
            return r_controlImpactosDAO.GetDetallePorAgrupacion(anio, mes, idCC);
        }

        public Dictionary<string, object> GetDetalleAnual(int anio, int mes, int idCC, int tipo, int id)
        {
            return r_controlImpactosDAO.GetDetalleAnual(anio, mes, idCC, tipo, id);
        }

        public Dictionary<string, object> CrearComentario(ComentarioDTO objDTO)
        {
            return r_controlImpactosDAO.CrearComentario(objDTO);
        }

        public Dictionary<string, object> GetComentarios(ComentarioDTO objDTO)
        {
            return r_controlImpactosDAO.GetComentarios(objDTO);
        }

        public void QuitarAlerta(int idPresupuesto)
        {
            r_controlImpactosDAO.QuitarAlerta(idPresupuesto);
        }

        #region FNC GRALES REPORTE
        public Dictionary<string, object> GetAreaCuentaByCC(int idCC)
        {
            return r_controlImpactosDAO.GetAreaCuentaByCC(idCC);
        }
        #endregion

        public Dictionary<string, object> GetCtasPolizas(int idCC, int anio, int idMes, int idConcepto, int empresa, bool costosAdministrativos, bool esConsultaMensual)
        {
            return r_controlImpactosDAO.GetCtasPolizas(idCC, anio, idMes, idConcepto, empresa, costosAdministrativos, esConsultaMensual);
        }

        #region PLAN DE ACCION
        public Dictionary<string, object> GetPlanAccion(PlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.GetPlanAccion(objDTO);
        }

        public Dictionary<string, object> CEPlanAccion(PlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.CEPlanAccion(objDTO);
        }

        public Dictionary<string, object> CerrarPlanAccion(PlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.CerrarPlanAccion(objDTO);
        }

        public Dictionary<string, object> VerificarExistenciaPlanAccion(PlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.VerificarExistenciaPlanAccion(objDTO);
        }

        public Dictionary<string, object> IndicarVobo(PlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.IndicarVobo(objDTO);
        }
        #endregion

        #region REPORTE (HTML) PLAN DE ACCIÓN
        public List<PlanAccionReporteDTO> GetPlanAccion(int idCC)
        {
            return r_controlImpactosDAO.GetPlanAccion(idCC); 
        }

        public Dictionary<string, object> GetReportePlanAcciones(ReportePlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.GetReportePlanAcciones(objDTO);
        }

        public Dictionary<string, object> EnviarCorreoReportePlanAccion(ReportePlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.EnviarCorreoReportePlanAccion(objDTO);
        }

        public PlanAccionDTO GetMesCC(PlanAccionDTO objDTO)
        {
            return r_controlImpactosDAO.GetMesCC(objDTO);
        }
        #endregion

        #region ENVIO INFORME
        public Dictionary<string, object> GetEnvioInforme(EnvioInformeDTO objDTO)
        {
            return r_controlImpactosDAO.GetEnvioInforme(objDTO);
        }

        public Dictionary<string, object> GetPermisoVisualizarEnvioInformes()
        {
            return r_controlImpactosDAO.GetPermisoVisualizarEnvioInformes();
        }
        #endregion

        public Dictionary<string, object> GetConceptosRelCC(ConceptosRelCCDTO objDTO)
        {
            return r_controlImpactosDAO.GetConceptosRelCC(objDTO);
        }
        public Dictionary<string, object> GuardarMensajes(tblAF_CtrlPptalOfCe_Mensaje obj, int empresa)
        {
            return r_controlImpactosDAO.GuardarMensajes(obj, empresa);
        }
        public Dictionary<string, object> CargarMensajes(int anio, int mes, int idCC, int empresa)
        {
            return r_controlImpactosDAO.CargarMensajes(anio, mes, idCC, empresa);
        }

        #region ESTIMADO REAL MENSUAL
        public Dictionary<string, object> GetListadoEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO)
        {
            return r_controlImpactosDAO.GetListadoEstimadoRealMensual(objParamsDTO);
        }

        public Dictionary<string, object> CEEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO)
        {
            return r_controlImpactosDAO.CEEstimadoRealMensual(objParamsDTO);
        }

        public Dictionary<string, object> GetDatosActualizarEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO)
        {
            return r_controlImpactosDAO.GetDatosActualizarEstimadoRealMensual(objParamsDTO);
        }
        #endregion
    }
}
