using Core.DTO;
using Core.DTO.Administracion.DocumentosXPagar;
using Core.DTO.Administracion.DocumentosXPagar.Poliza_revaluacion;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.DocumentosXPagar;
using Core.DTO.Contabilidad.DocumentosXPagar.PQ;
using Core.DTO.Contabilidad.DocumentosXPagar.Reportes;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Entity.Administrativo.DocumentosXPagar;
using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Core.DAO.Administracion.DocumentosXPagar
{
    public interface IContratosDAO
    {
        #region Cedula
        Dictionary<string, object> GetCedula(DateTime fechaCorte);
        #endregion

        Respuesta ObtenerContratos(FiltroContratosDTO filtro);
        //Respuesta GuardarContrato(tblAF_DxP_Contrato contrato);
        Respuesta GuardarDeudas(List<tblAF_DxP_Deuda> objDeudas, tblC_sc_polizas poliza, List<tblC_sc_movpol> movPolList);
        Respuesta AgregarMaquina(AgregarMaquinaDTO maquina);
        Respuesta ObtenerMaquinas(int idContrato);
        Respuesta ObtenerDesgloseGeneral(int idContrato);
        Respuesta ObtenerDesglosePorMaquina(int idContratoMaquina);
        Dictionary<string, object> ObtenerMaquinas();
        Dictionary<string, object> ObtenerInstituciones();
        Dictionary<string, object> ObtenerContratoByID(int id);
        Dictionary<string, object> ObtenerPagos(int contratoID, int parcialidad);
        Dictionary<string, object> SaveOrupdatepagos(tblAF_DxP_Pago dxpPago, List<tblAF_DxP_PagoMaquina> dxpPagoMaquina, List<tblAF_DxP_ContratoMaquinaDetalle> dxpContratoMaquinaDetalle);
        Dictionary<string, object> GuardarFechaNuevoPeriodo(int contratoID, DateTime nuevaFecha, int parcialidad);
        Dictionary<string, object> ObtenerContratosNotificaciones();
        Dictionary<string, object> GuardarProgramacion(List<tblAF_DxP_ProgramacionPagos> obj);
        Dictionary<string, object> CargarPropuesta(DateTime inicio, DateTime final, int estatus, int institucion);
        Dictionary<string, object> loadPropuestas(int? idInstitucion, int empresa, int moneda);
        ReporteDTO DesgloseGeneral(int idContrato);
        Dictionary<string, object> CargarContrato(int contratoId, int parcialidad, DateTime fechaPol);
        Dictionary<string, object> guardarPoliza(tblC_sc_polizas poliza, List<tblC_sc_movpol> movpol, List<listaContrato> contrato, decimal tipoCambio);
        Dictionary<string, object> GetProveedores();
        Dictionary<string, object> guardarInstitucion(string descripcion);
        Respuesta GuardarContrato(tblAF_DxP_Contrato contrato, List<AgregarMaquinaDTO> listaEconomicos, bool tasaFija);
        Dictionary<string, object> GetTipoCambioFecha(DateTime fecha);
        List<tblC_FED_DetProyeccionCierre> getLstContratos(BusqProyeccionCierreDTO busq);
        List<PropuestaArrendadoraDTO> LoadReportePropuestaArrendadora(DateTime pfechaInicio, DateTime pfechaFin);
        Dictionary<string, object> LoadProgramacionPagos(DateTime pInicio, DateTime pFinal, int pEstatus, List<int> institucion, int empresa, int moneda);
        List<autoCompleteCta> autoComplCatCtas(string term);
        List<autoCompleteCta> autoCompleteCtasIA(string term);
        Dictionary<string, object> CargarDetallePago(int parcialidad, int contratoId);
        Dictionary<string, object> getRptAdeudosGeneral(List<int> tipoMoneda, List<int> anio, List<int> institucion, bool tipoArrendamiento);

        Dictionary<string, object> getRptAdeudosDetalle(int tipoMoneda, List<int> instituciones, DateTime fechaFin, int tipo, List<bool> tipoArre);
        ComboDTO getArchivoDownLoad(int contratoID);
        Dictionary<string, object> getPolizaByFecha(DateTime fecha);
        Dictionary<string, object> comboCbosData();
        Dictionary<string, object> LoadContratosProgramados(DateTime pInicio, DateTime pFinal, string cc);
        Dictionary<string, object> LoadContratosProgramadosCplan(DateTime pInicio, DateTime pFinal, string cc);
        Dictionary<string, object> ActualizarContratos(List<tblAF_DxP_ProgramacionPagos> arrayProgramacionID);

        List<ctaDTO> GetCtaList();

        Dictionary<string, object> LoadCtas();

        Dictionary<string, object> TerminarContrato(int contratoID);

        Dictionary<string, object> UpdateContratosDet(int contratoID);

        Dictionary<string, object> UpdateContratoArchivo(int contratoID, string fileContrato);
        dtLoadCtaServerDTO LoadCtasServerSide();

        Dictionary<string, object> GetInfoLiquidar(bool liquidar, int contratoId, int parcialidad);

        #region REPORTE SALDO PENDIENTE POR PROYECTO
        Dictionary<string, object> ObtenerCboCC(List<int> lstDivisionID);

        Dictionary<string, object> ObtenerCboDivisiones();

        //List<adeudosDTO> ObtenerListadoDivisiones(adeudosDTO objDivision);

        Dictionary<string, object> ObtenerListadoDivisiones(adeudosDTO objDivision);
        #endregion

        #region CATALOGO DIVSIONES
        List<CatDivisionesDTO> GetDivisiones();
        CatDivisionesDTO GuardarDivisiones(tblAF_DxP_Divisiones parametros);
        bool EditarDivisiones(tblAF_DxP_Divisiones parametros);
        bool EliminarDivisiones(int id);

        #endregion

        #region DIVISIONES_PROYECTOS
        List<Divisiones_ProyectosDTO> GetDivisiones_Proyectos();
        List<Divisiones_ProyectosDTO> GetDivisiones_ProyectosFitro(tblAF_DxP_Divisiones_Proyecto objFiltro);

        List<ComboDTO> GetCC();    
        List<ComboDTO> GetCmbDivision();
        Divisiones_ProyectosDTO GuardarDivisiones_Proyectos(tblAF_DxP_Divisiones_Proyecto parametros);
        bool EliminarDivisionesProyectos(int id);
        bool EditarDivisionesProyectos(tblAF_DxP_Divisiones_Proyecto parametros);



        #endregion

        #region PQ
        Dictionary<string, object> GuardarPQ(tblAF_DxP_PQ pq, HttpPostedFileBase archivo);
        Dictionary<string, object> GetMonedas();
        Dictionary<string, object> GetPQs(bool estatus, string fechaCorte);
        Dictionary<string, object> GetPQ(int id);
        Dictionary<string, object> ObtenerInstitucionesPQ();
        Dictionary<string, object> GetPQLiquidar(int id);
        Dictionary<string, object> GetPQCambiarCC(int id);
        Dictionary<string, object> GetPQRenovar(int id);
        Dictionary<string, object> GetPQAbono(int id);
        Dictionary<string, object> Liquidar(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol);
        Dictionary<string, object> CambiarCC(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol);
        Dictionary<string, object> RenovarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol, DateTime fechaFirma, DateTime fechaVencimiento, decimal interes, HttpPostedFileBase archivo);
        Dictionary<string, object> AbonarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol);
        Dictionary<string, object> UrlArchivoPQ(int idPq);
        #endregion

        #region POLIZA REVALUACION
        Dictionary<string, object> GetInfoRevaluacion(DateTime fecha);
        Dictionary<string, object> RegistrarPolizaRevaluacion(PolizaMovPolEkDTO poliza);
        #endregion

        Dictionary<string, object> CargarReporteInteresesPagados(DateTime fecha);

        Dictionary<string, object> DescargarExcelInteresesPagados(DateTime fecha);
    }
}