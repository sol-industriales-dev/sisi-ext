using Core.DAO.Maquinaria.Rastreo;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Rastreo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface ISolicitudEquipoDAO
    {

        string GetFolio(string cc);
        void Guardar(tblM_SolicitudEquipo obj);
        void GuardarJustificaciones(int solicitudID, List<tblM_SM_Justificacion> array);
        void GuardarSolicitudEvidencia(int solicitudID, string dir);
        List<SolicitudEquipoJustificacionDTO> getListaJustificacionSolicitud(int idSolicitud);
        List<tblM_SolicitudEquipo> getListPendientes(int idUsuario, int tipo);
        List<SolicitudEquipoDTO> getListaDetalleSolicitud(int idSolicitud);
        List<tblM_SolicitudEquipo> getListAutorizadas(int idUsuario);
        tblM_SolicitudEquipo listaSolicitudEquipo(int idSolicitud);
        tblM_SolicitudEquipoDet GetSolicitudEquipoDet(int obj);
        List<SolicitudEquipoDTO> getListaDetalleSolicitudAutorizacion(int idSolicitud);
        tblM_SolicitudEquipo LoadDataSolicitud(string folio);
        List<SolicitudDetalleDTO> getListaSolicitudesPendientes(int idSolicitud);
        tblM_SolicitudEquipo loadSolicitudById(int id);
        List<tblP_Autoriza> GetAutorizadores(string cc);
        List<tblM_SolicitudEquipo> GetListaSolicitudesAutorizadas(int idUsuario);
        List<SolicitudesDetalleVacantesDTO> GetSolicitudesEquipo(string CentroCostos);
        List<DetalleVacantesSolicitudDTO> GetDetalleSolicitud(int solicitudes);
        List<DetalleGrupoPlantillaDTO> GetDataSolicitudesGrupo(int solicitudes, int grupo);
        List<tblM_SolicitudEquipo> GetEquipoNoMovimientos();

        List<tblM_AutorizacionSolicitudes> getTiemposAsignacion(List<string> CentroCostos);
        List<EquiposPendientesDTO> getEquiposPendientes(string cc, int Tipo, int grupo);
        List<EquiposPendientesReemplazoDTO> getEquiposReemplazo(string cc, int Tipo, int grupo);
        List<tblM_AutorizacionSolicitudes> GetListSolicitudesCC(List<string> centro_costos);
        string obtenerComentarioSolicitud(int solicitudID);
        //Funciones Envio Doc Gestor
        bool insertEnvioGestor(int solicitudID);
        tblM_SolicitudEquipo listaSolicitudEquipoPorEmpresa(int idSolicitud, int empresa);
        List<SolicitudEquipoDTO> getListaDetalleSolicitudAutorizacionPorEmpresa(int idSolicitud, int empresa);
        tblM_AutorizacionSolicitudes getAutorizadoresPorEmpresa(int idSolicitud, int empresa);
        List<tblM_SolicitudEquipoDet> listaDetalleSolicitudPorEmpresa(int obj, int empresa);
        List<tblM_AsignacionEquipos> getAsignacionesByIDPorEmpresa(int id, int empresa);

        Dictionary<string, object> getAsginaciones(int economicoID);
        Dictionary<string, object> GetAutorizadoresAC();
        void SetAutorizadorAC(int usuarioID, string ac, int perfil);
    }
}
