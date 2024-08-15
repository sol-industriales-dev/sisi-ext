using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Contabilidad.Nomina;
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

namespace Core.Service.Contabilidad.Nomina
{
    public class NominaService : INominaDAO
    {
        private INominaDAO m_interfazDAO;

        private INominaDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }

        public NominaService(INominaDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }



        #region General
        public Dictionary<string, object> PeriodosNomina(int tipoNomina)
        {
            return interfazDAO.PeriodosNomina(tipoNomina);
        }
        public Dictionary<string, object> PeriodosNominaAguinaldo()
        {
            return interfazDAO.PeriodosNominaAguinaldo();
        }
        #endregion

        #region Raya
        public List<tblC_Nom_PreNomina_Det> ConvertCSVTABtoPrenomina(byte[] file) 
        {
            return interfazDAO.ConvertCSVTABtoPrenomina(file);
        }
        public List<ComboDTO> FillCboCentroCostros()
        {
            return interfazDAO.FillCboCentroCostros();
        }
        #endregion

        #region Raya
        public Dictionary<string, object> CargarRaya(HttpPostedFileBase raya, int periodo, int tipoPeriodo, int year, int tipoRaya)
        {
            return interfazDAO.CargarRaya(raya, periodo, tipoPeriodo, year, tipoRaya);
        }
        public Dictionary<string, object> CargarSUA(HttpPostedFileBase raya, int periodo, int tipoPeriodo, int year, int tipoRaya)
        {
            return interfazDAO.CargarSUA(raya, periodo, tipoPeriodo, year, tipoRaya);
        }

        public Dictionary<string, object> GetTipoRaya()
        {
            return interfazDAO.GetTipoRaya();
        }

        public Dictionary<string, object> GetClasificacionCC()
        {
            return interfazDAO.GetClasificacionCC();
        }

        public Dictionary<string, object> GetRayaCargada(int periodo, int tipoPeriodo, int year, int tipoRaya, int clasificacionCC)
        {
            return interfazDAO.GetRayaCargada(periodo, tipoPeriodo, year, tipoRaya, clasificacionCC);
        }

        public Dictionary<string, object> GetRayaDetalleCargada(int nominaId)
        {
            return interfazDAO.GetRayaDetalleCargada(nominaId);
        }
        #endregion

        #region Nómina
        public Dictionary<string, object> GetNominas(int year, int tipoPeriodo, int periodo, int tipoRaya)
        {
            return interfazDAO.GetNominas(year, tipoPeriodo, periodo, tipoRaya);
        }

        public Dictionary<string, object> ResumenNomina(int nominaId)
        {
            return interfazDAO.ResumenNomina(nominaId);
        }

        public Dictionary<string, object> ResumenNominaAguinaldo(int year, string cc)
        {
            return interfazDAO.ResumenNominaAguinaldo(year, cc);
        }

        public Dictionary<string, object> ValidarNomina(int nominaId, List<ResumenDetalleNominaDTO> resumen)
        {
            return interfazDAO.ValidarNomina(nominaId, resumen);
        }

        public Dictionary<string, object> GenerarPoliza(int nominaId, DateTime fechaPol)
        {
            return interfazDAO.GenerarPoliza(nominaId, fechaPol);
        }

        public Dictionary<string, object> RegistrarPoliza(PolizaMovPolEkDTO poliza_movimientos, int tipoRayaId)
        {
            return interfazDAO.RegistrarPoliza(poliza_movimientos, tipoRayaId);
        }

        public Dictionary<string, object> DescargarPoliza(int idNomina)
        {
            return interfazDAO.DescargarPoliza(idNomina);
        }

        public MemoryStream DescargarExcelNomina(int tipo_nomina, int anio, int periodo)
        {
            return interfazDAO.DescargarExcelNomina(tipo_nomina, anio, periodo);
        }
        #endregion

        #region SUA
        public Dictionary<string, object> GetSUA(int tipoDocumento, int year, int periodo)
        {
            return interfazDAO.GetSUA(tipoDocumento, year, periodo);
        }

        public Dictionary<string, object> GenerarPolizaSUA(int suaId, DateTime fecha, int tipoDocumento)
        {
            return interfazDAO.GenerarPolizaSUA(suaId, fecha, tipoDocumento);
        }

        public Dictionary<string, object> RegistrarPolizaSUA(PolizaMovPolEkDTO poliza_movimientos, int suaId, ClasificacionDocumentosSUAEnum tipoDocumento)
        {
            return interfazDAO.RegistrarPolizaSUA(poliza_movimientos, suaId, tipoDocumento);
        }

        public MemoryStream DescargarExcelSUA(int tipo_nomina, int anio, int periodo, int tipo_documento)
        {
            return interfazDAO.DescargarExcelSUA(tipo_nomina, anio, periodo, tipo_documento);
        }
        #endregion

        #region Empleado
        public Dictionary<string, object> CatalogoCuentaEmpleado(int? tipoCuentaId, string cc)
        {
            return interfazDAO.CatalogoCuentaEmpleado(tipoCuentaId, cc);
        }

        public Dictionary<string, object> GetTipoCuenta()
        {
            return interfazDAO.GetTipoCuenta();
        }

        public Dictionary<string, object> RegistrarEmpleado(tblC_Nom_CuentaEmpleado empleado)
        {
            return interfazDAO.RegistrarEmpleado(empleado);
        }

        public Dictionary<string, object> EliminarEmpleado(int id)
        {
            return interfazDAO.EliminarEmpleado(id);
        }

        public Dictionary<string, object> ModificarEmpleado(tblC_Nom_CuentaEmpleado empleado)
        {
            return interfazDAO.ModificarEmpleado(empleado);
        }

        public Dictionary<string, object> ValidarCuentaEmpleado(List<ValidarCuentaEmpleadoDTO> ids)
        {
            return interfazDAO.ValidarCuentaEmpleado(ids);
        }

        public Dictionary<string, object> RelacionarCuentasEmpleadosAutomaticamente()
        {
            return interfazDAO.RelacionarCuentasEmpleadosAutomaticamente();
        }
        #endregion

        #region Prenomina
        public Dictionary<string, object> CargarPrenomina(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.CargarPrenomina(CC, periodo, tipoNomina, anio);
        }
        public Dictionary<string, object> GuardarPrenomina(int prenominaID, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes, string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.GuardarPrenomina(prenominaID, detalles, autorizantes, CC, periodo, tipoNomina, anio);
        }
        public MemoryStream crearExcelPrenomina(int prenominaID, int tipoReporte)
        {
            return interfazDAO.crearExcelPrenomina(prenominaID, tipoReporte);
        }
        public MemoryStream crearExcelPrenominaPeru(int prenominaID, int tipoReporte)
        {
            return interfazDAO.crearExcelPrenominaPeru(prenominaID, tipoReporte);
        }
        public MemoryStream crearExcelPrenominaColombia(int prenominaID, int tipoReporte)
        {
            return interfazDAO.crearExcelPrenominaColombia(prenominaID, tipoReporte);
        }
        public MemoryStream crearExcelAguinaldo(int prenominaID, int tipoReporte)
        {
            return interfazDAO.crearExcelAguinaldo(prenominaID, tipoReporte);
        }
        public MemoryStream crearExcelPrenominasPorPeriodo(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo, int banco)
        {
            return interfazDAO.crearExcelPrenominasPorPeriodo(anio, tipoNomina, periodo, banco);
        }
        public byte[] ExcelPrenominasPorPeriodo(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo, int banco)
        {
            return interfazDAO.ExcelPrenominasPorPeriodo(anio, tipoNomina, periodo, banco);
        }
        public byte[] ExcelAguinaldoPorPeriodo(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo, int banco)
        {
            return interfazDAO.ExcelAguinaldoPorPeriodo(anio, tipoNomina, periodo, banco);
        }
        public string GetPeriodoNomina(int year, int periodo, int tipoNomina)
        {
            return interfazDAO.GetPeriodoNomina(year, periodo, tipoNomina);
        }
        public Dictionary<string, object> ValidarPrenomina(int prenominaID, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes)
        {
            return interfazDAO.ValidarPrenomina(prenominaID, detalles, autorizantes);
        }
        public Dictionary<string, object> DesValidarPrenomina(int prenominaID)
        {
            return interfazDAO.DesValidarPrenomina(prenominaID);
        }
        public Dictionary<string, object> GetCCsIncidencias(tipoNominaPropuestaEnum tipoNomina, int periodo, int anio)
        {
            return interfazDAO.GetCCsIncidencias(tipoNomina, periodo, anio);
        }
        public Dictionary<string, object> GetUsuariosAutorizantes()
        {
            return interfazDAO.GetUsuariosAutorizantes();
        }
        public tblC_Nom_Prenomina GetPrenominaByID(int prenominaID)
        {
            return interfazDAO.GetPrenominaByID(prenominaID);
        }
        public List<tblC_Nom_PreNomina_Det> GetPrenominDetallesaByID(int prenominaID)
        {
            return interfazDAO.GetPrenominDetallesaByID(prenominaID);
        }
        public List<tblC_Nom_PreNomina_Aut> GetPrenominaAutorizantesByID(int prenominaID)
        {
            return interfazDAO.GetPrenominaAutorizantesByID(prenominaID);
        }
        public List<string> GetCorreosUsuarios(List<int> usuariosIds)
        {
            return interfazDAO.GetCorreosUsuarios(usuariosIds);
        }
        public Dictionary<string, object> GenerarReciboNomina(ReciboNominaDTO objParamsDTO)
        {
            return interfazDAO.GenerarReciboNomina(objParamsDTO);
        }
        #endregion

        #region Gestion Prenomina
        public Dictionary<string, object> GetLstGestionPrenomina(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio, string estatus)
        {
            return interfazDAO.GetLstGestionPrenomina(CC, periodo, tipoNomina, anio, estatus);
        }
        public Dictionary<string, object> GetListaAutorizantes(int prenominaID)
        {
            return interfazDAO.GetListaAutorizantes(prenominaID);
        }
        public Dictionary<string, object> AutorizarPrenomina(authDTO auth)
        {
            return interfazDAO.AutorizarPrenomina(auth);
        }
        public Dictionary<string, object> RechazarPrenomina(authDTO auth)
        {
            return interfazDAO.RechazarPrenomina(auth);
        }
        #endregion

        #region Alerta Autorizacion Prenomina
        public Dictionary<string, object> CargarPrenominasValidadas(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.CargarPrenominasValidadas(CC, periodo, tipoNomina, anio);
        }
        public List<authDTO> GetListaAutorizantes(List<int> prenominaID)
        {
            return interfazDAO.GetListaAutorizantes(prenominaID);
        }
        public List<tblC_Nom_PreNomina_Aut> GetListaAutorizantesOficina(List<int> prenominaIDs)
        {
            return interfazDAO.GetListaAutorizantesOficina(prenominaIDs);
        }
        public bool AplicarPrenominaNotificadaOficina(List<int> prenominasID)
        {
            return interfazDAO.AplicarPrenominaNotificadaOficina(prenominasID);
        }
        public List<tblC_Nom_Prenomina> GetPrenominasPeriodo(int periodo, int anio, int tipoNomina)
        {
            return interfazDAO.GetPrenominasPeriodo(periodo, anio, tipoNomina);
        }
        public tblC_Nom_CatalogoCC GetCatalogoCC(string CC)
        {
            return interfazDAO.GetCatalogoCC(CC);
        }
        public Dictionary<string, object> VerificarCorreoDespacho(int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.VerificarCorreoDespacho(periodo, tipoNomina, anio);
        }
        public byte[] DataExcelPrenomina(int prenominaID)
        {
            return interfazDAO.DataExcelPrenomina(prenominaID);
        }
        public List<tblC_Nom_Prenomina> GetPrenominasAutorizadas(int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.GetPrenominasAutorizadas(periodo, tipoNomina, anio);
        }
        #endregion

        #region Reporte Nomina Anual

        public List<reporteAnualDTO> GetReportes(string bottom, string top)
        {
            return interfazDAO.GetReportes(bottom, top);
        }

        public MemoryStream crearReporte(List<string[]> lstReporte, string tp, string btm)
        {
            return interfazDAO.crearReporte(lstReporte,tp,btm);
        }

        #endregion

        #region Acomulado Mensual

        public Dictionary<string, object> GetNominaDetalle(string botomDate, string topDate)
        {
            return interfazDAO.GetNominaDetalle(botomDate, topDate);
        }

        public MemoryStream crearReporteMensual(List<ReporteAcomuladoMensualDTO> lstReporte, ReporteAcomuladoMensualDTO listaRegHead, string numeroEmpleados, string periodoDate)
        {
            return interfazDAO.crearReporteMensual(lstReporte, listaRegHead, numeroEmpleados, periodoDate);
        }
        #endregion

        #region Reporte Rango por Centro de costo

        public List<ReporteRangoCCDTO> GetReportesCC(string bottom, string top)
        {
            return interfazDAO.GetReportesCC(bottom, top);
        }

        public MemoryStream crearReporteCC(List<string[]> lstReporte, string tp, string btm)
        {
            return interfazDAO.crearReporteCC(lstReporte, tp, btm);
        }

        public int GetNumEmpleados(string bottom, string top)
        {
            return interfazDAO.GetNumEmpleados(bottom, top);
        }

        #endregion

        #region Reporte rango por Centro de costo y Cuenta cta

        public Dictionary<string, object> GetReporteCtroCta(string bottom, string top)
        {
            return interfazDAO.GetReporteCtroCta(bottom,top);
        }

        public MemoryStream crearReporteCentroCuenta(List<string[]> lstReporte, string tp, string btm)
        {
            return interfazDAO.crearReporteCentroCuenta(lstReporte, tp, btm);
        }

        #endregion

        #region Reporte rango por Empleado y centr de costo

        public Dictionary<string,object> GetReporteEmpleadoCC(string bottom, string top, string cc, string empleado, int tipoRaya)
        {
            return interfazDAO.GetReporteEmpleadoCC(bottom,top,cc,empleado, tipoRaya);
        }

        public MemoryStream crearReporteEmpleadoCC(Dictionary<string, object> lstReporte, string tp, string btm)
        {
            return interfazDAO.crearReporteEmpleadoCC(lstReporte, tp, btm);
        }

        public Dictionary<string,object> GetCentroCostos()
        {
            return interfazDAO.GetCentroCostos();
        }

        public Dictionary<string, object> GetListaEmpleados()
        {
            return interfazDAO.GetListaEmpleados();
        }

        #endregion

        #region Reporte Concentrado
        public Dictionary<string,object> GetReporteConcentrado(DateTime? fechaInicial, DateTime? fechaFinal, int? tipoRaya, int? tipoNomina)
        {
            return interfazDAO.GetReporteConcentrado(fechaInicial, fechaFinal, tipoRaya, tipoNomina);
        }
        #endregion

        public SolicitudChequeReporteDTO GetSolicitudChequeReporte(int year, int periodo, int tipoNomina, int banco)
        {
            return interfazDAO.GetSolicitudChequeReporte(year, periodo, tipoNomina, banco);
        }
        public CedulaCostosReporteDTO GetCedulaCostos(int year, int periodo, int tipoNomina, int banco)
        {
            return interfazDAO.GetCedulaCostos(year, periodo, tipoNomina, banco);
        }
        public polizaOcsiReporteDTO GetPolizaOCSI(int year, int periodo, int tipoNomina, int banco)
        {
            return interfazDAO.GetPolizaOCSI(year, periodo, tipoNomina, banco);
        }

        #region Layout Incidencias
        public Dictionary<string, object> CargarLayoutIncidencias(int anio, int periodo, int tipo_nomina, string estatus)
        {
            return interfazDAO.CargarLayoutIncidencias(anio, periodo, tipo_nomina, estatus);
        }

        public Tuple<MemoryStream, string, List<tblRH_BN_Incidencia>> DescargarExcelLayoutIncidencias(int tipo)
        {
            return interfazDAO.DescargarExcelLayoutIncidencias(tipo);
        }
        public Tuple<MemoryStream, string> DescargarExcelReporteIncidencias(int anio, int tipo_nomina, int periodo, bool autorizado)
        {
            return interfazDAO.DescargarExcelReporteIncidencias(anio, tipo_nomina, periodo, autorizado);
        }
        public bool ActualizarIncidencias(List<tblRH_BN_Incidencia> incidencias)
        {
            return interfazDAO.ActualizarIncidencias(incidencias);
        }
        #endregion

        public List<EmpleadoPendienteLiberacionDTO> getEmpleadosPendientesLiberacion()
        {
            return interfazDAO.getEmpleadosPendientesLiberacion();
        }
        public void guardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados)
        {
            interfazDAO.guardarBajas(empleados);
        }

        #region Descuentos
        public Dictionary<string, object> GetDescuentos()
        {
            return interfazDAO.GetDescuentos();
        }

        public Dictionary<string, object> GuardarNuevoDescuento(tblC_Nom_PreNomina_Descuento descuento)
        {
            return interfazDAO.GuardarNuevoDescuento(descuento);
        }

        public Dictionary<string, object> EditarDescuento(tblC_Nom_PreNomina_Descuento descuento)
        {
            return interfazDAO.EditarDescuento(descuento);
        }

        public Dictionary<string, object> EliminarDescuento(tblC_Nom_PreNomina_Descuento descuento)
        {
            return interfazDAO.EliminarDescuento(descuento);
        }

        public Dictionary<string, object> CargarExcelDescuentos(HttpFileCollectionBase archivos)
        {
            return interfazDAO.CargarExcelDescuentos(archivos);
        }
        #endregion

        public Dictionary<string, object> GetEstatusPeriodo(int tipo_nomina, int anio)
        {
            return interfazDAO.GetEstatusPeriodo(tipo_nomina, anio);
		}
        public string GetCCNominas(string cc)
        {
            return interfazDAO.GetCCNominas(cc);
        }

        public Dictionary<string, object> FillCboBancos(tipoNominaPropuestaEnum tipoNomina, int periodo)
        {
            return interfazDAO.FillCboBancos(tipoNomina, periodo);
        }
        public List<tblC_Nom_CatSolicitudCheque> GetBancos(tipoNominaPropuestaEnum tipoNomina)
        {
            return interfazDAO.GetBancos(tipoNomina);
        }

        #region SUA
        public Dictionary<string, object> GetTipoDocumento()
        {
            return interfazDAO.GetTipoDocumento();
        }
        public Dictionary<string, object> GetSUACargado(int periodo, int tipoPeriodo, int year, int tipoRaya)
        {
            return interfazDAO.GetSUACargado(periodo, tipoPeriodo, year, tipoRaya);
        }
        #endregion

        #region PERU
        public Dictionary<string, object> CargarPrenominaPeru(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.CargarPrenominaPeru(CC, periodo, tipoNomina, anio);
        }

        public Dictionary<string, object> GuardarPrenominaPeru(int prenominaID, List<tblC_Nom_PreNominaPeru_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes, string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            return interfazDAO.GuardarPrenominaPeru(prenominaID, detalles, autorizantes, CC, periodo, tipoNomina, anio);
        }

        public Dictionary<string, object> ValidarPrenominaPeru(int prenominaID, List<tblC_Nom_PreNominaPeru_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes)
        {
            return interfazDAO.ValidarPrenominaPeru(prenominaID, detalles, autorizantes);
        }

        public List<tblC_Nom_PreNominaPeru_Det> GetPrenominDetallesPeruByID(int prenominaID)
        {
            return interfazDAO.GetPrenominDetallesPeruByID(prenominaID);
        }

        #region CATALOGO AFP
        public Dictionary<string, object> GetRegistrosPeruAFP()
        {
            return interfazDAO.GetRegistrosPeruAFP();
        }

        public Dictionary<string, object> CrearEditarRegistroPeruAFP(PeruAFPDTO objParamsDTO)
        {
            return interfazDAO.CrearEditarRegistroPeruAFP(objParamsDTO);
        }

        public Dictionary<string, object> GetRegistroActualizarPeruAFP(PeruAFPDTO objParamsDTO)
        {
            return interfazDAO.GetRegistroActualizarPeruAFP(objParamsDTO);
        }

        public Dictionary<string, object> EliminarRegistroPeruAFP(PeruAFPDTO objParamsDTO)
        {
            return interfazDAO.EliminarRegistroPeruAFP(objParamsDTO);
        }

        public Dictionary<string, object> FillCboCatAFP()
        {
            return interfazDAO.FillCboCatAFP();
        }

        public Dictionary<string, object> SetAnioMes()
        {
            return interfazDAO.SetAnioMes();
        }
        #endregion

        #endregion
    }
}
