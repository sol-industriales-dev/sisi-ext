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

namespace Core.DAO.Maquinaria.BackLogs
{
    public interface IBackLogsDAO
    {
        #region BACKLOGS OBRA
        #region INDEX
        Dictionary<string, object> GetBackLogsGraficaIndex(BackLogsDTO objDTO);
        #endregion

        #region REGISTRO BACKLOGS
        List<BackLogsDTO> GetBackLogsFiltros(BackLogsDTO objBackLog, bool esObra);

        decimal GetTotalOCRehabilitacion(GetTotalMXDTO objTotalDTO);

        List<InsumosDTO> GetInsumos(InsumosDTO objFiltro);

        Dictionary<string, object> GetDatosGraficasBLObra(BackLogsDTO objBL);

        Dictionary<string, object> GetDatosGraficasBLTMC(BackLogsDTO objBL);

        List<tblBL_CatBackLogs> GetNumBackLogs(string areaCuenta);

        int GetNumBacklogsAniosAnteriores(string areaCuenta);

        List<tblBL_Partes> GetPartes(int idBackLog);

        List<tblBL_CatSubconjuntos> GetSubconjuntos(List<int> idSubconjunto);

        List<tblBL_CatConjuntos> GetConjuntos(List<int> idConjunto);

        bool CrearBackLog(tblBL_CatBackLogs datosFormulario, List<tblBL_Partes> datosPartes, List<tblBL_ManoObra> datosManoObra, bool esParte, bool esManoObra, bool esObra, int idUsuarioResponsable);

        bool ActualizarBackLog(tblBL_CatBackLogs objBL, bool esActualizarCC);

        bool ActualizarParte(tblBL_Partes datosParte);

        bool CrearParte(tblBL_Partes datosParte);

        bool ActualizarManoObra(tblBL_ManoObra datosManoObra);

        bool CrearManoObra(tblBL_ManoObra datosManoObra);

        List<ComboDTO> FillAreasCuentas();

        List<ComboDTO> FillAreasCuentasTMC();

        decimal GetCostoPromedio(int almacen, int insumo);

        int RetornarAlmacen(string areaCuenta);

        List<ComboDTO> FillCboCC(string areaCuenta, bool esObra);

        List<ComboDTO> FillCboConjunto();

        List<ComboDTO> FillCboSubconjunto(int idConjunto);

        List<ComboDTO> FillCboModelo();

        List<ComboDTO> FillCboGrupo(BackLogsDTO objParamsDTO);

        List<tblM_CatMaquina> GetMaquina(string areaCuenta, string idCatMaquina);

        tblM_CapHorometro GetHorometroActual(string areaCuenta, string noEconomico);

        Dictionary<string, object> GetCantBLObligatorios(string areaCuenta, string noEconomico);

        List<tblM_CatModeloEquipo> GetModeloEquipo(List<int> idModelo);

        List<tblBL_CatBackLogs> GetUltimoFolio(bool esObra);

        List<tblBL_CatBackLogs> VerificarDisponibilidadFolio(string folioBL);

        bool EliminarBackLog(int id);

        bool EliminarParte(int id);

        bool EliminarManoObra(int id);

        List<tblBL_ManoObra> GetManoObra(int idBackLog);

        List<ComboDTO> FillPeriodos(int anio);

        bool VerificarEntradaAlmacenOC();

        bool ConfirmarRehabilitacionProgramada(int idBL);

        bool ConfirmarProcesoInstalacion(int idBL);

        bool ConfirmarBackLogInstalado(int idBL);

        bool ExisteEvidenciaLiberacion(int idBL);

        bool CancelarRequisicion(tblBL_MotivoCancelacionReq objMotivo);

        Dictionary<string, object> GetIDOT(int idBL);

        bool GetTipoBL(int idBL);

        Dictionary<string, object> MostrarEvidencia(int idEvidencia);

        List<FoliosTraspasosDTO> GetFoliosTraspasos(int idBL);

        bool CrearEditarTraspasoFolio(FoliosTraspasosDTO objCE);

        bool VerificarTraspasosBL(string areaCuenta);

        bool EliminarFolioTraspaso(int idFolioTraspaso);

        bool CambiarEstatusBL90a80(int idBL);

        Dictionary<string, object> VisualizarEvidencia(int idEvidencia);

        List<tblBL_Partes> GetPartesRelBL(int idBL);

        List<ParteDTO> GetPartesRptOrdebBL(int idBL);
        #endregion

        #region CATALOGO CONJUNTOS
        List<CatConjuntosDTO> GetConjuntos();

        bool CrearConjunto(tblBL_CatConjuntos objConjunto);

        bool ActualizarConjunto(tblBL_CatConjuntos objConjunto);

        bool EliminarConjunto(int idConjunto);
        #endregion

        #region CATALOGO SUBCONJUNTOS
        List<CatSubconjuntosDTO> GetSubconjuntos();

        bool CrearSubconjunto(tblBL_CatSubconjuntos objSubconjunto);

        bool ActualizarSubconjunto(tblBL_CatSubconjuntos objSubconjunto);

        bool EliminarSubconjunto(int idSubconjunto);
        #endregion

        #region REQUISICIONES
        List<RequisicionesDTO> GetRequisiciones(RequisicionesDTO objRequisicion);

        bool ValidaNumeroRequisicion(tblBL_Requisiciones objRequisiciones);

        bool ActualizarRequisicion(tblBL_Requisiciones objRequisiciones);

        bool CrearRequisicion(tblBL_Requisiciones objRequisiciones);

        bool EliminarRequisicion(int id);

        List<RequisicionesDTO> GetAllRequisiciones(RequisicionesDTO objReq);

        List<RequisicionesDTO> GetAllDetRequisiciones(RequisicionesDTO objReq);

        BackLogsDTO GetDatosBL(int idBL);

        List<MotivoCancelacionReq> GetMotivosCancelacion(int idBL);
        #endregion

        #region ORDENES DE COMPRA
        List<OrdenCompraDTO> GetOrdenesCompra(OrdenCompraDTO objOC);

        bool ActualizarOC(tblBL_OrdenesCompra objOC);

        bool CrearOC(List<tblBL_OrdenesCompra> objOC);

        bool EliminarOC(int id);
        List<OrdenCompraDTO> GetAllOrdenesCompra(OrdenCompraDTO objOC);

        List<OrdenCompraDTO> GetLstOcReq(OrdenCompraDTO objOC);

        List<OrdenCompraDTO> GetLstDetOcReq(OrdenCompraDTO objOC);

        bool GuardarOC(OrdenCompraDTO objOC);

        bool VerificarDuplicadoOC(OrdenCompraDTO objOC);
        #endregion

        #region INSPECCIONES
        List<InspeccionesDTO> GetInspecciones(InspeccionesDTO objFiltro);

        List<ComboDTO> FillCboNoEconomico(string areaCuenta, List<int> lstGrupos);

        int GetPeriodoActual();

        bool GuardarInspecciones(List<tblBL_Inspecciones> objInspecciones);

        bool ActualizarInspeccion(tblBL_Inspecciones objInspecciones);

        bool EliminarInspeccionObra(int id);

        List<InspeccionesObraExcelDTO> postObtenerTablaInspecciones(InspeccionesDTO parametros, bool ActivarHeader);

        MemoryStream GenerarExcelInspeccionesObras(BackLogsDTO objParamsDTO);

        MemoryStream crearExcelInspeccionesTMC(string AreaCuenta);


        #endregion

        #region REPORTE E INDICADORES
        List<ComboDTO> FillCboResponsables();

        Dictionary<string, object> GetReporteIndicadores(BackLogsDTO objParamsDTO);

        Dictionary<string, object> GetReporteIndicadoresGrafica(string areaCuenta);

        Dictionary<string, object> GetInfoGraficaResponsables(BackLogsDTO objParamsDTO);

        //Dictionary<string, object> GetReportePorResponsables();
        Dictionary<string, object> GetReportePorResponsables(BackLogsDTO objFiltro, DateTime fechaInicio, DateTime fechaFin);

        Dictionary<string, object> GetIndicadorBacklogPorEquipo(string AreaCuenta, DateTime fechaInicio, DateTime fechaFin, TipoMaquinaEnum tipoEquipo, EstatusBackLogEnum estatus);

        Dictionary<string, object> GetGraficaBLDias(BackLogsDTO objParamsDTO);

        Dictionary<string, object> GetGraficaConjuntos(BackLogsDTO objParamsDTO);

        List<BackLogsDTO> GetPorEquipoDet(BackLogsDTO objFiltro);

        List<ComboDTO> FillCboResponsablesAnalisisBLResponsable(string areaCuenta);

        BackLogsDTO GetBLOTVacia(int idBL);

        Dictionary<string, object> GetTotalInfoEconomicoBL(BackLogsDTO objParamsDTO);
        #endregion

        bool ModificarEstatus(int id, int idLogeado, int Autorizante, int Estatus, string Descripcion);
        #endregion

        #region BACKLOGS TMC
        List<BackLogsDTO> GetBackLogsFiltrosTMC(BackLogsDTO objBackLog);

        List<ComboDTO> FillCboModeloTMC();

        List<ComboDTO> FillCboGrupoTMC();

        List<ProgramaInspTMCDTO> GetProgramacionInspeccionTMC(ProgramaInspTMCDTO objFiltro);

        bool CrearProgramacionTMC(List<tblBL_InspeccionesTMC> objInsp);

        bool EliminarProgramaInspTMC(int id);

        List<ProgramaInspTMCDTO> GetProgramaInspeccionTMC(ProgramaInspTMCDTO objFiltro);

        bool ActualizarProgramaInspeccionTMC(tblBL_InspeccionesTMC objInsp);

        List<BackLogsDTO> FillTablaSolicitudPpto(BackLogsDTO objBL);

        bool SolicitarAutorizacion(int idInsp, int idUsuario, SeguimientoPptoDTO objSegPpto, List<string> lstFoliosBL);

        List<BackLogsDTO> GetBLPptos(int idSegPpto);

        #region CATÁLOGO DE FRENTES
        List<ComboDTO> FillcboUsuarios();

        List<CatFrentesDTO> GetFrentes();

        bool CrearFrente(tblBL_CatFrentes objFrente);

        bool ActualizarFrente(tblBL_CatFrentes objFrente);

        bool EliminarFrente(int idFrente);

        List<ComboDTO> FillTipoMaquinariaTMC();
        #endregion

        #region SEGUIMIENTO PPTO
        List<SeguimientoPptoDTO> GetSeguimientoPpto(string AreaCuenta, string motivo, int estatusPpto);
        List<SeguimientoPptoDTO> GetSeguimientoPptoFrentes(string AreaCuenta, string ObraoRenta, int idFrente);


        //List<SeguimientoPptoDTO> GetSeguimientoPptoFiltros(tblBL_SeguimientoPptos obj);


        List<ComboDTO> FillCboFrentes();

        #endregion

        #region DETALLE FRENTE
        List<DetFrentesDTO> GetDetFrentes(string AreaCuenta, List<int> lstFrentes, int estatusSeguimientoFrente);
        bool CrearDetFrentes(List<Seguimiento2DTO> parametros);

        bool EliminarDetFrente(int idFrente);

        #endregion

        #region INFORME TMC
        List<BackLogs_ArchivosDTO> GetArchivos(int id);
        bool EliminarArchivos(int id);

        Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio);

        Dictionary<string, object> postSubirArchivos(int id, List<HttpPostedFileBase> archivo, int tipoEvidencia);

        bool ActualizarLiberacion(string areaCuenta);

        Dictionary<string, object> GetModeloGrupoCCSeleccionado(string noEconomico);
        #endregion

        #region REPORTE GENERAL
        #endregion

        Dictionary<string, object> GetReporteGeneral(int tipoBL, DateTime fechaInicio, DateTime fechaFin, string ac);

        #region INDICADORES DE REHABILITACION TMC
        Dictionary<string, object> GetIndicadoresRehabilitacionTMC(IndicadoresRehabilitacionTMCDTO objFiltro);

        List<BackLogsDTO> GetBackLogs(int id);

        List<BackLogsDTO> GetBackLogsPresupuesto(string noEconomico);
        Dictionary<string, object> GetBackLogsGraficaresponsable(int inicioMes, int finMes, string areaCuenta, List<int> _lstResponsables, int inicioAnio, int finAnio);
        #endregion
        #endregion

        #region Excel Cargo Nómina
        Dictionary<string, object> FillComboCentroCostoBackLogs();
        Dictionary<string, object> GetGraficaCargoNomina(List<string> listaEconomicos, DateTime fechaInicio, DateTime fechaFin);
        MemoryStream DescargarExcelCargoNomina(string imagenString);
        #endregion

        #region GENERALES
        Dictionary<string, object> FillCboTipoMonedas();

        Dictionary<string, object> FillCboAC();

        Dictionary<string, object> FillCboTipoEquipo();
        #endregion
    }
}