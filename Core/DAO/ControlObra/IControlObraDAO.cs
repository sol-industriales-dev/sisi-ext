using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using System.IO;
using System.Web;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.ControlObra;


namespace Core.DAO.ControlObra
{
    public interface IControlObraDAO
    {
        #region CAPITULOS
        Dictionary<string, object> getCapitulosList(int capituloID);
        Dictionary<string, object> getCapitulosCatalogo();
        Dictionary<string, object> getCapitulo(int capituloID);
        Dictionary<string, object> guardarCapitulo(tblCO_Capitulos capitulo);
        Dictionary<string, object> updateCapitulo(int capituloID, string capitulo, DateTime fechaInicio, DateTime fechaFin, int? cc_id, int? autorizante_id, int? periodoFacturacion);
        Dictionary<string, object> removeCapitulo(int capituloID);
        Dictionary<string, object> obtenerCentrosCostos();
        Dictionary<string, object> getPeriodoFacturacion();
        #endregion

        #region SUBCAPITULOS NIVEL I
        Dictionary<string, object> getSubcapitulosN1List(int capituloID);
        Dictionary<string, object> getSubcapitulosN1Catalogo(List<int> listCapitulosID);
        Dictionary<string, object> getSubcapituloN1(int subcapituloID);
        Dictionary<string, object> guardarSubcapituloN1(tblCO_Subcapitulos_Nivel1 subcapituloN1);
        Dictionary<string, object> updateSubcapituloN1(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int capituloID);
        Dictionary<string, object> removeSubcapituloN1(int subcapituloID);
        #endregion

        #region SUBCAPITULOS NIVEL II
        Dictionary<string, object> getSubcapitulosN2List(int subcapituloN1_id);
        Dictionary<string, object> getSubcapitulosN2Catalogo(int subcapituloN1_id);
        Dictionary<string, object> getSubcapituloN2(int subcapituloID);
        Dictionary<string, object> guardarSubcapituloN2(tblCO_Subcapitulos_Nivel2 subcapituloN2);
        Dictionary<string, object> updateSubcapituloN2(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN1_id);
        Dictionary<string, object> removeSubcapituloN2(int subcapituloID);
        #endregion

        #region SUBCAPITULOS NIVEL III
        Dictionary<string, object> getSubcapitulosN3List(int subcapituloN2_id);
        Dictionary<string, object> getSubcapitulosN3Catalogo(int subcapituloN2_id);
        Dictionary<string, object> getSubcapituloN3(int subcapituloID);
        Dictionary<string, object> guardarSubcapituloN3(tblCO_Subcapitulos_Nivel3 subcapituloN3);
        Dictionary<string, object> updateSubcapituloN3(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN2_id);
        Dictionary<string, object> removeSubcapituloN3(int subcapituloID);
        #endregion

        #region ACTIVIDADES
        Dictionary<string, object> getActividadesList(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id);
        Dictionary<string, object> getActividadLigadaSiguiente(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id);
        Dictionary<string, object> getActividadesCatalogo(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id);
        Dictionary<string, object> getActividad(int actividadID);
        Dictionary<string, object> guardarActividad(string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, bool estatus, int? actividadPadre_id, bool actividadPadreRequerida, bool actividadTerminada);
        Dictionary<string, object> updateActividad(int actividadID, string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, int? actividadPadre_id, bool actividadPadreRequerida);
        Dictionary<string, object> removeActividad(int actividadID);
        Dictionary<string, object> updateActividadPeriodoValor(List<ActividadPeriodoAvanceDTO> actividadPeriodo);
        #endregion

        #region UNIDADES
        Dictionary<string, object> getUnidadesList();
        Dictionary<string, object> getUnidadesCatalogo();
        Dictionary<string, object> getUnidad(int unidadID);
        Dictionary<string, object> guardarUnidad(string unidad);
        Dictionary<string, object> editarUnidad(int unidadID, string unidad);
        Dictionary<string, object> removeUnidad(int unidadID);
        #endregion

        #region ACTIVIDAD AVANCE
        Dictionary<string, object> guardarAvance(ActividadAvanceDTO actividadAvance, List<ActividadAvanceDetalleDTO> actividadAvance_detalle);
        Dictionary<string, object> getFechasUltimoAvance(int actividadID);
        Dictionary<string, object> getActividadAvanceDetalleAutorizar(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, int actividadAvance_id);
        Dictionary<string, object> guardarAutorizacion(bool autorizacion, int avance_id);
        Dictionary<string, object> guardarAvanceFacturado(FacturadoDTO facturado, List<FacturadoDetalleDTO> facturadoDetalle);
        Dictionary<string, object> getPeriodoAvance();

        //Dictionary<string, object> validaActividadAvanceEdit(int actividadAvanceID, int actividadID, decimal cantidad, DateTime fechaInicio, DateTime fechaFin);
        //Dictionary<string, object> removeActividadAvance(int actividadAvanceID, int actividadID);
       
        #endregion

        #region REPORTE AVANCES
        Dictionary<string, object> getAvancesAutorizar(int capituloID);
        Dictionary<string, object> getActividadAvanceReporte(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getConcentradoReporte(int capitulo_id, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region IMPORTAR ARCHIVO OPUS
        Dictionary<string, object> guardarInfoOpus(string nombreObra, string nombreSinEspacios, List<HttpPostedFileBase> archivos, int? periodoFacturacion, int? cc_id, int? autorizande_id);

        #endregion

        #region INFORME SEMANAL
        Dictionary<string, object> ObtenerDivisiones();
        Dictionary<string, object> obtenerDivisionCC();
        Dictionary<string, object> getInformesSemanal(int division_id);
        Dictionary<string, object> getUltimoInforme();
        Dictionary<string, object> getInformeSemanal(int informe_id);
        Dictionary<string, object> getInformeSemanalContenido(int informe_id);
        Dictionary<string, object> getPlantillaInformeDetalle(int plantilla_id);
        Dictionary<string, object> getPlantillaInformeDetalleCC();
        Dictionary<string, object> getPlantillaDivision(int divisionID);
        Dictionary<string, object> guardarPlantilla(PlantillaInformeDTO plantilla, List<PlantillaInforme_detalleDTO> plantilla_detalle);
        Dictionary<string, object> guardarPlantillaContenido(int plantilla_id, List<tblCO_PlantillaInforme_detalle> plantilla_contenido);
        Dictionary<string, object> guardarInforme(informeSemanalDTO informe, List<informeSemanal_detalleDTO> informe_detalle);
        #endregion


        #region SUBCONTRATISTA

        List<ComboDTO> getProyecto();
        List<ComboDTO> getSubContratistas();
        SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros);
        SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo,List<SubContratistasDTO> parametros);
        List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros);
        List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros);

        List<DivicionesMenuDTO> obtenerDiviciones();
        List<DivicionesMenuDTO> obtenerDivicionesEvaluador();
        DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros);
        DivicionesMenuDTO eliminarDiviciones(int id);
        List<RequerimientosDTO> obtenerRequerimientos(int idDiv);
        byte[] DescargarArchivos(long idDet);
        string getFileName(long idDet);
        #endregion


    }
}
