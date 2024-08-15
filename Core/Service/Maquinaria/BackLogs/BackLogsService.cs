using Core.DAO.Maquinaria.BackLogs;
using Core.DTO.Maquinaria.BackLogs;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.BackLogs;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria.BackLogs;
using Core.Enum.Maquinaria;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Entity.Administrativo.Almacen;
using Core.DTO;
using Core.Enum.Multiempresa;

namespace Core.Service.Maquinaria.BackLogs
{
    public class BackLogsService : IBackLogsDAO
    {
        #region CONSTRUCTOR
        private IBackLogsDAO m_IBackLogsDAO;

        private IBackLogsDAO BackLogsDAO
        {
            get { return m_IBackLogsDAO; }
            set { m_IBackLogsDAO = value; }
        }

        public BackLogsService(IBackLogsDAO BackLogsDAO)
        {
            this.BackLogsDAO = BackLogsDAO;
        }
        #endregion

        #region BACKLOGS OBRA
        #region INDEX
        public Dictionary<string, object> GetBackLogsGraficaIndex(BackLogsDTO objDTO)
        {
            return BackLogsDAO.GetBackLogsGraficaIndex(objDTO);
        }
        #endregion

        #region REGISTRO DE BACKLOGS
        public List<BackLogsDTO> GetBackLogsFiltros(BackLogsDTO objBackLog, bool esObra)
        {
            return BackLogsDAO.GetBackLogsFiltros(objBackLog, esObra);
        }

        public decimal GetTotalOCRehabilitacion(GetTotalMXDTO objTotalDTO)
        {
            return BackLogsDAO.GetTotalOCRehabilitacion(objTotalDTO);
        }

        public List<InsumosDTO> GetInsumos(InsumosDTO objFiltro)
        {
            return BackLogsDAO.GetInsumos(objFiltro);
        }

        public Dictionary<string, object> GetDatosGraficasBLObra(BackLogsDTO objBL)
        {
            return BackLogsDAO.GetDatosGraficasBLObra(objBL);
        }

        public Dictionary<string, object> GetDatosGraficasBLTMC(BackLogsDTO objBL)
        {
            return BackLogsDAO.GetDatosGraficasBLTMC(objBL);
        }

        public List<tblBL_CatBackLogs> GetNumBackLogs(string areaCuenta)
        {
            return BackLogsDAO.GetNumBackLogs(areaCuenta);
        }

        public int GetNumBacklogsAniosAnteriores(string areaCuenta)
        {
            return BackLogsDAO.GetNumBacklogsAniosAnteriores(areaCuenta);
        }

        public List<tblBL_Partes> GetPartes(int idBackLog)
        {
            return BackLogsDAO.GetPartes(idBackLog);
        }

        public bool CrearBackLog(tblBL_CatBackLogs datosFormulario, List<tblBL_Partes> datosPartes, List<tblBL_ManoObra> datosManoObra, bool esParte, bool esManoObra, bool esObra, int idUsuarioResponsable)
        {
            return BackLogsDAO.CrearBackLog(datosFormulario, datosPartes, datosManoObra, esParte, esManoObra, esObra, idUsuarioResponsable);
        }

        public bool ActualizarBackLog(tblBL_CatBackLogs objBL, bool esActualizarCC)
        {
            return BackLogsDAO.ActualizarBackLog(objBL, esActualizarCC);
        }

        public bool ActualizarParte(tblBL_Partes datosParte)
        {
            return BackLogsDAO.ActualizarParte(datosParte);
        }

        public bool CrearParte(tblBL_Partes datosParte)
        {
            return BackLogsDAO.CrearParte(datosParte);
        }

        public bool ActualizarManoObra(tblBL_ManoObra datosManoObra)
        {
            return BackLogsDAO.ActualizarManoObra(datosManoObra);
        }

        public bool CrearManoObra(tblBL_ManoObra datosManoObra)
        {
            return BackLogsDAO.CrearManoObra(datosManoObra);
        }

        public List<tblBL_CatSubconjuntos> GetSubconjuntos(List<int> idSubconjunto)
        {
            return BackLogsDAO.GetSubconjuntos(idSubconjunto);
        }

        public List<tblBL_CatConjuntos> GetConjuntos(List<int> idConjunto)
        {
            return BackLogsDAO.GetConjuntos(idConjunto);
        }

        public List<ComboDTO> FillAreasCuentas()
        {
            return BackLogsDAO.FillAreasCuentas();
        }

        public decimal GetCostoPromedio(int almacen, int insumo)
        {
            return BackLogsDAO.GetCostoPromedio(almacen, insumo);
        }

        public int RetornarAlmacen(string areaCuenta)
        {
            return BackLogsDAO.RetornarAlmacen(areaCuenta);
        }

        public List<ComboDTO> FillCboCC(string areaCuenta, bool esObra)
        {
            return BackLogsDAO.FillCboCC(areaCuenta, esObra);
        }

        public List<ComboDTO> FillCboConjunto()
        {
            return BackLogsDAO.FillCboConjunto();
        }

        public List<ComboDTO> FillCboSubconjunto(int idConjunto)
        {
            return BackLogsDAO.FillCboSubconjunto(idConjunto);
        }

        public List<ComboDTO> FillCboModelo()
        {
            return BackLogsDAO.FillCboModelo();
        }

        public List<ComboDTO> FillCboGrupo(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.FillCboGrupo(objParamsDTO);
        }

        public List<tblM_CatMaquina> GetMaquina(string areaCuenta, string idCatMaquina)
        {
            return BackLogsDAO.GetMaquina(areaCuenta, idCatMaquina);
        }

        public tblM_CapHorometro GetHorometroActual(string areaCuenta, string noEconomico)
        {
            return BackLogsDAO.GetHorometroActual(areaCuenta, noEconomico);
        }

        public Dictionary<string, object> GetCantBLObligatorios(string areaCuenta, string noEconomico)
        {
            return BackLogsDAO.GetCantBLObligatorios(areaCuenta, noEconomico);
        }

        public List<tblM_CatModeloEquipo> GetModeloEquipo(List<int> idModelo)
        {
            return BackLogsDAO.GetModeloEquipo(idModelo);
        }

        public List<tblBL_CatBackLogs> GetUltimoFolio(bool esObra)
        {
            return BackLogsDAO.GetUltimoFolio(esObra);
        }

        public List<tblBL_CatBackLogs> VerificarDisponibilidadFolio(string folio)
        {
            return BackLogsDAO.VerificarDisponibilidadFolio(folio);
        }

        public bool EliminarBackLog(int id)
        {
            return BackLogsDAO.EliminarBackLog(id);
        }

        public bool EliminarParte(int id)
        {
            return BackLogsDAO.EliminarParte(id);
        }

        public List<tblBL_ManoObra> GetManoObra(int idBackLog)
        {
            return BackLogsDAO.GetManoObra(idBackLog);
        }

        public bool EliminarManoObra(int id)
        {
            return BackLogsDAO.EliminarManoObra(id);
        }

        public List<ComboDTO> FillPeriodos(int anio)
        {
            return BackLogsDAO.FillPeriodos(anio);
        }

        public bool VerificarEntradaAlmacenOC()
        {
            return BackLogsDAO.VerificarEntradaAlmacenOC();
        }

        public bool ConfirmarRehabilitacionProgramada(int idBL)
        {
            return BackLogsDAO.ConfirmarRehabilitacionProgramada(idBL);
        }

        public bool ConfirmarProcesoInstalacion(int idBL)
        {
            return BackLogsDAO.ConfirmarProcesoInstalacion(idBL);
        }

        public bool ConfirmarBackLogInstalado(int idBL)
        {
            return BackLogsDAO.ConfirmarBackLogInstalado(idBL);
        }

        public bool ExisteEvidenciaLiberacion(int idBL)
        {
            return BackLogsDAO.ExisteEvidenciaLiberacion(idBL);
        }

        public List<BackLogsDTO> FillTablaSolicitudPpto(BackLogsDTO objBL)
        {
            return BackLogsDAO.FillTablaSolicitudPpto(objBL);
        }

        public bool SolicitarAutorizacion(int idInsp, int idUsuario, SeguimientoPptoDTO objSegPpto, List<string> lstFoliosBL)
        {
            return BackLogsDAO.SolicitarAutorizacion(idInsp, idUsuario, objSegPpto, lstFoliosBL);
        }

        public bool CancelarRequisicion(tblBL_MotivoCancelacionReq objMotivo)
        {
            return BackLogsDAO.CancelarRequisicion(objMotivo);
        }

        public Dictionary<string, object> GetIDOT(int idBL)
        {
            return BackLogsDAO.GetIDOT(idBL);
        }

        public bool GetTipoBL(int idBL)
        {
            return BackLogsDAO.GetTipoBL(idBL);
        }

        public Dictionary<string, object> MostrarEvidencia(int idEvidencia)
        {
            return BackLogsDAO.MostrarEvidencia(idEvidencia);
        }

        public List<FoliosTraspasosDTO> GetFoliosTraspasos(int idBL)
        {
            return BackLogsDAO.GetFoliosTraspasos(idBL);
        }

        public bool CrearEditarTraspasoFolio(FoliosTraspasosDTO objCE)
        {
            return BackLogsDAO.CrearEditarTraspasoFolio(objCE);
        }

        public bool VerificarTraspasosBL(string areaCuenta)
        {
            return BackLogsDAO.VerificarTraspasosBL(areaCuenta);
        }

        public bool EliminarFolioTraspaso(int idFolioTraspaso)
        {
            return BackLogsDAO.EliminarFolioTraspaso(idFolioTraspaso);
        }
        
        public bool CambiarEstatusBL90a80(int idBL)
        {
            return BackLogsDAO.CambiarEstatusBL90a80(idBL);
        }

        public Dictionary<string, object> VisualizarEvidencia(int idEvidencia)
        {
            return BackLogsDAO.VisualizarEvidencia(idEvidencia);
        }

        public List<tblBL_Partes> GetPartesRelBL(int idBL)
        {
            return BackLogsDAO.GetPartesRelBL(idBL);
        }

        public List<ParteDTO> GetPartesRptOrdebBL(int idBL)
        {
            return BackLogsDAO.GetPartesRptOrdebBL(idBL);
        }
        #endregion

        #region CATALOGO CONJUNTOS
        public List<CatConjuntosDTO> GetConjuntos()
        {
            return BackLogsDAO.GetConjuntos();
        }

        public bool CrearConjunto(tblBL_CatConjuntos objConjunto)
        {
            return BackLogsDAO.CrearConjunto(objConjunto);
        }

        public bool ActualizarConjunto(tblBL_CatConjuntos objConjunto)
        {
            return BackLogsDAO.ActualizarConjunto(objConjunto);
        }

        public bool EliminarConjunto(int idConjunto)
        {
            return BackLogsDAO.EliminarConjunto(idConjunto);
        }
        #endregion

        #region CATALOGO SUBCONJUNTOS
        public List<CatSubconjuntosDTO> GetSubconjuntos()
        {
            return BackLogsDAO.GetSubconjuntos();
        }

        public bool CrearSubconjunto(tblBL_CatSubconjuntos objSubconjunto)
        {
            return BackLogsDAO.CrearSubconjunto(objSubconjunto);
        }

        public bool ActualizarSubconjunto(tblBL_CatSubconjuntos objSubconjunto)
        {
            return BackLogsDAO.ActualizarSubconjunto(objSubconjunto);
        }

        public bool EliminarSubconjunto(int idSubconjunto)
        {
            return BackLogsDAO.EliminarSubconjunto(idSubconjunto);
        }
        #endregion

        #region REQUISICIONES
        public List<RequisicionesDTO> GetRequisiciones(RequisicionesDTO objRequisicion)
        {
            return BackLogsDAO.GetRequisiciones(objRequisicion);
        }

        public bool ValidaNumeroRequisicion(tblBL_Requisiciones objRequisiciones)
        {
            return BackLogsDAO.ValidaNumeroRequisicion(objRequisiciones);
        }

        public bool ActualizarRequisicion(tblBL_Requisiciones objRequisiciones)
        {
            return BackLogsDAO.ActualizarRequisicion(objRequisiciones);
        }

        public bool CrearRequisicion(tblBL_Requisiciones objRequisiciones)
        {
            return BackLogsDAO.CrearRequisicion(objRequisiciones);
        }

        public bool EliminarRequisicion(int id)
        {
            return BackLogsDAO.EliminarRequisicion(id);
        }

        public List<RequisicionesDTO> GetAllRequisiciones(RequisicionesDTO objReq)
        {
            return BackLogsDAO.GetAllRequisiciones(objReq);
        }

        public List<RequisicionesDTO> GetAllDetRequisiciones(RequisicionesDTO objReq)
        {
            return BackLogsDAO.GetAllDetRequisiciones(objReq);
        }

        public BackLogsDTO GetDatosBL(int idBL)
        {
            return BackLogsDAO.GetDatosBL(idBL);
        }

        public List<MotivoCancelacionReq> GetMotivosCancelacion(int idBL)
        {
            return BackLogsDAO.GetMotivosCancelacion(idBL);
        }
        #endregion

        #region ORDENES COMPRAS
        public List<OrdenCompraDTO> GetOrdenesCompra(OrdenCompraDTO objOC)
        {
            return BackLogsDAO.GetOrdenesCompra(objOC);
        }

        public bool ActualizarOC(tblBL_OrdenesCompra objOC)
        {
            return BackLogsDAO.ActualizarOC(objOC);
        }

        public bool CrearOC(List<tblBL_OrdenesCompra> objOC)
        {
            return BackLogsDAO.CrearOC(objOC);
        }

        public bool EliminarOC(int id)
        {
            return BackLogsDAO.EliminarOC(id);
        }

        public List<OrdenCompraDTO> GetAllOrdenesCompra(OrdenCompraDTO objOC)
        {
            return BackLogsDAO.GetAllOrdenesCompra(objOC);
        }

        public List<OrdenCompraDTO> GetLstOcReq(OrdenCompraDTO objOC)
        {
            return BackLogsDAO.GetLstOcReq(objOC);
        }

        public List<OrdenCompraDTO> GetLstDetOcReq(OrdenCompraDTO objOC)
        {
            return BackLogsDAO.GetLstDetOcReq(objOC);
        }

        public bool GuardarOC(OrdenCompraDTO objOC)
        {
            return BackLogsDAO.GuardarOC(objOC);
        }

        public bool VerificarDuplicadoOC(OrdenCompraDTO objOC)
        {
            return BackLogsDAO.VerificarDuplicadoOC(objOC);
        }
        #endregion

        #region INSPECCIONES
        public List<InspeccionesDTO> GetInspecciones(InspeccionesDTO objFiltro)
        {
            return BackLogsDAO.GetInspecciones(objFiltro);
        }

        public List<ComboDTO> FillCboNoEconomico(string areaCuenta, List<int> lstGrupos)
        {
            return BackLogsDAO.FillCboNoEconomico(areaCuenta, lstGrupos);
        }

        public int GetPeriodoActual()
        {
            return BackLogsDAO.GetPeriodoActual();
        }

        public bool GuardarInspecciones(List<tblBL_Inspecciones> objInspecciones)
        {
            return BackLogsDAO.GuardarInspecciones(objInspecciones);
        }

        public bool ActualizarInspeccion(tblBL_Inspecciones objInspecciones)
        {
            return BackLogsDAO.ActualizarInspeccion(objInspecciones);
        }

        public bool EliminarInspeccionObra(int id)
        {
            return BackLogsDAO.EliminarInspeccionObra(id);
        }

        public List<InspeccionesObraExcelDTO> postObtenerTablaInspecciones(InspeccionesDTO parametros, bool ActivarHeader)
        {
            return BackLogsDAO.postObtenerTablaInspecciones(parametros, ActivarHeader);
        }

        public MemoryStream GenerarExcelInspeccionesObras(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.GenerarExcelInspeccionesObras(objParamsDTO);
        }

        public MemoryStream crearExcelInspeccionesTMC(string AreaCuenta)
        {
            return BackLogsDAO.crearExcelInspeccionesTMC(AreaCuenta);
        }
        #endregion

        #region REPORTE E INDICADORES
        public List<ComboDTO> FillCboResponsables()
        {
            return BackLogsDAO.FillCboResponsables();
        }

        public Dictionary<string, object> GetGraficaBLDias(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.GetGraficaBLDias(objParamsDTO);
        }

        public Dictionary<string, object> GetGraficaConjuntos(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.GetGraficaConjuntos(objParamsDTO);
        }

        public List<BackLogsDTO> GetPorEquipoDet(BackLogsDTO objFiltro)
        {
            return BackLogsDAO.GetPorEquipoDet(objFiltro);
        }

        public List<ComboDTO> FillCboResponsablesAnalisisBLResponsable(string areaCuenta)
        {
            return BackLogsDAO.FillCboResponsablesAnalisisBLResponsable(areaCuenta);
        }

        public BackLogsDTO GetBLOTVacia(int idBL)
        {
            return BackLogsDAO.GetBLOTVacia(idBL);
        }

        public Dictionary<string, object> GetTotalInfoEconomicoBL(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.GetTotalInfoEconomicoBL(objParamsDTO);
        }
        #endregion

        public List<BackLogsDTO> GetBackLogsPresupuesto(string noEconomico)
        {
            return BackLogsDAO.GetBackLogsPresupuesto(noEconomico);
        }

        public Dictionary<string, object> GetBackLogsGraficaresponsable(int inicioMes, int finMes, string areaCuenta, List<int> _lstResponsables, int inicioAnio, int finAnio)
        {
            return BackLogsDAO.GetBackLogsGraficaresponsable(inicioMes, finMes, areaCuenta, _lstResponsables, inicioAnio, finAnio);
        }
        #endregion

        #region BACKLOGS TMC
        public List<ComboDTO> FillAreasCuentasTMC()
        {
            return BackLogsDAO.FillAreasCuentasTMC();
        }

        public List<BackLogsDTO> GetBackLogsFiltrosTMC(BackLogsDTO objBackLog)
        {
            return BackLogsDAO.GetBackLogsFiltrosTMC(objBackLog);
        }

        public List<ComboDTO> FillCboModeloTMC()
        {
            return BackLogsDAO.FillCboModeloTMC();
        }

        public List<ComboDTO> FillCboGrupoTMC()
        {
            return BackLogsDAO.FillCboGrupoTMC();
        }

        public List<ProgramaInspTMCDTO> GetProgramacionInspeccionTMC(ProgramaInspTMCDTO objFiltro)
        {
            return BackLogsDAO.GetProgramacionInspeccionTMC(objFiltro);
        }

        public List<ProgramaInspTMCDTO> GetProgramaInspeccionTMC(ProgramaInspTMCDTO objFiltro)
        {
            return BackLogsDAO.GetProgramaInspeccionTMC(objFiltro);
        }

        public bool CrearProgramacionTMC(List<tblBL_InspeccionesTMC> objInsp)
        {
            return BackLogsDAO.CrearProgramacionTMC(objInsp);
        }

        public bool EliminarProgramaInspTMC(int id)
        {
            return BackLogsDAO.EliminarProgramaInspTMC(id);
        }

        public bool ActualizarProgramaInspeccionTMC(tblBL_InspeccionesTMC objInsp)
        {
            return BackLogsDAO.ActualizarProgramaInspeccionTMC(objInsp);
        }

        public List<BackLogsDTO> GetBLPptos(int idSegPpto)
        {
            return BackLogsDAO.GetBLPptos(idSegPpto);
        }

        public Dictionary<string, object> GetReporteIndicadores(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.GetReporteIndicadores(objParamsDTO);
        }

        public Dictionary<string, object> GetReporteIndicadoresGrafica(string areaCuenta)
        {
            return BackLogsDAO.GetReporteIndicadoresGrafica(areaCuenta);
        }

        public Dictionary<string, object> GetInfoGraficaResponsables(BackLogsDTO objParamsDTO)
        {
            return BackLogsDAO.GetInfoGraficaResponsables(objParamsDTO);
        }

        //public Dictionary<string, object> GetReportePorResponsables()
        public Dictionary<string, object> GetReportePorResponsables(BackLogsDTO objFiltro, DateTime fechaInicio, DateTime fechaFin)
        {
            return BackLogsDAO.GetReportePorResponsables(objFiltro, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> GetIndicadorBacklogPorEquipo(string AreaCuenta, DateTime fechaInicio, DateTime fechaFin, TipoMaquinaEnum tipoEquipo, EstatusBackLogEnum estatus)
        {
            return BackLogsDAO.GetIndicadorBacklogPorEquipo(AreaCuenta, fechaInicio, fechaFin, tipoEquipo, estatus);
        }

        #region CATÁLOGO DE FRENTES
        public List<ComboDTO> FillcboUsuarios()
        {
            return BackLogsDAO.FillcboUsuarios();
        }

        public List<CatFrentesDTO> GetFrentes()
        {
            return BackLogsDAO.GetFrentes();
        }

        public bool CrearFrente(tblBL_CatFrentes objFrente)
        {
            return BackLogsDAO.CrearFrente(objFrente);
        }

        public bool ActualizarFrente(tblBL_CatFrentes objFrente)
        {
            return BackLogsDAO.ActualizarFrente(objFrente);
        }

        public bool EliminarFrente(int idFrente)
        {
            return BackLogsDAO.EliminarFrente(idFrente);
        }

        public List<ComboDTO> FillTipoMaquinariaTMC()
        {
            return BackLogsDAO.FillTipoMaquinariaTMC();
        }
        #endregion

        #region SEGUIMIENTO PPTO
        public List<SeguimientoPptoDTO> GetSeguimientoPpto(string AreaCuenta, string motivo, int estatusPpto)
        {
            return BackLogsDAO.GetSeguimientoPpto(AreaCuenta, motivo, estatusPpto);
        }

        public List<SeguimientoPptoDTO> GetSeguimientoPptoFrentes(string AreaCuenta, string ObraoRenta, int idFrente)
        {
            return BackLogsDAO.GetSeguimientoPptoFrentes(AreaCuenta, ObraoRenta, idFrente);
        }

        //public List<SeguimientoPptoDTO> GetSeguimientoPptoFiltros(tblBL_SeguimientoPptos obj)
        //{
        //    return BackLogsDAO.GetSeguimientoPptoFiltros(obj);
        //}

        public List<ComboDTO> FillCboFrentes()
        {
            return BackLogsDAO.FillCboFrentes();
        }
        #endregion

        #region DETALLE FRENTE
        public List<DetFrentesDTO> GetDetFrentes(string AreaCuenta, List<int> lstFrentes, int estatusSeguimientoFrente)
        {
            return BackLogsDAO.GetDetFrentes(AreaCuenta, lstFrentes, estatusSeguimientoFrente);
        }
        public bool CrearDetFrentes(List<Seguimiento2DTO> parametros)
        {
            return BackLogsDAO.CrearDetFrentes(parametros);
        }

        public bool EliminarDetFrente(int idFrente)
        {
            return BackLogsDAO.EliminarDetFrente(idFrente);
        }
        #endregion

        #region INFORME
        public Dictionary<string, object> postSubirArchivos(int id, List<HttpPostedFileBase> archivo, int tipoEvidencia)
        {
            return BackLogsDAO.postSubirArchivos(id, archivo, tipoEvidencia);
        }

        public Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio)
        {
            return BackLogsDAO.obtenerArchivoCODescargas(idFormatoCambio);
        }

        public bool EliminarArchivos(int id)
        {
            return BackLogsDAO.EliminarArchivos(id);
        }

        public List<BackLogs_ArchivosDTO> GetArchivos(int id)
        {
            return BackLogsDAO.GetArchivos(id);
        }

        public bool ActualizarLiberacion(string areaCuenta)
        {
            return BackLogsDAO.ActualizarLiberacion(areaCuenta);
        }

        public Dictionary<string, object> GetModeloGrupoCCSeleccionado(string noEconomico)
        {
            return BackLogsDAO.GetModeloGrupoCCSeleccionado(noEconomico);
        }
        #endregion

        #region REPORTE GENERAL
        public Dictionary<string, object> GetReporteGeneral(int tipoBL, DateTime fechaInicio, DateTime fechaFin, string ac)
        {
            return BackLogsDAO.GetReporteGeneral(tipoBL, fechaInicio, fechaFin, ac);
        }
        #endregion

        #region INDICADORES DE REHABILITACION TMC
        public Dictionary<string, object> GetIndicadoresRehabilitacionTMC(IndicadoresRehabilitacionTMCDTO objFiltro)
        {
            return BackLogsDAO.GetIndicadoresRehabilitacionTMC(objFiltro);
        }

        public List<BackLogsDTO> GetBackLogs(int id)
        {
            return BackLogsDAO.GetBackLogs(id);
        }
        #endregion

        public bool ModificarEstatus(int id, int idLogeado, int Autorizante, int Estatus, string Descripcion)
        {
            return BackLogsDAO.ModificarEstatus(id, idLogeado, Autorizante, Estatus, Descripcion);
        }
        #endregion

        #region EXCEL CARGO DE NÓMINA
        public Dictionary<string, object> FillComboCentroCostoBackLogs()
        {
            return BackLogsDAO.FillComboCentroCostoBackLogs();
        }

        public Dictionary<string, object> GetGraficaCargoNomina(List<string> listaEconomicos, DateTime fechaInicio, DateTime fechaFin)
        {
            return BackLogsDAO.GetGraficaCargoNomina(listaEconomicos, fechaInicio, fechaFin);
        }

        public MemoryStream DescargarExcelCargoNomina(string imagenString)
        {
            return BackLogsDAO.DescargarExcelCargoNomina(imagenString);
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> FillCboTipoMonedas()
        {
            return BackLogsDAO.FillCboTipoMonedas();
        }

        public Dictionary<string, object> FillCboAC()
        {
            return BackLogsDAO.FillCboAC();
        }

        public Dictionary<string, object> FillCboTipoEquipo()
        {
            return BackLogsDAO.FillCboTipoEquipo();
        }
        #endregion
    }
}