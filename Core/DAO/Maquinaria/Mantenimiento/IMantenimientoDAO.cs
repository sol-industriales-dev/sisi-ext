using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Maquinaria.Mantenimiento;
using Core.DTO.RecursosHumanos;
using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria;
using System.Web;
using Core.Entity.Maquinaria.Captura;
using System.IO;
namespace Core.DAO.Maquinaria.Mantenimiento
{
    public interface IMantenimientoDAO
    {
        List<ComboDTO> GetInsumoEnkontrol(List<string> codigos);
        void saveInfoResetLubricantes(List<objRestLubricantesDTO> obj);
        List<objRestLubricantesDTO> getBitacoraLubricantesByMant(int idMant);
        List<tblM_CatComponentesViscosidades> getCatComponentesViscosidadesByModelo(int modeloID);
        List<getActividadModeloDTO> getActividadesModelos(int id);
        List<tblM_CatFiltroMant> FillCboCatFiltros();
        tblM_BitacoraActividadesMantProy getActividadesProByID(int id);
        bool ActualizarActividadProgramada(tblM_BitacoraActividadesMantProy id);
        tblM_BitacoraControlAceiteMant getBitHisLubByid(int id);
        object GuardarBitacoraDetActividadesMantProy(tblM_BitacoraDetActividadesMantProy ObjAct);
        List<gridDetActividadesDTO> getDetActividadesMantProy(int idMant);
        void guardarDetAct(List<tblM_BitacoraControlAceiteMant> arrJG, tblM_BitacoraActividadesMantProy actividadExtra, int idMantenimiento, tblM_CatMaquina maquinaria);
        List<tblM_MatenimientoPm> getListaEquiposAC(string areaCuenta);
        tblM_BitacoraControlAEMantProy GetBitProyID(int uid);
        tblM_BitacoraControlAceiteMantProy getBitProyLubByid(int id);
        List<tblM_CatPM_CatActividadPM> getActividadesByPM(int modeloEquipoID, int tipoPM);
        tblM_DocumentosMaquinaria getDocumentosByID(int id);
        List<dtPlaneacionSemanalPM> getPlaneacionSemanal(string areaCuenta, DateTime fechaInicio, DateTime FechaFin, string economico, bool ejecutado);
        List<dataSetLubProxDTO> ConsultarJGEstructura2(int modeloEquipoID);
        List<tblM_CatComponentesViscosidades> getCatComponentesViscosidades();
        List<tblM_CatPM> FillCombotablaPM(int Id, int Factor, string TipoMantenimiento);
        tblM_MatenimientoPm GuardarPM(tblM_MatenimientoPm objMantenimiento);
        object ConsultarPMActivo();
        object ActividadadesbyID(int IDmaquinaria);
        tblM_ActvContPM GuardarContadores(tblM_ActvContPM objActContPM);
        decimal ConsultarRitmoAutomatico(string EconomicoID);
        tblM_CapHorometro ConsultarUltimoHorometro(string fechaIniMante, string EconomicoID);
        object ConsultarIDmaquinaria(string EconomicoID);
        object ModificacionFecha(int idMaquina, DateTime FechaUpdate);
        object ConsultarFechaUltimoHorometro(decimal Horometro, string EconomicoID);
        Dictionary<string, object> ConsultarIntervaloFecha(DateTime Fecha, string EconomicoID);
        object ModificacionHorarioServicio(int idMaquina, DateTime inicio, DateTime fin);
        object FillGridActividad(tblM_CatActividadPM obj);
        object GuardarNuevaActividad(tblM_CatActividadPM objCatActividadPM);
        object ELiminarActividad(tblM_CatActividadPM objCatActividadPM);
        List<tblM_CatParteVidaUtil> getCatComponente(string term);
        object FillGridParte(tblM_CatParteVidaUtil obj);
        object GuardarParte(tblM_CatParteVidaUtil objParte);
        List<tblM_CatModeloEquipo> getCatModelo(string term);
        object getActividadModelo(int id);
        object VinculaNuevaActividad(tblM_CatPM_CatActividadPM objActContPM);
        List<tblM_CatTipoActividad> getTipoActividad(bool estatus);
        List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus);
        List<tblM_CatMaquina> FillCboEconomicos(int grupo, int usuarioid);
        List<tblM_CatModeloEquipo> FillCboModelo_Maquina(int idTipo);
        List<tblM_CatActividadPM> getCatActividad(string term, int idTipo);
        object ELiminarVinc(tblM_CatPM_CatActividadPM obj);
        object FillGridComponenteVin(int modeloEquipoID, int idActs, int idTipoAct, int idpm);
        object tipoLeyenda(tblM_CatPM_CatActividadPM obj);
        List<object> FillGridMiselaneo(int modeloEquipoID, int idAct, int idCompVis, int idTipo);
        int GuardarDoc(tblM_DocumentosMaquinaria obj);
        object GuardarDocFormat(tblM_FormatoManteniento obj);
        tblM_DocumentosMaquinaria GetObjRutaDocumentobyID(int objIdFormato);
        object VincularComponete(tblM_ComponenteMantenimiento obj);
        object VincularMis(tblM_MiscelaneoMantenimiento obj);
        object VincularEdadMis(int id, int edad);
        List<JGEstructuraDTO> ConsultarJGEstructura(int modeloEquipoID);
        int ConsultarModelo(string noEconomico);
        object VincularEdadAct(int id, int edad);
        void EliminarVincDoc(int idActividad, int modelo);
        List<actividadesExtraDTO> ConsultarActividadesExtras(int modeloEquipoID);
        Dictionary<string, object> GetModeloEconomico(int idNoEconomico);
        tblM_BitacoraControlAceiteMant GuardarBitJG(tblM_BitacoraControlAceiteMant objBitJG);
        tblM_BitacoraControlActExt GuardarBitAE(tblM_BitacoraControlActExt objAE);
        object VincularCantidadMis(int id, int cantidad);
        List<object> ConsultarBitacora(string noEconomico);
        bool ActividadExistente(tblM_CatPM_CatActividadPM objActContPM);
        List<ActividadesDNDTO> ConsultarActividadesDN(int modeloEquipoID, int idPM);
        tblM_BitacoraControlDN GuardarBitDN(tblM_BitacoraControlDN objDN);
        void ProgramaActividades(int idmantenimiento);
        tblM_MatenimientoPm ConsultarPMbyID(int idobjMatenimientoPm);
        string getEmpleadosID(string id);
        List<JGHisDTO> ConsultarJGHis(int idMantenimiento);
        List<actividadesExtraHisDTO> ConsultarActividadesExtrashis(int idMantenimiento);
        tblM_BitacoraControlAceiteMantProy GuardarBitProyLub(tblM_BitacoraControlAceiteMantProy objLubProy);
        List<tblM_BitacoraControlAceiteMantProy> CargaDeProyectado(int idmantenimiento);
        object CargaLubObs(int idMantProy);
        List<object> ConsultarGestorPM(int modeloEquipoID, int idPM, int idmantenimiento);
        object GuardarActividadesMantProy(tblM_BitacoraActividadesMantProy objActvProy);
        object GuardarActividadProy(tblM_BitacoraActividadesMantProy ObjAct);
        List<object> ActOtroPm(int idMant);
        string ConsultarObservacionActividad(int idObjAct);
        List<ActividadesDNHisDTO> ConsultarActividadesDNhis(int idMantenimiento);
        tblM_BitacoraControlDNMantProy GuardarBitProyDN(tblM_BitacoraControlDNMantProy ObjBitProyDN);
        tblM_BitacoraControlAEMantProy GuardarBitProyAE(tblM_BitacoraControlAEMantProy ObjBitProyAE);
        List<tblM_BitacoraControlAEMantProy> CargaDeAEProyectado(int idmantenimiento);
        int DeshabilitarLubProy(int idobjLub);
        int ConsultaModelobyMantenimiento(int idMant);
        int DeshabilitarACProy(int id);
        List<tblM_BitacoraControlDNMantProy> CargaDeDNProyectado(int idmantenimiento);
        int DeshabilitarDNProy(int id);
        List<actividadesPMDTO> getFormato(int id, int idpm);
        tblM_ParamReport ConsultarMantenimientobyID(int idobjMatenimientoPm);
        object ConsultarActPmbyModelo(int Modelo, int idact);
        object FillGridComponenteRestrinccion(int modeloEquipoID, int idActs, int idTipoAct, int idpm);
        /**/
        object ConsultarPMActivoByObra(string obra);
        PuestosDTO ConsultaPersonalIdManteniminto(int numEmpleado);
        /**/

        //Oscar
        List<tblM_MatenimientoPm> GetMantenimientosProg(string cc);
        tblM_MatenimientoPm GuardarEjecutado(tblM_MatenimientoPm objGeneral, int idMantenimiento, List<tblM_BitacoraControlAceiteMantProy> tblGridLubProxTbl, List<tblM_BitacoraControlAEMantProy> tblGridActProxTbl, List<tblM_BitacoraControlDNMantProy> tblgridDNProxTbl, List<tblM_MantenimientoPm_Archivo> referencias);
        tblM_MantenimientoPm_Archivo GetUltimoArchivoMantenimiento();
        ReporteProgramadoDTO GetReporteProgramado(int idMant);
        //---

        //Alta Filtros
        List<tblM_CatMarcaMant> fillCboMarcaFiltro();
        bool GuardarFiltro(tblM_CatFiltroMant obj);

        //Eliminar Actividad Proy
        bool EliminarActividadProy(tblM_BitacoraActividadesMantProy ObjAct);

        //Descargar Archivos
        byte[] GetZipDocumentosPM(List<tblM_DocumentosMaquinaria> documentos);
        List<tblM_DocumentosMaquinaria> getDocumentosByID(List<int> ids);
        Dictionary<string, object> GuardarDocumentoPM(tblM_DocumentoMantenimientoPM objDocumentoPM, HttpPostedFileBase objFile);
        Tuple<Stream, string> descargarArchivo(int idArchivo);
        Dictionary<string, object> GetArchivosAdjuntos(int idArchivo);

        MemoryStream DescargarExcelJOMAGALI(string areaCuenta, DateTime fechaInicio, DateTime fechaFin, string economico);
    }
}
