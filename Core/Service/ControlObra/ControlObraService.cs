using Core.DAO.ControlObra;
using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.DTO.Principal.Generales;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.ControlObra;


namespace Core.Service.ControlObra
{
    public class ControlObraService : IControlObraDAO
    {
        private IControlObraDAO m_ControlObraDAO;
        public IControlObraDAO ControlObraDAO
        {
            get { return m_ControlObraDAO; }
            set { m_ControlObraDAO = value; }
        }
        public ControlObraService(IControlObraDAO ControlObraDAO)
        {
            this.ControlObraDAO = ControlObraDAO;
        }

        #region CAPITULOS
        public Dictionary<string, object> getCapitulosList(int capituloID)
        {
            return ControlObraDAO.getCapitulosList(capituloID);
        }
        public Dictionary<string, object> getCapitulosCatalogo()
        {
            return ControlObraDAO.getCapitulosCatalogo();
        }
        public Dictionary<string, object> getCapitulo(int capituloID)
        {
            return ControlObraDAO.getCapitulo(capituloID);
        }
        public Dictionary<string, object> guardarCapitulo(tblCO_Capitulos capitulo)
        {
            return ControlObraDAO.guardarCapitulo(capitulo);
        }
        public Dictionary<string, object> updateCapitulo(int capituloID, string capitulo, DateTime fechaInicio, DateTime fechaFin, int? cc_id, int? autorizante_id, int? periodoFacturacion)
        {
            return ControlObraDAO.updateCapitulo(capituloID, capitulo, fechaInicio, fechaFin, cc_id, autorizante_id, periodoFacturacion);
        }
        public Dictionary<string, object> removeCapitulo(int capituloID)
        {
            return ControlObraDAO.removeCapitulo(capituloID);
        }
        public Dictionary<string, object> obtenerCentrosCostos()
        {
            return ControlObraDAO.obtenerCentrosCostos();
        }
        public Dictionary<string, object> getPeriodoFacturacion()
        {
            return ControlObraDAO.getPeriodoFacturacion();
        }
        #endregion

        #region SUBCAPITULOS NIVEL I
        public Dictionary<string, object> getSubcapitulosN1List(int capituloID)
        {
            return ControlObraDAO.getSubcapitulosN1List(capituloID);
        }
        public Dictionary<string, object> getSubcapitulosN1Catalogo(List<int> listCapitulosID)
        {
            return ControlObraDAO.getSubcapitulosN1Catalogo(listCapitulosID);
        }
        public Dictionary<string, object> getSubcapituloN1(int subcapituloID)
        {
            return ControlObraDAO.getSubcapituloN1(subcapituloID);
        }
        public Dictionary<string, object> guardarSubcapituloN1(tblCO_Subcapitulos_Nivel1 subcapituloN1)
        {
            return ControlObraDAO.guardarSubcapituloN1(subcapituloN1);
        }
        public Dictionary<string, object> updateSubcapituloN1(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int capituloID)
        {
            return ControlObraDAO.updateSubcapituloN1(subcapituloID, subcapitulo, fechaInicio, fechaFin, capituloID);
        }
        public Dictionary<string, object> removeSubcapituloN1(int subcapituloID)
        {
            return ControlObraDAO.removeSubcapituloN1(subcapituloID);
        }
        #endregion

        #region SUBCAPITULOS NIVEL II
        public Dictionary<string, object> getSubcapitulosN2List(int subcapituloN1_id)
        {
            return ControlObraDAO.getSubcapitulosN2List(subcapituloN1_id);
        }
        public Dictionary<string, object> getSubcapitulosN2Catalogo(int subcapituloN1_id)
        {
            return ControlObraDAO.getSubcapitulosN2Catalogo(subcapituloN1_id);
        }
        public Dictionary<string, object> getSubcapituloN2(int subcapituloID)
        {
            return ControlObraDAO.getSubcapituloN2(subcapituloID);
        }
        public Dictionary<string, object> guardarSubcapituloN2(tblCO_Subcapitulos_Nivel2 subcapituloN2)
        {
            return ControlObraDAO.guardarSubcapituloN2(subcapituloN2);
        }
        public Dictionary<string, object> updateSubcapituloN2(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN1_id)
        {
            return ControlObraDAO.updateSubcapituloN2(subcapituloID, subcapitulo, fechaInicio, fechaFin, subcapituloN1_id);
        }
        public Dictionary<string, object> removeSubcapituloN2(int subcapituloID)
        {
            return ControlObraDAO.removeSubcapituloN2(subcapituloID);
        }
        #endregion

        #region SUBCAPITULOS NIVEL III
        public Dictionary<string, object> getSubcapitulosN3List(int subcapituloN2_id)
        {
            return ControlObraDAO.getSubcapitulosN3List(subcapituloN2_id);
        }
        public Dictionary<string, object> getSubcapitulosN3Catalogo(int subcapituloN2_id)
        {
            return ControlObraDAO.getSubcapitulosN3Catalogo(subcapituloN2_id);
        }
        public Dictionary<string, object> getSubcapituloN3(int subcapituloID)
        {
            return ControlObraDAO.getSubcapituloN3(subcapituloID);
        }
        public Dictionary<string, object> guardarSubcapituloN3(tblCO_Subcapitulos_Nivel3 subcapituloN3)
        {
            return ControlObraDAO.guardarSubcapituloN3(subcapituloN3);
        }
        public Dictionary<string, object> updateSubcapituloN3(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN2_id)
        {
            return ControlObraDAO.updateSubcapituloN3(subcapituloID, subcapitulo, fechaInicio, fechaFin, subcapituloN2_id);
        }
        public Dictionary<string, object> removeSubcapituloN3(int subcapituloID)
        {
            return ControlObraDAO.removeSubcapituloN3(subcapituloID);
        }
        #endregion

        #region ACTIVIDADES
        public Dictionary<string, object> getActividadesList(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            return ControlObraDAO.getActividadesList(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id);
        }
        public Dictionary<string, object> getActividadLigadaSiguiente(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            return ControlObraDAO.getActividadLigadaSiguiente(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id);
        }
        public Dictionary<string, object> getActividadesCatalogo(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            return ControlObraDAO.getActividadesCatalogo(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id);
        }
        public Dictionary<string, object> getActividad(int actividadID)
        {
            return ControlObraDAO.getActividad(actividadID);
        }
        public Dictionary<string, object> guardarActividad(string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, bool estatus, int? actividadPadre_id, bool actividadPadreRequerida, bool actividadTerminada)
        {
            return ControlObraDAO.guardarActividad(actividad, cantidad, unidad_id, fechaInicio, fechaFin, subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, estatus, actividadPadre_id, actividadPadreRequerida, actividadTerminada);
        }
        public Dictionary<string, object> updateActividad(int actividadID, string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, int? actividadPadre_id, bool actividadPadreRequerida)
        {
            return ControlObraDAO.updateActividad(actividadID, actividad, cantidad, unidad_id, fechaInicio, fechaFin, subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, actividadPadre_id, actividadPadreRequerida);
        }
        public Dictionary<string, object> removeActividad(int actividadID)
        {
            return ControlObraDAO.removeActividad(actividadID);
        }
        public Dictionary<string, object> updateActividadPeriodoValor(List<ActividadPeriodoAvanceDTO> actividadPeriodo)
        {
            return ControlObraDAO.updateActividadPeriodoValor(actividadPeriodo);
        }
        #endregion

        #region UNIDADES
        public Dictionary<string, object> getUnidadesList()
        {
            return ControlObraDAO.getUnidadesList();
        }
        public Dictionary<string, object> getUnidadesCatalogo()
        {
            return ControlObraDAO.getUnidadesCatalogo();
        }
        public Dictionary<string, object> getUnidad(int unidadID)
        {
            return ControlObraDAO.getUnidad(unidadID);
        }
        public Dictionary<string, object> guardarUnidad(string unidad)
        {
            return ControlObraDAO.guardarUnidad(unidad);
        }
        public Dictionary<string, object> editarUnidad(int unidadID, string unidad)
        {
            return ControlObraDAO.editarUnidad(unidadID, unidad);
        }
        public Dictionary<string, object> removeUnidad(int unidadID)
        {
            return ControlObraDAO.removeUnidad(unidadID);
        }
        #endregion

        #region ACTIVIDADES AVANCES
        public Dictionary<string, object> guardarAvance(ActividadAvanceDTO actividadAvance, List<ActividadAvanceDetalleDTO> actividadAvance_detalle)
        {
            return ControlObraDAO.guardarAvance(actividadAvance, actividadAvance_detalle);
        }
        public Dictionary<string, object> getFechasUltimoAvance(int actividadID)
        {
            return ControlObraDAO.getFechasUltimoAvance(actividadID);
        }
        public Dictionary<string, object> getActividadAvanceDetalleAutorizar(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, int actividadAvance_id)
        {
            return ControlObraDAO.getActividadAvanceDetalleAutorizar(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, actividadAvance_id);
        }
        public Dictionary<string, object> guardarAutorizacion(bool autorizacion, int avance_id)
        {
            return ControlObraDAO.guardarAutorizacion(autorizacion, avance_id);
        }
        public Dictionary<string, object> guardarAvanceFacturado(FacturadoDTO facturado, List<FacturadoDetalleDTO> facturadoDetalle)
        {
            return ControlObraDAO.guardarAvanceFacturado(facturado, facturadoDetalle);
        }
        public Dictionary<string, object> getPeriodoAvance()
        {
            return ControlObraDAO.getPeriodoAvance();
        }
        #endregion

        #region REPORTE AVANCES
        public Dictionary<string, object> getAvancesAutorizar(int capituloID)
        {
            return ControlObraDAO.getAvancesAutorizar(capituloID);
        }
        public Dictionary<string, object> getActividadAvanceReporte(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, DateTime fechaInicio, DateTime fechaFin)
        {
            return ControlObraDAO.getActividadAvanceReporte(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, fechaInicio, fechaFin);
        }
        public Dictionary<string, object> getConcentradoReporte(int capitulo_id, DateTime fechaInicio, DateTime fechaFin)
        {
            return ControlObraDAO.getConcentradoReporte(capitulo_id, fechaInicio, fechaFin);
        }
        #endregion

        #region IMPORTAR ARCHIVO OPUS
        public Dictionary<string, object> guardarInfoOpus(string nombreObra, string nombreSinEspacios, List<HttpPostedFileBase> archivos, int? periodoFacturacion, int? cc_id, int? autorizande_id)
        {
            return ControlObraDAO.guardarInfoOpus(nombreObra, nombreSinEspacios, archivos, periodoFacturacion, cc_id, autorizande_id);
        }
        #endregion

        #region INFORME SEMANAL
        public Dictionary<string, object> ObtenerDivisiones()
        {
            return ControlObraDAO.ObtenerDivisiones();
        }
        public Dictionary<string, object> obtenerDivisionCC()
        {
            return ControlObraDAO.obtenerDivisionCC();
        }
        public Dictionary<string, object> getInformesSemanal(int division_id)
        {
            return ControlObraDAO.getInformesSemanal(division_id);
        }
        public Dictionary<string, object> getUltimoInforme()
        {
            return ControlObraDAO.getUltimoInforme();
        }
        public Dictionary<string, object> getPlantillaDivision(int divisionID)
        {
            return ControlObraDAO.getPlantillaDivision(divisionID);
        }
        public Dictionary<string, object> getInformeSemanal(int informe_id)
        {
            return ControlObraDAO.getInformeSemanal(informe_id);
        }
        public Dictionary<string, object> getInformeSemanalContenido(int informe_id)
        {
            return ControlObraDAO.getInformeSemanalContenido(informe_id);
        }
        public Dictionary<string, object> getPlantillaInformeDetalle(int plantilla_id)
        {
            return ControlObraDAO.getPlantillaInformeDetalle(plantilla_id);
        }
        public Dictionary<string, object> getPlantillaInformeDetalleCC()
        {
            return ControlObraDAO.getPlantillaInformeDetalleCC();
        }
        public Dictionary<string, object> guardarPlantilla(PlantillaInformeDTO plantilla, List<PlantillaInforme_detalleDTO> plantilla_detalle)
        {
            return ControlObraDAO.guardarPlantilla(plantilla, plantilla_detalle);
        }
        public Dictionary<string, object> guardarPlantillaContenido(int plantilla_id, List<tblCO_PlantillaInforme_detalle> plantilla_contenido)
        {
            return ControlObraDAO.guardarPlantillaContenido(plantilla_id, plantilla_contenido);
        }
        public Dictionary<string, object> guardarInforme(informeSemanalDTO informe, List<informeSemanal_detalleDTO> informe_detalle)
        {
            return ControlObraDAO.guardarInforme(informe, informe_detalle);
        }
        #endregion


        #region SUB CONTRATISTA

        public List<ComboDTO> getProyecto()
        {
            return ControlObraDAO.getProyecto();
        }
        public List<ComboDTO> getSubContratistas()
        {
            return ControlObraDAO.getSubContratistas();
        }
        public SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros)
        {
            return ControlObraDAO.getTblSubContratista(parametros);
        }
        public SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros)
        {
            return ControlObraDAO.addEditSubContratista(Archivo, parametros);
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            return ControlObraDAO.CargarArchivosXSubcontratista(parametros);
        }
        public List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            return ControlObraDAO.ObtenerTblAutorizacion(parametros);
        }


        public List<DivicionesMenuDTO> obtenerDiviciones()
        {
            return ControlObraDAO.obtenerDiviciones();
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluador()
        {
            return ControlObraDAO.obtenerDivicionesEvaluador();
        }
        public DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros)
        {
            return ControlObraDAO.addEditDiviciones(parametros);
        }
        public DivicionesMenuDTO eliminarDiviciones(int id)
        {
            return ControlObraDAO.eliminarDiviciones(id);
        }
        public List<RequerimientosDTO> obtenerRequerimientos(int idDiv)
        {
            return ControlObraDAO.obtenerRequerimientos(idDiv);
        }

        public byte[] DescargarArchivos(long idDet)
        {
            return ControlObraDAO.DescargarArchivos(idDet);
        }
        public string getFileName(long idDet)
        {
            return ControlObraDAO.getFileName(idDet);
        }
        #endregion

    }
}
