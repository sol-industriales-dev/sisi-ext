using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.DTO.Principal.Generales;
using Core.DTO.Contabilidad.Nomina.PolizaNomina;
using System.Web;
using Core.DTO.Contabilidad.Nomina.CuentaEmpleado;
using Core.DTO.Contabilidad.Poliza;
using Core.Enum.Administracion.Propuesta.Nomina;
using System.IO;
using Core.DTO.Utils.Auth;
using Core.DTO.Contabilidad.Nomina;
using Core.DTO.Contabilidad.Nomina.AcomuladoMensual;
using Core.DTO.Contabilidad.Nomina.ReporteRangoCC;
using Core.DTO.Contabilidad.Nomina.ReporteEmpleadoCC;
using Core.Entity.RecursosHumanos.Captura;
using Core.DTO.Enkontrol.Alamcen;
using Core.Enum.Administracion.Nomina;

namespace Core.DAO.Contabilidad.Nomina
{
    public interface INominaDAO
    {
        #region General
        Dictionary<string, object> PeriodosNomina(int tipoNomina);
        Dictionary<string, object> PeriodosNominaAguinaldo();
        #endregion

        #region Raya
        List<tblC_Nom_PreNomina_Det> ConvertCSVTABtoPrenomina(byte[] file);
        List<ComboDTO> FillCboCentroCostros();
        #endregion

        #region Raya
        Dictionary<string, object> CargarRaya(HttpPostedFileBase raya, int periodo, int tipoPeriodo, int year, int tipoRaya);
        Dictionary<string, object> CargarSUA(HttpPostedFileBase raya, int periodo, int tipoPeriodo, int year, int tipoRaya);
        Dictionary<string, object> GetTipoRaya();
        Dictionary<string, object> GetClasificacionCC();
        Dictionary<string, object> GetRayaCargada(int periodo, int tipoPeriodo, int year, int tipoRaya, int clasificacionCC);
        Dictionary<string, object> GetRayaDetalleCargada(int nominaId);
        #endregion

        #region Nómina
        Dictionary<string, object> GetNominas(int year, int tipoPeriodo, int periodo, int tipoRaya);
        Dictionary<string, object> ResumenNomina(int nominaId);
        Dictionary<string, object> ResumenNominaAguinaldo(int year, string cc);
        Dictionary<string, object> ValidarNomina(int nominaId, List<ResumenDetalleNominaDTO> resumen);
        Dictionary<string, object> GenerarPoliza(int nominaId, DateTime fechaPol);
        Dictionary<string, object> RegistrarPoliza(PolizaMovPolEkDTO poliza_movimientos, int tipoRayaId);
        Dictionary<string, object> DescargarPoliza(int idNomina);
        MemoryStream DescargarExcelNomina(int tipo_nomina, int anio, int periodo);
        #endregion

        #region SUA
        Dictionary<string, object> GetSUA(int tipoDocumento, int year, int periodo);
        Dictionary<string, object> GenerarPolizaSUA(int suaId, DateTime fecha, int tipoDocumento);
        Dictionary<string, object> RegistrarPolizaSUA(PolizaMovPolEkDTO poliza_movimientos, int suaId, ClasificacionDocumentosSUAEnum tipoDocumento);
        MemoryStream DescargarExcelSUA(int tipo_nomina, int anio, int periodo, int tipo_documento);
        #endregion

        #region Empleado
        Dictionary<string, object> GetTipoCuenta();
        Dictionary<string, object> CatalogoCuentaEmpleado(int? tipoCuentaId, string cc);
        Dictionary<string, object> RegistrarEmpleado(tblC_Nom_CuentaEmpleado empleado);
        Dictionary<string, object> EliminarEmpleado(int id);
        Dictionary<string, object> ModificarEmpleado(tblC_Nom_CuentaEmpleado empleado);
        Dictionary<string, object> ValidarCuentaEmpleado(List<ValidarCuentaEmpleadoDTO> ids);
        Dictionary<string, object> RelacionarCuentasEmpleadosAutomaticamente();
        #endregion

        #region Prenomina
        Dictionary<string, object> CargarPrenomina(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);
        Dictionary<string, object> GuardarPrenomina(int prenominaID, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes, string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);
        MemoryStream crearExcelPrenomina(int prenominaID, int tipoReporte);
        MemoryStream crearExcelPrenominaPeru(int prenominaID, int tipoReporte);
        MemoryStream crearExcelPrenominaColombia(int prenominaID, int tipoReporte);
        MemoryStream crearExcelAguinaldo(int prenominaID, int tipoReporte);
        MemoryStream crearExcelPrenominasPorPeriodo(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo, int banco);
        byte[] ExcelPrenominasPorPeriodo(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo, int banco);
        byte[] ExcelAguinaldoPorPeriodo(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo, int banco);
        string GetPeriodoNomina(int year, int periodo, int tipoNomina);
        Dictionary<string, object> ValidarPrenomina(int prenominaID, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes);
        Dictionary<string, object> DesValidarPrenomina(int prenominaID);
        Dictionary<string, object> GetCCsIncidencias(tipoNominaPropuestaEnum tipoNomina, int periodo, int anio);
        Dictionary<string, object> GetUsuariosAutorizantes();
        tblC_Nom_Prenomina GetPrenominaByID(int prenominaID);
        List<tblC_Nom_PreNomina_Det> GetPrenominDetallesaByID(int prenominaID);
        List<tblC_Nom_PreNomina_Aut> GetPrenominaAutorizantesByID(int prenominaID);
        List<string> GetCorreosUsuarios(List<int> usuariosIds);
        Dictionary<string, object> GenerarReciboNomina(ReciboNominaDTO objParamsDTO);
        #endregion

        #region Gestion Prenomina
        Dictionary<string, object> GetLstGestionPrenomina(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio, string estatus);
        Dictionary<string, object> GetListaAutorizantes(int prenominaID);
        Dictionary<string, object> AutorizarPrenomina(authDTO auth);
        Dictionary<string, object> RechazarPrenomina(authDTO auth);
        #endregion

        #region Alerta Autorizacion Prenomina
        Dictionary<string, object> CargarPrenominasValidadas(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);
        List<authDTO> GetListaAutorizantes(List<int> prenominaID);
        List<tblC_Nom_PreNomina_Aut> GetListaAutorizantesOficina(List<int> prenominaIDs);
        bool AplicarPrenominaNotificadaOficina(List<int> prenominasID);
        List<tblC_Nom_Prenomina> GetPrenominasPeriodo(int periodo, int anio, int tipoNomina);
        tblC_Nom_CatalogoCC GetCatalogoCC(string CC);
        Dictionary<string, object> VerificarCorreoDespacho(int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);
        byte[] DataExcelPrenomina(int prenominaID);
        List<tblC_Nom_Prenomina> GetPrenominasAutorizadas(int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);
        #endregion

        #region Reporte Nomina Anual

        List<reporteAnualDTO> GetReportes(string bottom, string top);
        MemoryStream crearReporte(List<string[]> lstReporte, string tp, string btm);

        #endregion

        #region Acomulado Mensual

        Dictionary<string, object> GetNominaDetalle(string botomDate, string topDate);
        MemoryStream crearReporteMensual(List<ReporteAcomuladoMensualDTO> lstReporte, ReporteAcomuladoMensualDTO listaRegHead, string numeroEmpleados, string periodoDate);

        #endregion

        #region Reporte Rango por Centro de costo

        List<ReporteRangoCCDTO> GetReportesCC(string bottom, string top);

        MemoryStream crearReporteCC(List<string[]> lstReporte, string tp, string btm);

        int GetNumEmpleados(string bottom, string top);

        #endregion

        #region Reporte rango por Centr de costo y Cuenta cta

        Dictionary<string, object> GetReporteCtroCta(string bottom, string top);

        MemoryStream crearReporteCentroCuenta(List<string[]> lstReporte, string tp, string btm);

        #endregion

        #region Reporte rango por Empleado y centro de costo

        Dictionary<string, object> GetReporteEmpleadoCC(string bottom, string top, string cc, string empleado, int tipoRaya);
        MemoryStream crearReporteEmpleadoCC(Dictionary<string, object> lstReporte, string tp, string btm);
        Dictionary<string, object> GetCentroCostos();
        Dictionary<string, object> GetListaEmpleados();
        #endregion

        #region Reporte Concentrado
        Dictionary<string, object> GetReporteConcentrado(DateTime? fechaInicial, DateTime? fechaFinal, int? tipoRaya, int? tipoNomina);
        #endregion

        SolicitudChequeReporteDTO GetSolicitudChequeReporte(int year, int periodo, int tipoNomina, int banco);
        CedulaCostosReporteDTO GetCedulaCostos(int year, int periodo, int tipoNomina, int banco);
        polizaOcsiReporteDTO GetPolizaOCSI(int year, int periodo, int tipoNomina, int banco);

        #region Layout Incidencias
        Dictionary<string, object> CargarLayoutIncidencias(int anio, int periodo, int tipo_nomina, string estatus);
        Tuple<MemoryStream, string, List<tblRH_BN_Incidencia>> DescargarExcelLayoutIncidencias(int tipo);
        Tuple<MemoryStream, string> DescargarExcelReporteIncidencias(int anio, int tipo_nomina, int periodo, bool autorizado);
        bool ActualizarIncidencias(List<tblRH_BN_Incidencia> incidencias);
        #endregion

        List<EmpleadoPendienteLiberacionDTO> getEmpleadosPendientesLiberacion();
        void guardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados);

        #region Descuentos
        Dictionary<string, object> GetDescuentos();
        Dictionary<string, object> GuardarNuevoDescuento(tblC_Nom_PreNomina_Descuento descuento);
        Dictionary<string, object> EditarDescuento(tblC_Nom_PreNomina_Descuento descuento);
        Dictionary<string, object> EliminarDescuento(tblC_Nom_PreNomina_Descuento descuento);
        Dictionary<string, object> CargarExcelDescuentos(HttpFileCollectionBase archivos);
        #endregion

        Dictionary<string, object> GetEstatusPeriodo(int tipo_nomina, int anio);
        string GetCCNominas(string cc);

        Dictionary<string, object> FillCboBancos(tipoNominaPropuestaEnum tipoNomina, int periodo);
        List<tblC_Nom_CatSolicitudCheque> GetBancos(tipoNominaPropuestaEnum tipoNomina);

        #region SUA
        Dictionary<string, object> GetTipoDocumento();
        Dictionary<string, object> GetSUACargado(int periodo, int tipoPeriodo, int year, int tipoRaya);
        #endregion

        #region PERU
        Dictionary<string, object> CargarPrenominaPeru(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);

        Dictionary<string, object> GuardarPrenominaPeru(int prenominaID, List<tblC_Nom_PreNominaPeru_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes, string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio);

        Dictionary<string, object> ValidarPrenominaPeru(int prenominaID, List<tblC_Nom_PreNominaPeru_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes);

        List<tblC_Nom_PreNominaPeru_Det> GetPrenominDetallesPeruByID(int prenominaID);

        #region CATALOGO AFP
        Dictionary<string, object> GetRegistrosPeruAFP();

        Dictionary<string, object> CrearEditarRegistroPeruAFP(PeruAFPDTO objParamsDTO);

        Dictionary<string, object> GetRegistroActualizarPeruAFP(PeruAFPDTO objParamsDTO);

        Dictionary<string, object> EliminarRegistroPeruAFP(PeruAFPDTO objParamsDTO);

        Dictionary<string, object> FillCboCatAFP();

        Dictionary<string, object> SetAnioMes();
        #endregion

        #endregion
    }
}
