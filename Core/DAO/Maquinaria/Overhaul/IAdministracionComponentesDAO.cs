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

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IAdministracionComponentesDAO
    {
        void Guardar(tblM_trackComponentes obj);
        List<tblP_CC> getListaCCByID(List<int> ccID);
        List<ListaComponentesMaquina> getMaquinas(int grupo, int modelo, string economicoBusqueda, string descripcionComponente, string obra, string noComponente);
        List<ListaComponentesMaquina> getMaquinasLocaciones(int estatus);
        string getDescripcionCC(string centro_costos);
        List<tblM_CatModeloEquipo> FillCboModeloEquipoGrupo(int idGrupo);
        List<tblM_trackComponentes> getListaComponentes(int idMaquina);
        List<tblM_trackComponentes> FillModalComponentes(int idMaquina, string componente);
        string getLocacionByID(int idMaquina, bool tipoLocacion);
        List<tblM_trackComponentes> FillModalComponentesHistorial(int idComponente);
        List<ComboDTO> FillCboGrupoMaquinaria(int idTipo);
        List<ComboDTO> FillCboObraMaquina();
        int getVidasAcumuladas(int idComponente);
        int getVidasAcumuladasByFecha(int idComponente, DateTime fecha);
        List<tblM_CatSubConjunto> getSubConjuntos(string term);
        List<tblM_CatComponente> getNoComponente(string term);
        List<tblM_CatComponente> getNoComponenteReporte(string term, int modeloID, int subconjuntoID);
        List<tblM_CatMaquina> getEconomico(string term);
        List<tblM_CatLocacionesComponentes> FillCboLocacion(int estatus);
        List<tblM_CatLocacionesComponentes> FillCboLocacionByListaTipo(List<int> tipoLocaciones);
        List<tblM_trackComponentes> getComponentesAlmacenInactivos(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId);

        string descripcionModelo(int modeloId);
        
        string getLocacionByID(int id);
        void GuardarFechasCRC(int idReporte, DateTime fecha, int estatus, bool intercambio, string datosExtra, int usuario);
        ComboDTO getMaquinaModalCRC(int idComponente);
        tblM_trackComponentes getTrackingByID(int idTrack);
        void cambioAlmacen(List<int> componentes, int idAlmacen, int estatus);
        string getAlmacenLocacionByID(int id);
        tblM_CatLocacionesComponentes getLocacion(int id);
        List<tblM_CatMaquina> getMaquinasByCC();
        List<ComboDTO> getEmpleadosChoferAlmacen(string term);
        List<ComboDTO> getCompradores(string term);
        string getUltimoEconomico(int idComponente);
        tblM_CatMaquina getUltimaMaquina(int idComponente);
        DateTime getEntradaAlmacen(int idComponente);
        tblM_trackComponentes getTrackingByComponente(int idComponente);
        string getObraLocacion(int idLocacion);
        List<RemocionesVidaUtilDTO> CargarRemocionesVidaUtil(int modelo, int grupo, DateTime fechaInicio, DateTime fechaFin);
        List<rptInventarioComponenteDTO> CargarReporteInventario(int grupo, int modelo, int conjunto, int subconjunto, string obra);

       

        

        List<rptValorAlmacenDTO> CargarReporteValorAlmacen(int anioInicial, int anioFinal);
        List<ComboDTO> FillCboAniosValorAlmacen();
        List<string> getLocaciones();
        List<rptListadoMaestroDTO> CargarReporteMaestro(int idCalendario);
        List<ComboDTO> CargarDatosDetalleMaestro(int idPlaneacionOH);
        List<ComboDTO> FillCboCalendarioReporteMaestro();
        List<ComboDTO> CargarDatosDetalleMaestroPlaneacion(string indexCal);
        List<tblP_CC> FillCboObraMaquinaID();
        Dictionary<string, object> FillCboObraMaquinaIDComboDTO(); 
        tblM_CapPlaneacionOverhaul CargarEventoOverhaul(int idPlaneacionOH);
        List<tblM_trackComponentes> GetReporteConjunto(bool enProceso);
        tblP_CC getCCByEconomico(string economico);
        tblM_trackComponentes GetSiguenteTracking(int id, int tipo);
        tblM_trackComponentes getUltimoTrackCRC(int componenteID);
        tblM_trackComponentes getTrackAnterior(int componenteID);
        tblM_trackComponentes GetTrackUltimaInstalacion(int componenteID, DateTime fecha);
        List<ComboDTO> getFacturaTrackAnterior(List<int> componentesID);
        List<ComboDTO> getLocacionTrackAnterior(List<int> componentesID);
        List<tblM_trackComponentes> CargarTablaHistorial(string componente, int subconjunto, string locacion, DateTime fechaInicio, DateTime fechaFin, int grupo, int modelo);
        RemocionDTO cargarDatosRemocionHistorial(int idComponente, int trackID);
        int GetVidasComponenteTracking(int trackID);
        int GetReporteDesechoID(int componenteID);
        string getDescripcionCCByCC(string centro_costos);
        List<ComboDTO> FillCboLocacionYObra();
        tblM_trackComponentes getTrackingByIDComp(int idComponente);
        bool GuardarEntradaAlmacen(int trackingID, DateTime fecha);
        bool ReactivarComponentesInactivos(int componenteID, int locacionID, DateTime fecha);

        DateTime getFechaFacturaEnkontrol(string factura);
        List<FechaFacturaEnkontrolDTO> getFechaFacturaEnkontrol(List<string> facturas);

        #region Reporte Component List
        List<ComboDTO> FillCboLocacionesComponentList(List<int> modelosID);
        List<ComboDTO> FillCboAlmacenesInventario();
        List<ComboDTO> FillCboComponentes();
        List<ComboDTO> FillCboConjuntos();
        List<ComboDTO> FillCboSubconjuntos(int conjunto);
        List<TrackingRitmoDTO> CargarComponentList(int locacion, string noComponente, int conjunto, int subconjunto, List<int> modelo, string obraAC);
        #endregion

        #region Reporte Componentes Reparacion




        #endregion



        #region Reporte Inventario
        List<tblM_trackComponentes> CargarInventario(int locacion, string noComponente, int conjunto, int subconjunto, List<int> modelo);

        #endregion

        #region REPORTE ALMACEN
        List<tblM_trackComponentes> CargarReporteAlmacen(string noComponente, List<int> idLocacion, string descripcionComponente, int estatus, int grupoId, int modeloId);

        #endregion

        int getNumeroTrackings(int noComponente);

        #region Reporte componentes reparacion
        List<tblM_trackComponentes> CargarCompReparacion(int locacion, int modelo, int grupo, int subconjunto);
        #endregion
    }
}