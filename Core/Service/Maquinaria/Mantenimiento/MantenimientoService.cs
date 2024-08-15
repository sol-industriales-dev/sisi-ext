using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Maquinaria.Mantenimiento;//interfaz
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
namespace Core.Service.Maquinaria.Mantenimiento
{
    public class MantenimientoService : IMantenimientoDAO
    {


        private IMantenimientoDAO MantenimientoDAO { get; set; }
        public MantenimientoService(IMantenimientoDAO MantenimientoDAO)
        {
            this.MantenimientoDAO = MantenimientoDAO;
        }
        public List<ComboDTO> GetInsumoEnkontrol(List<string> codigos)
        {
            return MantenimientoDAO.GetInsumoEnkontrol(codigos);
        }
        public List<objRestLubricantesDTO> getBitacoraLubricantesByMant(int idMant)
        {
            return MantenimientoDAO.getBitacoraLubricantesByMant(idMant);
        }

        public List<tblM_CatComponentesViscosidades> getCatComponentesViscosidadesByModelo(int modeloID)
        {
            return MantenimientoDAO.getCatComponentesViscosidadesByModelo(modeloID);
        }

        public List<tblM_CatFiltroMant> FillCboCatFiltros()
        {
            return MantenimientoDAO.FillCboCatFiltros();
        }
        public tblM_BitacoraActividadesMantProy getActividadesProByID(int id)
        {
            return MantenimientoDAO.getActividadesProByID(id);
        }

        public bool ActualizarActividadProgramada(tblM_BitacoraActividadesMantProy actividad)
        {
            return MantenimientoDAO.ActualizarActividadProgramada(actividad);
        }

        public void saveInfoResetLubricantes(List<objRestLubricantesDTO> obj)
        {
            MantenimientoDAO.saveInfoResetLubricantes(obj);
        }
        public List<tblM_CatPM> FillCombotablaPM(int Id, int Factor, string TipoMantenimiento)
        {
            return MantenimientoDAO.FillCombotablaPM(Id, Factor, TipoMantenimiento);
        }
        public tblM_MatenimientoPm GuardarPM(tblM_MatenimientoPm objMatenimiento)
        {
            return MantenimientoDAO.GuardarPM(objMatenimiento);
        }
        public tblM_BitacoraControlAEMantProy GetBitProyID(int uid)
        {
            return MantenimientoDAO.GetBitProyID(uid);
        }
        public tblM_BitacoraControlAceiteMant getBitHisLubByid(int id)
        {
            return MantenimientoDAO.getBitHisLubByid(id);
        }

        public object GuardarBitacoraDetActividadesMantProy(tblM_BitacoraDetActividadesMantProy ObjAct)
        {
            return MantenimientoDAO.GuardarBitacoraDetActividadesMantProy(ObjAct);
        }
        public List<tblM_MatenimientoPm> getListaEquiposAC(string areaCuenta)
        {
            return MantenimientoDAO.getListaEquiposAC(areaCuenta);
        }
        public tblM_BitacoraControlAceiteMantProy getBitProyLubByid(int id)
        {
            return MantenimientoDAO.getBitProyLubByid(id);
        }
        public object ConsultarPMActivo()
        {
            return MantenimientoDAO.ConsultarPMActivo();
        }
        public object ActividadadesbyID(int IDmaquinaria)
        {
            return MantenimientoDAO.ActividadadesbyID(IDmaquinaria);
        }
        public tblM_ActvContPM GuardarContadores(tblM_ActvContPM objActContPM)
        {
            return MantenimientoDAO.GuardarContadores(objActContPM);
        }
        public decimal ConsultarRitmoAutomatico(string EconomicoID)
        {
            return MantenimientoDAO.ConsultarRitmoAutomatico(EconomicoID);
        }
        public tblM_DocumentosMaquinaria getDocumentosByID(int id)
        {
            return MantenimientoDAO.getDocumentosByID(id);
        }
        public tblM_CapHorometro ConsultarUltimoHorometro(string fechaIniMante, string EconomicoID)
        {
            return MantenimientoDAO.ConsultarUltimoHorometro(fechaIniMante, EconomicoID);
        }
        public object ConsultarIDmaquinaria(string EconomicoID)
        {
            return MantenimientoDAO.ConsultarIDmaquinaria(EconomicoID);
        }
        public object ModificacionFecha(int idMaquina, DateTime FechaUpdate)
        {
            return MantenimientoDAO.ModificacionFecha(idMaquina, FechaUpdate);
        }
        public object ConsultarFechaUltimoHorometro(decimal Horometro, string EconomicoID)
        {
            return MantenimientoDAO.ConsultarFechaUltimoHorometro(Horometro, EconomicoID);
        }
        public Dictionary<string, object> ConsultarIntervaloFecha(DateTime Fecha, string EconomicoID)
        {
            return MantenimientoDAO.ConsultarIntervaloFecha(Fecha, EconomicoID);
        }
        public object ModificacionHorarioServicio(int idMaquina, DateTime inicio, DateTime fin)
        {
            return MantenimientoDAO.ModificacionHorarioServicio(idMaquina, inicio, fin);
        }
        public object FillGridActividad(tblM_CatActividadPM obj)
        {
            return MantenimientoDAO.FillGridActividad(obj);
        }
        public object GuardarNuevaActividad(tblM_CatActividadPM objCatActividadPM)
        {
            return MantenimientoDAO.GuardarNuevaActividad(objCatActividadPM);
        }
        public object ELiminarActividad(tblM_CatActividadPM objCatActividadPM)
        {
            return MantenimientoDAO.ELiminarActividad(objCatActividadPM);
        }
        public List<tblM_CatParteVidaUtil> getCatComponente(string term)
        {
            return MantenimientoDAO.getCatComponente(term);
        }
        public object FillGridParte(tblM_CatParteVidaUtil obj)
        {
            return MantenimientoDAO.FillGridParte(obj);
        }
        public object GuardarParte(tblM_CatParteVidaUtil objParte)
        {
            return MantenimientoDAO.GuardarParte(objParte);
        }
        public List<tblM_CatModeloEquipo> getCatModelo(string term)
        {
            return MantenimientoDAO.getCatModelo(term);
        }

        public List<getActividadModeloDTO> getActividadesModelos(int id)
        {
            return MantenimientoDAO.getActividadesModelos(id);
        }
        public object getActividadModelo(int id)
        {
            return MantenimientoDAO.getActividadModelo(id);
        }
        public object VinculaNuevaActividad(tblM_CatPM_CatActividadPM objActContPM)
        {
            return MantenimientoDAO.VinculaNuevaActividad(objActContPM);
        }
        public List<tblM_CatTipoActividad> getTipoActividad(bool estatus)
        {
            return MantenimientoDAO.getTipoActividad(estatus);
        }
        public List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus)
        {
            return MantenimientoDAO.FillCboTipoMaquinaria(estatus);
        }
        public List<tblM_CatMaquina> FillCboEconomicos(int grupo, int usuarioid)
        {
            return MantenimientoDAO.FillCboEconomicos(grupo, usuarioid);
        }
        public List<tblM_CatModeloEquipo> FillCboModelo_Maquina(int idTipo)
        {
            return MantenimientoDAO.FillCboModelo_Maquina(idTipo);
        }
        public List<tblM_CatActividadPM> getCatActividad(string term, int idTipo)
        {
            return MantenimientoDAO.getCatActividad(term, idTipo);
        }
        public object ELiminarVinc(tblM_CatPM_CatActividadPM obj)
        {
            return MantenimientoDAO.ELiminarVinc(obj);
        }
        public object FillGridComponenteVin(int modeloEquipoID, int idActs, int idTipoAct, int idpm)
        {
            return MantenimientoDAO.FillGridComponenteVin(modeloEquipoID, idActs, idTipoAct, idpm);
        }
        public object tipoLeyenda(tblM_CatPM_CatActividadPM obj)
        {
            return MantenimientoDAO.tipoLeyenda(obj);
        }
        public int GuardarDoc(tblM_DocumentosMaquinaria obj)
        {
            return MantenimientoDAO.GuardarDoc(obj);
        }
        public List<object> FillGridMiselaneo(int modeloEquipoID, int idAct, int idCompVis, int idTipo)
        {
            return MantenimientoDAO.FillGridMiselaneo(modeloEquipoID, idAct, idCompVis, idTipo);
        }
        public object GuardarDocFormat(tblM_FormatoManteniento obj)
        {
            return MantenimientoDAO.GuardarDocFormat(obj);
        }
        public tblM_DocumentosMaquinaria GetObjRutaDocumentobyID(int objIdFormato)
        {
            return MantenimientoDAO.GetObjRutaDocumentobyID(objIdFormato);
        }
        public object VincularComponete(tblM_ComponenteMantenimiento obj)
        {
            return MantenimientoDAO.VincularComponete(obj);
        }
        public object VincularMis(tblM_MiscelaneoMantenimiento obj)
        {
            return MantenimientoDAO.VincularMis(obj);
        }
        public object VincularEdadMis(int id, int edad)
        {
            return MantenimientoDAO.VincularEdadMis(id, edad);
        }
        public List<JGEstructuraDTO> ConsultarJGEstructura(int modeloEquipoID)
        {
            return MantenimientoDAO.ConsultarJGEstructura(modeloEquipoID);
        }

        public List<dataSetLubProxDTO> ConsultarJGEstructura2(int modeloEquipoID)
        {
            return MantenimientoDAO.ConsultarJGEstructura2(modeloEquipoID);
        }
        public int ConsultarModelo(string noEconomico)
        {
            return MantenimientoDAO.ConsultarModelo(noEconomico);
        }
        public object VincularEdadAct(int id, int edad)
        {
            return MantenimientoDAO.VincularEdadAct(id, edad);
        }
        public void EliminarVincDoc(int idActividad, int modelo)
        {
            MantenimientoDAO.EliminarVincDoc(idActividad, modelo);
        }
        public List<actividadesExtraDTO> ConsultarActividadesExtras(int modeloEquipoID)
        {
            return MantenimientoDAO.ConsultarActividadesExtras(modeloEquipoID);
        }
        public tblM_BitacoraControlAceiteMant GuardarBitJG(tblM_BitacoraControlAceiteMant objBitJG)
        {
            return MantenimientoDAO.GuardarBitJG(objBitJG);
        }
        public tblM_BitacoraControlActExt GuardarBitAE(tblM_BitacoraControlActExt objAE)
        {
            return MantenimientoDAO.GuardarBitAE(objAE);
        }
        public object VincularCantidadMis(int id, int cantidad)
        {
            return MantenimientoDAO.VincularCantidadMis(id, cantidad);
        }
        public List<object> ConsultarBitacora(string noEconomico)
        {
            return MantenimientoDAO.ConsultarBitacora(noEconomico);
        }
        public bool ActividadExistente(tblM_CatPM_CatActividadPM objActContPM)//valida si existe la actividad
        {
            return MantenimientoDAO.ActividadExistente(objActContPM);
        }
        public List<ActividadesDNDTO> ConsultarActividadesDN(int modeloEquipoID, int idPM)
        {
            return MantenimientoDAO.ConsultarActividadesDN(modeloEquipoID, idPM);
        }
        public tblM_BitacoraControlDN GuardarBitDN(tblM_BitacoraControlDN objDN)
        {
            return MantenimientoDAO.GuardarBitDN(objDN);
        }
        public void ProgramaActividades(int idmantenimiento)
        {
            MantenimientoDAO.ProgramaActividades(idmantenimiento);
        }
        public tblM_MatenimientoPm ConsultarPMbyID(int idobjMatenimientoPm)
        {
            return MantenimientoDAO.ConsultarPMbyID(idobjMatenimientoPm);
        }
        public string getEmpleadosID(string id)
        {
            return MantenimientoDAO.getEmpleadosID(id);
        }
        public List<JGHisDTO> ConsultarJGHis(int idMantenimiento)
        {
            return MantenimientoDAO.ConsultarJGHis(idMantenimiento);
        }
        public List<actividadesExtraHisDTO> ConsultarActividadesExtrashis(int idMantenimiento)
        {
            return MantenimientoDAO.ConsultarActividadesExtrashis(idMantenimiento);
        }
        public Dictionary<string, object> GetModeloEconomico(int idNoEconomico)
        {
            return MantenimientoDAO.GetModeloEconomico(idNoEconomico);
        }
        public tblM_BitacoraControlAceiteMantProy GuardarBitProyLub(tblM_BitacoraControlAceiteMantProy objLubProy)
        {
            return MantenimientoDAO.GuardarBitProyLub(objLubProy);
        }
        public List<tblM_BitacoraControlAceiteMantProy> CargaDeProyectado(int idmantenimiento)
        {
            return MantenimientoDAO.CargaDeProyectado(idmantenimiento);
        }
        public object CargaLubObs(int idMantProy)
        {
            return MantenimientoDAO.CargaLubObs(idMantProy);
        }
        public List<object> ConsultarGestorPM(int modeloEquipoID, int idPM, int idmantenimiento)
        {
            return MantenimientoDAO.ConsultarGestorPM(modeloEquipoID, idPM, idmantenimiento);
        }
        public object GuardarActividadesMantProy(tblM_BitacoraActividadesMantProy objActvProy)
        {
            return MantenimientoDAO.GuardarActividadesMantProy(objActvProy);
        }
        public object GuardarActividadProy(tblM_BitacoraActividadesMantProy ObjAct)
        {
            return MantenimientoDAO.GuardarActividadProy(ObjAct);
        }
        public List<object> ActOtroPm(int idMant)
        {
            return MantenimientoDAO.ActOtroPm(idMant);
        }
        public string ConsultarObservacionActividad(int idObjAct)
        {
            return MantenimientoDAO.ConsultarObservacionActividad(idObjAct);
        }
        public List<ActividadesDNHisDTO> ConsultarActividadesDNhis(int idMantenimiento)
        {
            return MantenimientoDAO.ConsultarActividadesDNhis(idMantenimiento);
        }
        public tblM_BitacoraControlDNMantProy GuardarBitProyDN(tblM_BitacoraControlDNMantProy ObjBitProyDN)
        {
            return MantenimientoDAO.GuardarBitProyDN(ObjBitProyDN);
        }
        public tblM_BitacoraControlAEMantProy GuardarBitProyAE(tblM_BitacoraControlAEMantProy ObjBitProyAE)
        {
            return MantenimientoDAO.GuardarBitProyAE(ObjBitProyAE);
        }
        public List<tblM_BitacoraControlAEMantProy> CargaDeAEProyectado(int idmantenimiento)
        {
            return MantenimientoDAO.CargaDeAEProyectado(idmantenimiento);
        }
        public int DeshabilitarLubProy(int idobjLub)
        {
            return MantenimientoDAO.DeshabilitarLubProy(idobjLub);
        }
        public int ConsultaModelobyMantenimiento(int idMant)
        {
            return MantenimientoDAO.ConsultaModelobyMantenimiento(idMant);
        }
        public int DeshabilitarACProy(int id)
        {
            return MantenimientoDAO.DeshabilitarACProy(id);
        }
        public List<tblM_BitacoraControlDNMantProy> CargaDeDNProyectado(int idmantenimiento)
        {
            return MantenimientoDAO.CargaDeDNProyectado(idmantenimiento);
        }
        public int DeshabilitarDNProy(int id)
        {
            return MantenimientoDAO.DeshabilitarDNProy(id);
        }
        public List<actividadesPMDTO> getFormato(int id, int idpm)
        {
            return MantenimientoDAO.getFormato(id, idpm);
        }
        public tblM_ParamReport ConsultarMantenimientobyID(int idobjMatenimientoPm)
        {
            return MantenimientoDAO.ConsultarMantenimientobyID(idobjMatenimientoPm);
        }
        public object ConsultarActPmbyModelo(int Modelo, int idact)
        {
            return MantenimientoDAO.ConsultarActPmbyModelo(Modelo, idact);
        }
        public object FillGridComponenteRestrinccion(int modeloEquipoID, int idActs, int idTipoAct, int idpm)
        {
            return MantenimientoDAO.FillGridComponenteRestrinccion(modeloEquipoID, idActs, idTipoAct, idpm);
        }

        public object ConsultarPMActivoByObra(string obra)
        {
            return MantenimientoDAO.ConsultarPMActivoByObra(obra);
        }

        public PuestosDTO ConsultaPersonalIdManteniminto(int numEmpleado)
        {
            return MantenimientoDAO.ConsultaPersonalIdManteniminto(numEmpleado);
        }

        public List<tblM_CatComponentesViscosidades> getCatComponentesViscosidades()
        {
            return MantenimientoDAO.getCatComponentesViscosidades();
        }
        public List<tblM_CatPM_CatActividadPM> getActividadesByPM(int modeloEquipoID, int tipoPM)
        {
            return MantenimientoDAO.getActividadesByPM(modeloEquipoID, tipoPM);
        }

        public void guardarDetAct(List<tblM_BitacoraControlAceiteMant> arrJG, tblM_BitacoraActividadesMantProy actividadExtra, int idMantenimiento, tblM_CatMaquina maquinaria)
        {
            MantenimientoDAO.guardarDetAct(arrJG, actividadExtra, idMantenimiento, maquinaria);
        }

        //Oscar
        public List<tblM_MatenimientoPm> GetMantenimientosProg(string cc)
        {
            return MantenimientoDAO.GetMantenimientosProg(cc);
        }
        public tblM_MatenimientoPm GuardarEjecutado(tblM_MatenimientoPm objGeneral, int idMantenimiento, List<tblM_BitacoraControlAceiteMantProy> tblGridLubProxTbl, List<tblM_BitacoraControlAEMantProy> tblGridActProxTbl, List<tblM_BitacoraControlDNMantProy> tblgridDNProxTbl, List<tblM_MantenimientoPm_Archivo> referencias)
        {
            return MantenimientoDAO.GuardarEjecutado(objGeneral, idMantenimiento, tblGridLubProxTbl, tblGridActProxTbl, tblgridDNProxTbl, referencias);
        }
        public tblM_MantenimientoPm_Archivo GetUltimoArchivoMantenimiento()
        {
            return MantenimientoDAO.GetUltimoArchivoMantenimiento();
        }
        public ReporteProgramadoDTO GetReporteProgramado(int idMant)
        {
            return MantenimientoDAO.GetReporteProgramado(idMant);
        }
        //---

        public List<dtPlaneacionSemanalPM> getPlaneacionSemanal(string areaCuenta, DateTime fechaInicio, DateTime FechaFin, string economico, bool ejecutado)
        {
            return MantenimientoDAO.getPlaneacionSemanal(areaCuenta, fechaInicio, FechaFin, economico, ejecutado);
        }

        public List<gridDetActividadesDTO> getDetActividadesMantProy(int idMant)
        {
            return MantenimientoDAO.getDetActividadesMantProy(idMant);
        }

        //Alta filtros
        public List<tblM_CatMarcaMant> fillCboMarcaFiltro()
        {
            return MantenimientoDAO.fillCboMarcaFiltro();
        }
        public bool GuardarFiltro(tblM_CatFiltroMant obj)
        {
            return MantenimientoDAO.GuardarFiltro(obj);
        }

        //Eliminar Actividad Proy
        public bool EliminarActividadProy(tblM_BitacoraActividadesMantProy ObjAct)
        {
            return MantenimientoDAO.EliminarActividadProy(ObjAct);
        }

        //Descarga de archivos PM
        public byte[] GetZipDocumentosPM(List<tblM_DocumentosMaquinaria> documentos)
        {
            return MantenimientoDAO.GetZipDocumentosPM(documentos);
        }
        public List<tblM_DocumentosMaquinaria> getDocumentosByID(List<int> ids)
        {
            return MantenimientoDAO.getDocumentosByID(ids);
        }
        public Dictionary<string, object> GuardarDocumentoPM(tblM_DocumentoMantenimientoPM objDocumentoPM, HttpPostedFileBase objFile)
        {
            return MantenimientoDAO.GuardarDocumentoPM(objDocumentoPM, objFile);
        }
        public Tuple<Stream, string> descargarArchivo(int idArchivo)
        {
            return MantenimientoDAO.descargarArchivo(idArchivo);
        }
        public Dictionary<string, object> GetArchivosAdjuntos(int idArchivo)
        {
            return MantenimientoDAO.GetArchivosAdjuntos(idArchivo);
        }

        public MemoryStream DescargarExcelJOMAGALI(string areaCuenta, DateTime fechaInicio, DateTime fechaFin, string economico)
        {
            return MantenimientoDAO.DescargarExcelJOMAGALI(areaCuenta, fechaInicio, fechaFin, economico);
        }
    }
}
