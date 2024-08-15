using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Entity.Principal.Multiempresa;

namespace Core.Service.Maquinaria.Overhaul
{
    public class AdministracionComponentesServices : IAdministracionComponentesDAO
    {
        #region Atributos
        private IAdministracionComponentesDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IAdministracionComponentesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public AdministracionComponentesServices(IAdministracionComponentesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public void Guardar(tblM_trackComponentes obj)
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblP_CC> getListaCCByID(List<int> ccID)
        {
            return interfazDAO.getListaCCByID(ccID);
        }
        public List<ListaComponentesMaquina> getMaquinas(int grupo, int modelo, string economicoBusqueda, string descripcionComponente, string obra, string noComponente = "")
        {
            return interfazDAO.getMaquinas(grupo, modelo, economicoBusqueda, descripcionComponente, obra, noComponente);
        }
        public List<ListaComponentesMaquina> getMaquinasLocaciones(int estatus)
        {
            return interfazDAO.getMaquinasLocaciones(estatus);
        }

        public string getDescripcionCC(string centro_costos)
        {
            return interfazDAO.getDescripcionCC(centro_costos);
        }

        public List<tblM_CatModeloEquipo> FillCboModeloEquipoGrupo(int idGrupo)
        {
            return interfazDAO.FillCboModeloEquipoGrupo(idGrupo);
        }

        public List<tblM_trackComponentes> getListaComponentes(int idMaquina)
        {
            return interfazDAO.getListaComponentes(idMaquina);
        }
        public List<tblM_trackComponentes> FillModalComponentes(int idMaquina, string componente)
        {
            return interfazDAO.FillModalComponentes(idMaquina, componente);
        }
        public string getLocacionByID(int idMaquina, bool tipoLocacion)
        {
            return interfazDAO.getLocacionByID(idMaquina, tipoLocacion);
        }

        public List<tblM_trackComponentes> FillModalComponentesHistorial(int idComponente)
        {
            return interfazDAO.FillModalComponentesHistorial(idComponente);
        }
        public List<ComboDTO> FillCboGrupoMaquinaria(int idTipo)
        {
            return interfazDAO.FillCboGrupoMaquinaria(idTipo);
        }
        public List<ComboDTO> FillCboObraMaquina()
        {
            return interfazDAO.FillCboObraMaquina();
        }
        public int getVidasAcumuladas(int idComponente)
        {
            return interfazDAO.getVidasAcumuladas(idComponente);
        }
        public int getVidasAcumuladasByFecha(int idComponente, DateTime fecha)
        {
            return interfazDAO.getVidasAcumuladasByFecha(idComponente, fecha);
        }
        public List<tblM_CatSubConjunto> getSubConjuntos(string term)
        {
            return interfazDAO.getSubConjuntos(term);
        }
        public List<tblM_CatComponente> getNoComponente(string term)
        {
            return interfazDAO.getNoComponente(term);
        }
        public List<tblM_CatComponente> getNoComponenteReporte(string term, int modeloID, int subconjuntoID)
        {
            return interfazDAO.getNoComponenteReporte(term, modeloID, subconjuntoID);
        }
        public List<tblM_CatMaquina> getEconomico(string term)
        {
            return interfazDAO.getEconomico(term);
        }
        public List<tblM_CatLocacionesComponentes> FillCboLocacion(int estatus)
        {
            return interfazDAO.FillCboLocacion(estatus);
        }
        public List<tblM_CatLocacionesComponentes> FillCboLocacionByListaTipo(List<int> tipoLocaciones)
        {
            return interfazDAO.FillCboLocacionByListaTipo(tipoLocaciones);
        }
        public List<tblM_trackComponentes> getComponentesAlmacenInactivos(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId)
        {
            return interfazDAO.getComponentesAlmacenInactivos(noComponente, idLocacion, descripcionComponente, estatus, grupoId, modeloId);
        }


        #region REPORTE ALMACEN
        public List<tblM_trackComponentes> CargarReporteAlmacen(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId)
        {
            return interfazDAO.CargarReporteAlmacen(noComponente, idLocacion, descripcionComponente, estatus, grupoId, modeloId);
        }

        #endregion



        public string getLocacionByID(int id)
        {
            return interfazDAO.getLocacionByID(id);
        }
        public void GuardarFechasCRC(int idReporte, DateTime fecha, int estatus, bool intercambio, string datosExtra, int usuario)
        {
            interfazDAO.GuardarFechasCRC(idReporte, fecha, estatus, intercambio, datosExtra, usuario);
        }
        public ComboDTO getMaquinaModalCRC(int idComponente)
        {
            return interfazDAO.getMaquinaModalCRC(idComponente);
        }
        public tblM_trackComponentes getTrackingByID(int idTrack)
        {
            return interfazDAO.getTrackingByID(idTrack);
        }
        public void cambioAlmacen(List<int> componentes, int idAlmacen, int estatus)
        {
            interfazDAO.cambioAlmacen(componentes, idAlmacen, estatus);
        }
        public string getAlmacenLocacionByID(int id)
        {
            return interfazDAO.getAlmacenLocacionByID(id);
        }
        public tblM_CatLocacionesComponentes getLocacion(int id)
        {
            return interfazDAO.getLocacion(id);
        }
        public List<tblM_CatMaquina> getMaquinasByCC()
        {
            return interfazDAO.getMaquinasByCC();
        }
        public List<ComboDTO> getEmpleadosChoferAlmacen(string term)
        {
            return interfazDAO.getEmpleadosChoferAlmacen(term);
        }
        public List<ComboDTO> getCompradores(string term)
        {
            return interfazDAO.getCompradores(term);
        }
        public string getUltimoEconomico(int idComponente)
        {
            return interfazDAO.getUltimoEconomico(idComponente);
        }
        public tblM_CatMaquina getUltimaMaquina(int idComponente)
        {
            return interfazDAO.getUltimaMaquina(idComponente);
        }
        public DateTime getEntradaAlmacen(int idComponente)
        {
            return interfazDAO.getEntradaAlmacen(idComponente);
        }
        public tblM_trackComponentes getTrackingByComponente(int idComponente)
        {
            return interfazDAO.getTrackingByComponente(idComponente);
        }
        public string getObraLocacion(int idLocacion)
        {
            return interfazDAO.getObraLocacion(idLocacion);
        }
        public List<RemocionesVidaUtilDTO> CargarRemocionesVidaUtil(int modelo, int grupo, DateTime fechaInicio, DateTime fechaFin)
        {
            return interfazDAO.CargarRemocionesVidaUtil(modelo, grupo, fechaInicio, fechaFin);
        }
        public List<rptInventarioComponenteDTO> CargarReporteInventario(int grupo, int modelo, int conjunto, int subconjunto, string obra)
        {
            return interfazDAO.CargarReporteInventario(grupo, modelo, conjunto, subconjunto, obra);
        }

        public List<rptListadoMaestroDTO> CargarReporteMaestro(int idCalendario)
        {
            return interfazDAO.CargarReporteMaestro(idCalendario);
        }



       /* public List<RptAlmacenComponentesDTO> CargarReporteAlmacen(DateTime fecha, DateTime entrada, DateTime fechaEntradaFactura, int dias, string subconjunto, int modeloId, string locacion, int horasCicloActual)
        {
            return interfazDAO.CargarReporteAlmacen(fecha,  entrada,  fechaEntradaFactura,  dias,  subconjunto,  modeloId,  locacion, horasCicloActual);
        }*/



        public List<rptValorAlmacenDTO> CargarReporteValorAlmacen(int anioInicial, int anioFinal)
        {
            return interfazDAO.CargarReporteValorAlmacen(anioInicial, anioFinal);
        }
        public List<ComboDTO> FillCboAniosValorAlmacen()
        {
            return interfazDAO.FillCboAniosValorAlmacen();
        }
        public List<string> getLocaciones()
        {
            return interfazDAO.getLocaciones();
        }
     
        public List<ComboDTO> CargarDatosDetalleMaestro(int idPlaneacionOH)
        {
            return interfazDAO.CargarDatosDetalleMaestro(idPlaneacionOH);
        }
        public List<ComboDTO> FillCboCalendarioReporteMaestro()
        {
            return interfazDAO.FillCboCalendarioReporteMaestro();
        }
        public List<ComboDTO> CargarDatosDetalleMaestroPlaneacion(string indexCal)
        {
            return interfazDAO.CargarDatosDetalleMaestroPlaneacion(indexCal);
        }
        public List<tblP_CC> FillCboObraMaquinaID()
        {
            return interfazDAO.FillCboObraMaquinaID();
        }
        public Dictionary<string, object> FillCboObraMaquinaIDComboDTO()
        {
            return interfazDAO.FillCboObraMaquinaIDComboDTO();
        }
        public tblM_CapPlaneacionOverhaul CargarEventoOverhaul(int idPlaneacionOH)
        {
            return interfazDAO.CargarEventoOverhaul(idPlaneacionOH);
        }
        public List<tblM_trackComponentes> GetReporteConjunto(bool enProceso)
        {
            return interfazDAO.GetReporteConjunto(enProceso);
        }
        public tblP_CC getCCByEconomico(string economico)
        {
            return interfazDAO.getCCByEconomico(economico);
        }
        public tblM_trackComponentes GetSiguenteTracking(int id, int tipo)
        {
            return interfazDAO.GetSiguenteTracking(id, tipo);
        }
        public tblM_trackComponentes getUltimoTrackCRC(int componenteID)
        {
            return interfazDAO.getUltimoTrackCRC(componenteID);
        }
        public tblM_trackComponentes getTrackAnterior(int componenteID)
        {
            return interfazDAO.getTrackAnterior(componenteID);
        }
        public tblM_trackComponentes GetTrackUltimaInstalacion(int componenteID, DateTime fecha)
        {
            return interfazDAO.GetTrackUltimaInstalacion(componenteID, fecha);
        }
        public List<ComboDTO> getFacturaTrackAnterior(List<int> componentesID)
        {
            return interfazDAO.getFacturaTrackAnterior(componentesID);
        }
        public List<ComboDTO> getLocacionTrackAnterior(List<int> componentesID)
        {
            return interfazDAO.getLocacionTrackAnterior(componentesID);
        }
        public List<tblM_trackComponentes> CargarTablaHistorial(string componente, int subconjunto, string locacion, DateTime fechaInicio, DateTime fechaFin, int grupo, int modelo)
        {
            return interfazDAO.CargarTablaHistorial(componente, subconjunto, locacion, fechaInicio, fechaFin, grupo, modelo);
        }
        public RemocionDTO cargarDatosRemocionHistorial(int idComponente, int trackID)
        {
            return interfazDAO.cargarDatosRemocionHistorial(idComponente, trackID);
        }
        public int GetVidasComponenteTracking(int trackID)
        {
            return interfazDAO.GetVidasComponenteTracking(trackID);
        }
        public int GetReporteDesechoID(int componenteID)
        {
            return interfazDAO.GetReporteDesechoID(componenteID);
        }
        public string getDescripcionCCByCC(string centro_costos)
        {
            return interfazDAO.getDescripcionCCByCC(centro_costos);
        }
        public List<ComboDTO> FillCboLocacionYObra()
        {
            return interfazDAO.FillCboLocacionYObra();
        }
        public tblM_trackComponentes getTrackingByIDComp(int idComponente)
        {
            return interfazDAO.getTrackingByIDComp(idComponente);
        }
        public bool GuardarEntradaAlmacen(int trackingID, DateTime fecha)
        {
            return interfazDAO.GuardarEntradaAlmacen(trackingID, fecha);
        }
        public bool ReactivarComponentesInactivos(int componenteID, int locacionID, DateTime fecha)
        {
            return interfazDAO.ReactivarComponentesInactivos(componenteID, locacionID, fecha);
        }
        public DateTime getFechaFacturaEnkontrol(string factura)
        {
            return interfazDAO.getFechaFacturaEnkontrol(factura);
        }
        public List<FechaFacturaEnkontrolDTO> getFechaFacturaEnkontrol(List<string> facturas)
        {
            return interfazDAO.getFechaFacturaEnkontrol(facturas);
        }
        #region Reporte Component List
        public List<ComboDTO> FillCboLocacionesComponentList(List<int> modelosID)
        {
            return interfazDAO.FillCboLocacionesComponentList(modelosID);
        }
        public List<ComboDTO> FillCboAlmacenesInventario()
        {
            return interfazDAO.FillCboAlmacenesInventario();
        }
        public List<ComboDTO> FillCboComponentes()
        {
            return interfazDAO.FillCboComponentes();
        }
        public List<ComboDTO> FillCboConjuntos()
        {
            return interfazDAO.FillCboConjuntos();
        }
        public List<ComboDTO> FillCboSubconjuntos(int conjunto)
        {
            return interfazDAO.FillCboSubconjuntos(conjunto);
        }
        public List<TrackingRitmoDTO> CargarComponentList(int locacion, string noComponente, int conjunto, int subconjunto, List<int> modelo, string obraAC)
        {
            return interfazDAO.CargarComponentList(locacion, noComponente, conjunto, subconjunto, modelo, obraAC);
        }
        #endregion
        #region Reporte Inventario
        public List<tblM_trackComponentes> CargarInventario(int locacion, string noComponente, int conjunto, int subconjunto, List<int> modelo)
        {
            return interfazDAO.CargarInventario(locacion, noComponente, conjunto, subconjunto, modelo);
        }
        #endregion
        public int getNumeroTrackings(int noComponente)
        {
            return interfazDAO.getNumeroTrackings(noComponente);
        }
        #region Reporte componentes reparacion
        public List<tblM_trackComponentes> CargarCompReparacion(int locacion, int grupo, int modelo, int subconjunto)
        {

            return interfazDAO.CargarCompReparacion(locacion, grupo, modelo, subconjunto);
        }
        #endregion

        public string descripcionModelo(int modeloId)
        {
            return interfazDAO.descripcionModelo(modeloId);
        }

    }
}



