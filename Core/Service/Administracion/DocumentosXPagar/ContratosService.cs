using Core.DAO.Administracion.DocumentosXPagar;
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

namespace Core.Service.Administracion.DocumentosXPagar
{
    public class ContratosService : IContratosDAO
    {
        #region Atributos
        private IContratosDAO m_interfazDAO;
        #endregion Atributos
        #region Propiedades
        private IContratosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades
        #region Constructores
        public ContratosService(IContratosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        #region Cedula
        public Dictionary<string, object> GetCedula(DateTime fechaCorte)
        {
            return interfazDAO.GetCedula(fechaCorte);
        }
        #endregion

        public Respuesta ObtenerContratos(FiltroContratosDTO filtro)
        {
            return interfazDAO.ObtenerContratos(filtro);
        }

        /* public Respuesta GuardarContrato(tblAF_DxP_Contrato contrato)
         {
             return interfazDAO.GuardarContrato(contrato);
         }*/

        public Respuesta GuardarDeudas(List<tblAF_DxP_Deuda> objDeudas, tblC_sc_polizas poliza, List<tblC_sc_movpol> movPolList)
        {
            return interfazDAO.GuardarDeudas(objDeudas, poliza, movPolList);
        }

        public Respuesta AgregarMaquina(AgregarMaquinaDTO maquina)
        {
            return interfazDAO.AgregarMaquina(maquina);
        }

        public Respuesta ObtenerMaquinas(int idContrato)
        {
            return interfazDAO.ObtenerMaquinas(idContrato);
        }

        public Respuesta ObtenerDesgloseGeneral(int idContrato)
        {
            return interfazDAO.ObtenerDesgloseGeneral(idContrato);
        }

        public Respuesta ObtenerDesglosePorMaquina(int idContratoMaquina)
        {
            return interfazDAO.ObtenerDesglosePorMaquina(idContratoMaquina);
        }

        public Dictionary<string, object> ObtenerMaquinas()
        {
            return interfazDAO.ObtenerMaquinas();
        }
        public Dictionary<string, object> ObtenerInstituciones()
        {
            return interfazDAO.ObtenerInstituciones();
        }
        public Dictionary<string, object> ObtenerContratoByID(int id)
        {
            return interfazDAO.ObtenerContratoByID(id);
        }
        public Dictionary<string, object> ObtenerPagos(int contratoID, int parcialidad)
        {
            return interfazDAO.ObtenerPagos(contratoID, parcialidad);
        }

        public Dictionary<string, object> SaveOrupdatepagos(tblAF_DxP_Pago dxpPago, List<tblAF_DxP_PagoMaquina> dxpPagoMaquina, List<tblAF_DxP_ContratoMaquinaDetalle> dxpContratoMaquinaDetalle)
        {
            return interfazDAO.SaveOrupdatepagos(dxpPago, dxpPagoMaquina, dxpContratoMaquinaDetalle);
        }
        public Dictionary<string, object> GuardarFechaNuevoPeriodo(int contratoID, DateTime nuevaFecha, int parcialidad)
        {
            return interfazDAO.GuardarFechaNuevoPeriodo(contratoID, nuevaFecha, parcialidad);
        }
        public Dictionary<string, object> ObtenerContratosNotificaciones()
        {
            return interfazDAO.ObtenerContratosNotificaciones();
        }

        public Dictionary<string, object> CargarPropuesta(DateTime inicio, DateTime final, int estatus, int institucion)
        {
            return interfazDAO.CargarPropuesta(inicio, final, estatus, institucion);
        }
        public Dictionary<string, object> GuardarProgramacion(List<tblAF_DxP_ProgramacionPagos> obj)
        {
            return interfazDAO.GuardarProgramacion(obj);
        }
        public Respuesta GuardarContrato(tblAF_DxP_Contrato contrato, List<AgregarMaquinaDTO> listaEconomicos, bool tasaFija)
        {
            return interfazDAO.GuardarContrato(contrato, listaEconomicos, tasaFija);
        }
        public Dictionary<string, object> GetTipoCambioFecha(DateTime fecha)
        {
            return interfazDAO.GetTipoCambioFecha(fecha);
        }

        public Dictionary<string, object> loadPropuestas(int? idInstitucion, int empresa, int moneda)
        {
            return interfazDAO.loadPropuestas(idInstitucion, empresa, moneda);
        }
        public ReporteDTO DesgloseGeneral(int idContrato)
        {
            return interfazDAO.DesgloseGeneral(idContrato);
        }

        public Dictionary<string, object> CargarContrato(int contratoId, int parcialidad, DateTime fechaPol)
        {
            return interfazDAO.CargarContrato(contratoId, parcialidad, fechaPol);
        }
        public Dictionary<string, object> guardarPoliza(tblC_sc_polizas poliza, List<tblC_sc_movpol> movpol, List<listaContrato> contrato, decimal tipoCambio)
        {
            return interfazDAO.guardarPoliza(poliza, movpol, contrato, tipoCambio);
        }

        public Dictionary<string, object> GetProveedores()
        {
            return interfazDAO.GetProveedores();
        }

        public Dictionary<string, object> guardarInstitucion(string descripcion)
        {
            return interfazDAO.guardarInstitucion(descripcion);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstContratos(BusqProyeccionCierreDTO busq)
        {
            return interfazDAO.getLstContratos(busq);
        }

        public List<PropuestaArrendadoraDTO> LoadReportePropuestaArrendadora(DateTime pfechaInicio, DateTime pfechaFin)
        {
            return interfazDAO.LoadReportePropuestaArrendadora(pfechaInicio, pfechaFin);
        }

        public Dictionary<string, object> LoadProgramacionPagos(DateTime pInicio, DateTime pFinal, int pEstatus, List<int> institucion, int empresa, int moneda)
        {
            return interfazDAO.LoadProgramacionPagos(pInicio, pFinal, pEstatus, institucion, empresa, moneda);
        }

        public List<autoCompleteCta> autoComplCatCtas(string term)
        {
            return interfazDAO.autoComplCatCtas(term);
        }
        public List<autoCompleteCta> autoCompleteCtasIA(string term)
        {
            return interfazDAO.autoCompleteCtasIA(term);
        }
        public Dictionary<string, object> CargarDetallePago(int parcialidad, int contratoId)
        {
            return interfazDAO.CargarDetallePago(parcialidad, contratoId);
        }
        public Dictionary<string, object> getRptAdeudosGeneral(List<int> tipoMoneda, List<int> anio, List<int> institucion, bool tipoArrendamiento)
        {
            return interfazDAO.getRptAdeudosGeneral(tipoMoneda, anio, institucion, tipoArrendamiento);
        }
        public Dictionary<string, object> getRptAdeudosDetalle(int tipoMoneda, List<int> instituciones, DateTime fechaFin, int tipo, List<bool> tipoArre)
        {
            return interfazDAO.getRptAdeudosDetalle(tipoMoneda, instituciones, fechaFin, tipo, tipoArre);
        }

        public ComboDTO getArchivoDownLoad(int contratoID)
        {
            return interfazDAO.getArchivoDownLoad(contratoID);
        }
        public Dictionary<string, object> getPolizaByFecha(DateTime fecha)
        {
            return interfazDAO.getPolizaByFecha(fecha);
        }
        public Dictionary<string, object> comboCbosData()
        {
            return interfazDAO.comboCbosData();
        }

        public Dictionary<string, object> LoadContratosProgramados(DateTime pInicio, DateTime pFinal, string cc)
        {
            return interfazDAO.LoadContratosProgramados(pInicio, pFinal, cc);
        }
        public Dictionary<string, object> LoadContratosProgramadosCplan(DateTime pInicio, DateTime pFinal, string cc)
        {
            return interfazDAO.LoadContratosProgramadosCplan(pInicio, pFinal, cc);
        }
        public Dictionary<string, object> ActualizarContratos(List<tblAF_DxP_ProgramacionPagos> arrayProgramacionID)
        {
            return interfazDAO.ActualizarContratos(arrayProgramacionID);
        }

        public List<ctaDTO> GetCtaList()
        {
            return interfazDAO.GetCtaList();
        }

        public Dictionary<string, object> LoadCtas()
        {
            return interfazDAO.LoadCtas();
        }

        public Dictionary<string, object> TerminarContrato(int contratoID)
        {
            return interfazDAO.TerminarContrato(contratoID);
        }

        public Dictionary<string, object> UpdateContratosDet(int contratoID)
        {
            return interfazDAO.UpdateContratosDet(contratoID);
        }

        public Dictionary<string, object> UpdateContratoArchivo(int contratoID, string fileContrato)
        {
            return interfazDAO.UpdateContratoArchivo(contratoID, fileContrato);
        }

        public dtLoadCtaServerDTO LoadCtasServerSide()
        {
            return interfazDAO.LoadCtasServerSide();
        }

        public Dictionary<string, object> GetInfoLiquidar(bool liquidar, int contratoId, int parcialidad)
        {
            return interfazDAO.GetInfoLiquidar(liquidar, contratoId, parcialidad);
        }

        #region REPORTE SALDO PENDIENTE POR PROYECTO
        public Dictionary<string, object> ObtenerCboCC(List<int> lstDivisionID)
        {
            return interfazDAO.ObtenerCboCC(lstDivisionID);
        }

        public Dictionary<string, object> ObtenerCboDivisiones()
        {
            return interfazDAO.ObtenerCboDivisiones();
        }

        //public List<adeudosDTO> ObtenerListadoDivisiones(adeudosDTO objDivision)
        //{
        //    return interfazDAO.ObtenerListadoDivisiones(objDivision);
        //}

        public Dictionary<string, object> ObtenerListadoDivisiones(adeudosDTO objDivision)
        {
            return interfazDAO.ObtenerListadoDivisiones(objDivision);
        }
        #endregion

        #region CATAGOLO DIVISIONES
        public List<CatDivisionesDTO> GetDivisiones()
        {
            return interfazDAO.GetDivisiones();
        }

        public CatDivisionesDTO GuardarDivisiones(tblAF_DxP_Divisiones parametros)
        {
            return interfazDAO.GuardarDivisiones(parametros);

        }
        public bool EditarDivisiones(tblAF_DxP_Divisiones parametros)
        {
            return interfazDAO.EditarDivisiones(parametros);

        }
        public bool EliminarDivisiones(int id)
        {
            return interfazDAO.EliminarDivisiones(id);
        }
        #endregion

        #region DIVISIONES_PROYECYOS
        public List<Divisiones_ProyectosDTO> GetDivisiones_Proyectos()
        {
            return interfazDAO.GetDivisiones_Proyectos();
        }

        public List<Divisiones_ProyectosDTO> GetDivisiones_ProyectosFitro(tblAF_DxP_Divisiones_Proyecto objFiltro)
        {
            return interfazDAO.GetDivisiones_ProyectosFitro(objFiltro);
        }

        public List<ComboDTO> GetCC()
        {
            return interfazDAO.GetCC();
        }
    
        public List<ComboDTO> GetCmbDivision()
        {
            return interfazDAO.GetCmbDivision();
        }

        public Divisiones_ProyectosDTO GuardarDivisiones_Proyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        {
            return interfazDAO.GuardarDivisiones_Proyectos(parametros);
        }

        public bool EliminarDivisionesProyectos(int id)
        {
            return interfazDAO.EliminarDivisionesProyectos(id);
        }

        public bool EditarDivisionesProyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        {
            return interfazDAO.EditarDivisionesProyectos(parametros);

        }
	    #endregion

        #region PQ
        public Dictionary<string, object> GuardarPQ(tblAF_DxP_PQ pq, HttpPostedFileBase archivo)
        {
            return this.interfazDAO.GuardarPQ(pq, archivo);
        }

        public Dictionary<string, object> GetMonedas()
        {
            return this.interfazDAO.GetMonedas();
        }

        public Dictionary<string, object> GetPQs(bool estatus, string fechaCorte)
        {
            return this.interfazDAO.GetPQs(estatus, fechaCorte);
        }

        public Dictionary<string, object> GetPQ(int id)
        {
            return this.interfazDAO.GetPQ(id);
        }

        public Dictionary<string, object> ObtenerInstitucionesPQ()
        {
            return this.interfazDAO.ObtenerInstitucionesPQ();
        }

        public Dictionary<string, object> GetPQLiquidar(int id)
        {
            return this.interfazDAO.GetPQLiquidar(id);
        }

        public Dictionary<string, object> GetPQCambiarCC(int id)
        {
            return this.interfazDAO.GetPQCambiarCC(id);
        }

        public Dictionary<string, object> GetPQRenovar(int id)
        {
            return this.interfazDAO.GetPQRenovar(id);
        }

        public Dictionary<string, object> GetPQAbono(int id)
        {
            return this.interfazDAO.GetPQAbono(id);
        }

        public Dictionary<string, object> Liquidar(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            return this.interfazDAO.Liquidar(idPq, fechaMovimiento, infoPol);
        }

        public Dictionary<string, object> CambiarCC(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            return this.interfazDAO.CambiarCC(idPq, fechaMovimiento, infoPol);
        }

        public Dictionary<string, object> RenovarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol, DateTime fechaFirma, DateTime fechaVencimiento, decimal interes, HttpPostedFileBase archivo)
        {
            return this.interfazDAO.RenovarPQ(idPq, fechaMovimiento, infoPol, fechaFirma, fechaVencimiento, interes, archivo);
        }

        public Dictionary<string, object> AbonarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            return this.interfazDAO.AbonarPQ(idPq, fechaMovimiento, infoPol);
        }

        public Dictionary<string, object> UrlArchivoPQ(int idPq)
        {
            return this.interfazDAO.UrlArchivoPQ(idPq);
        }
        #endregion

        #region POLIZA REVALUACION
        public Dictionary<string, object> GetInfoRevaluacion(DateTime fecha)
        {
            return interfazDAO.GetInfoRevaluacion(fecha);
        }

        public Dictionary<string, object> RegistrarPolizaRevaluacion(PolizaMovPolEkDTO poliza)
        {
            return interfazDAO.RegistrarPolizaRevaluacion(poliza);
        }
        #endregion

        public Dictionary<string, object> CargarReporteInteresesPagados(DateTime fecha)
        {
            return this.interfazDAO.CargarReporteInteresesPagados(fecha);
        }

        public Dictionary<string, object> DescargarExcelInteresesPagados(DateTime fecha)
        {
            return this.interfazDAO.DescargarExcelInteresesPagados(fecha);
        }
    }
}