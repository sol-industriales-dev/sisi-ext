using Core.DTO.Administracion.CtrlPptalOficinasCentrales;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.CtrlPresupuestalOficinasCentrales
{
    public interface ICtrlPresupuestalOficinasCentralesDAO
    {
        #region CONTROL IMPACTOS
        Dictionary<string, object> GetControlImpactos();

        Dictionary<string, object> CrearEditarControlImpacto(ControlImpactoDTO objDTO);

        Dictionary<string, object> EliminarControlImpacto(int idControlImpacto);

        Dictionary<string, object> GetDatosActualizarControlImpacto(int idControlImpacto);
        #endregion

        #region CONCEPTOS
        Dictionary<string, object> GetConceptos(int idAgrupacion);

        Dictionary<string, object> CrearEditarConcepto(ConceptoDTO objDTO);

        Dictionary<string, object> EliminarConcepto(int idConcepto);

        Dictionary<string, object> GetDatosActualizarConcepto(int idConcepto);

        Dictionary<string, object> GetInsumoDescripcion(int insumo);

        List<AutocompletadoDTO> GetInsumosAutocomplete(string term);

        Dictionary<string, object> GetCuentaDescripcion(int cta, int scta, int sscta);

        List<AutocompletadoDTO> GetCuentasAutocomplete(string term);

        Dictionary<string, object> GetCtaInsumoConcepto(int idConcepto);
        #endregion

        #region CAPTURA PPTOS
        Dictionary<string, object> GetCapturas(CapPptoDTO objDTO);

        Dictionary<string, object> GetCapturasContable(CapPptoDTO objDTO);

        Dictionary<string, object> CrearEditarCaptura(CapPptoDTO objDTO);

        Dictionary<string, object> EliminarCaptura(int idCaptura);

        Dictionary<string, object> GetDatosActualizarCaptura(int idCaptura);

        Dictionary<string, object> GetCapturasPorMes(CapPptoDTO objDTO);

        Dictionary<string, object> GetCapturasPorMesContable(CapPptoDTO objDTO);

        Dictionary<string, object> GetInsumoCuenta(int idConcepto);

        Dictionary<string, object> GetAditivas(int idPresupuesto);

        Dictionary<string, object> GuardarAditivasNuevas(List<AditivaDTO> aditivas, int idPresupuesto);

        Dictionary<string, object> NotificarPptoCapturado(int anio, string idCC);

        Dictionary<string, object> VerificarPptoNotificado(int anio, string idCC);

        Dictionary<string, object> GetAditivasAAutorizar(int cc, int year, bool? esAutorizacion);

        //Dictionary<string, object> GetPresupuestoConAditivas(int idPresupuesto);

        Dictionary<string, object> AutorizarAditiva(int aditivaId);

        Dictionary<string, object> RechazarAditiva(int aditivaId, string comentario);

        Dictionary<string, object> MatchPptoVsPlanMaestro(CapPptoDTO objDTO);

        Dictionary<string, object> GetDetalleAutorizacionAditiva(int aditivaId);

        Dictionary<string, object> VerificarPlanMaestroCapturado(int anio, int idCC);
        #endregion

        #region INDEX
        Dictionary<string, object> GetSumaCapturas(CapPptoDTO objDTO);

        Dictionary<string, object> GetAditivasPorCaptura(CapPptoDTO objDTO);

        Dictionary<string, object> GetPlanMaestroRelReportePpto(CapPptoDTO objDTO);
        #endregion

        #region FILL COMBOS
        Dictionary<string, object> FillEstrategias();

        Dictionary<string, object> FillResponsablesCuentasLider();

        Dictionary<string, object> FillConceptos(int idAgrupacion);

        Dictionary<string, object> FillAgrupaciones(int anio, int idCC);

        Dictionary<string, object> FillUsuarios();

        Dictionary<string, object> FillCC();

        Dictionary<string, object> FillAnios();

        Dictionary<string, object> FillUsuarioRelCC(int anio);

        Dictionary<string, object> FillUsuarioRelCCPptosAutorizados(int anio, int idEmpresa);

        Dictionary<string, object> FillDivisiones(int anio);

        Dictionary<string, object> FillCCRelDivisiones(int anio, int divisionID);

        Dictionary<string, object> FillConceptosRelCtasInsumos();

        Dictionary<string, object> FillConceptosDeCapturas(int idCC, int idAgrupacion);
        #endregion

        #region CATÁLOGOS
        #region AGRUPACIONES
        Dictionary<string, object> GetAgrupaciones(tblAF_CtrllPptalOfCe_CatAgrupaciones objFiltro);
        Dictionary<string, object> GuardarNuevaAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion);
        Dictionary<string, object> EditarAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion);
        Dictionary<string, object> EliminarAgrupacion(tblAF_CtrllPptalOfCe_CatAgrupaciones agrupacion);
        #endregion
        #region PRESUPUESTO ANUAL
        Dictionary<string, object> GetPresupuestosAnuales();
        Dictionary<string, object> GuardarNuevoPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto);
        Dictionary<string, object> EditarPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto);
        Dictionary<string, object> EliminarPresupuestoAnual(tblAF_CtrlPptalOfCe_PptoAnual presupuesto);
        #endregion
        #region AUTORIZANTES
        Dictionary<string, object> GetAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO);
        Dictionary<string, object> FillCCAutorizantes();
        Dictionary<string, object> AddEditAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO);
        Dictionary<string, object> DeleteAutorizantes(tblAF_CtrllPptalOfCe_CatAutorizantes objDTO);
        Dictionary<string, object> GetAutorizantesCombo();
        Dictionary<string, object> GetCCAutorizantes();
        #endregion
        #endregion

        #region AUTORIZACIONES
        Dictionary<string, object> GetPresupuestosAEvaluar(string cc, int year, bool? estatus);

        Dictionary<string, object> Autorizar(int presupuestoId);

        Dictionary<string, object> RechazarPpto(int idPptoAnual, string comentarioRechazo);

        Dictionary<string, object> GetDetalleAutorizacion(int presupuestoId);
        #endregion

        #region PPTO INICIAL
        Dictionary<string, object> GetPptosIniciales();

        Dictionary<string, object> CrearEditarPptoInicial(PptoInicialDTO objDTO);

        Dictionary<string, object> EliminarPptoInicial(int idPptoInicial);

        Dictionary<string, object> GetCCRelPpto(int idPptoInicial);

        Dictionary<string, object> AgregarCC(int idPptoInicial, int idCC);

        Dictionary<string, object> FillCboCCFaltantes(int idPptoInicial);
        #endregion

        #region REPORTE CONTABLE
        Dictionary<string, object> GetSumaCapturasContable(CapPptoDTO objDTO);

        Dictionary<string, object> FillCboEmpresas();
        #endregion

        #region PLAN MAESTRO
        Dictionary<string, object> GetPlanMaestro(PlanMaestroDTO objDTO);

        Dictionary<string, object> CrearEditarPlanMaestro(PlanMaestroDTO objDTO);

        Dictionary<string, object> EliminarPlanMaestro(int idPlanMaestro);

        Dictionary<string, object> GetDatosActualizarPlanMaestro(int idPlanMaestro);

        Dictionary<string, object> GetCantAgrupacionesConceptos(PlanMaestroDTO objDTO);

        Dictionary<string, object> EliminarMedicionIndicador(int idMedicionIndicador);

        Dictionary<string, object> CrearEditarMedicionIndicador(MedicionIndicadorDTO objDTO);

        Dictionary<string, object> GetRptPlanMaestro(int id);
        #endregion

        #region PRESUPUESTOS GASTOS
        Dictionary<string, object> GetSumaCapturasPptosGastos(CapPptoDTO objDTO);

        Dictionary<string, object> GetDataGraficasDashboard(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora, bool costosAdministrativos);
        Dictionary<string, object> GetDataGraficasInforme(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora, bool costosAdministrativos);

        Dictionary<string, object> GetDetalleNivelDosIngresoGasto(int year, int mes, List<int> listaCC);
        Dictionary<string, object> GetDetalleNivelTresIngresoGasto(int year, int _cc);

        Dictionary<string, object> GetDetalleNivelDosPresupuestoGasto(int year, int mes, List<int> arrConstruplan, List<int> arrArrendadora);
        Dictionary<string, object> GetDetalleNivelCuatroPresupuestoGasto(CapPptoDTO objDTO, int gasto, int mes);

        Dictionary<string, object> GetDetalleAgrupadorConcepto(ConceptoDTO objDTO);

        Dictionary<string, object> GetDetalleAgrupadorConceptoCC(ConceptoDTO objDTO);
        #endregion

        #region RECURSOS NECESARIOS
        #region AGRUPACIONES
        Dictionary<string, object> GetRNAgrupaciones(RNAgrupacionDTO objDTO);

        Dictionary<string, object> CrearEditarRNAgrupacion(RNAgrupacionDTO objDTO);

        Dictionary<string, object> EliminarRNAgrupacion(int idAgrupacion);

        Dictionary<string, object> GetDatosActualizarRNAgrupacion(int idAgrupacion);
        #endregion

        #region CONCEPTOS
        Dictionary<string, object> GetRNConceptos(RNConceptoDTO objDTO);

        Dictionary<string, object> CrearEditarRNConcepto(RNConceptoDTO objDTO);

        Dictionary<string, object> EliminarRNConcepto(int idConcepto);

        Dictionary<string, object> GetDatosActualizarRNConcepto(int idConcepto);

        Dictionary<string, object> FillRNAgrupaciones(RNConceptoDTO objDTO);
        #endregion
        #endregion

        Dictionary<string, object> GetDetallePorAgrupacion(int anio, int mes, int idCC);

        Dictionary<string, object> GetDetalleAnual(int anio, int mes, int idCC, int tipo, int id);

        Dictionary<string, object> CrearComentario(ComentarioDTO objDTO);

        Dictionary<string, object> GetComentarios(ComentarioDTO objDTO);

        void QuitarAlerta(int idPresupuesto);

        #region FNC GRALES REPORTE
        Dictionary<string, object> GetAreaCuentaByCC(int idCC);
        #endregion

        Dictionary<string, object> GetCtasPolizas(int idCC, int anio, int idMes, int idConcepto, int empresa, bool costosAdministrativos, bool esConsultaMensual);

        #region PLAN DE ACCION
        Dictionary<string, object> GetPlanAccion(PlanAccionDTO objDTO);

        Dictionary<string, object> CEPlanAccion(PlanAccionDTO objDTO);

        Dictionary<string, object> CerrarPlanAccion(PlanAccionDTO objDTO);

        Dictionary<string, object> VerificarExistenciaPlanAccion(PlanAccionDTO objDTO);

        Dictionary<string, object> IndicarVobo(PlanAccionDTO objDTO);
        #endregion

        #region REPORTE (HTML) PLAN DE ACCIÓN
        List<PlanAccionReporteDTO> GetPlanAccion(int idCC);

        Dictionary<string, object> GetReportePlanAcciones(ReportePlanAccionDTO objDTO);

        Dictionary<string, object> EnviarCorreoReportePlanAccion(ReportePlanAccionDTO objDTO);

        PlanAccionDTO GetMesCC(PlanAccionDTO objDTO);
        #endregion

        #region ENVIO INFORME
        Dictionary<string, object> GetEnvioInforme(EnvioInformeDTO objDTO);

        Dictionary<string, object> GetPermisoVisualizarEnvioInformes();
        #endregion

        Dictionary<string, object> GetConceptosRelCC(ConceptosRelCCDTO objDTO);

        Dictionary<string, object> GuardarMensajes(tblAF_CtrlPptalOfCe_Mensaje obj, int empresa);

        Dictionary<string, object> CargarMensajes(int anio, int mes, int idCC, int empresa);

        #region ESTIMADO REAL MENSUAL
        Dictionary<string, object> GetListadoEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO);

        Dictionary<string, object> CEEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO);

        Dictionary<string, object> GetDatosActualizarEstimadoRealMensual(EstimadoRealMensualDTO objParamsDTO);
        #endregion
    }
}
