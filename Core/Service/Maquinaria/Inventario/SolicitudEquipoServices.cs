using Core.DAO.Maquinaria.Inventario;
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

namespace Core.Service.Maquinaria.Inventario
{
    public class SolicitudEquipoServices : ISolicitudEquipoDAO
    {
        #region Atributos
        private ISolicitudEquipoDAO m_SolicitudEquipoDAO;
        #endregion
        #region Propiedades
        public ISolicitudEquipoDAO SolicitudEquipoDAO
        {
            get { return m_SolicitudEquipoDAO; }
            set { m_SolicitudEquipoDAO = value; }
        }
        #endregion
        #region Constructores
        public SolicitudEquipoServices(ISolicitudEquipoDAO solicitudEquipoDAO)
        {
            this.SolicitudEquipoDAO = solicitudEquipoDAO;
        }
        #endregion


        public void Guardar(tblM_SolicitudEquipo obj)
        {
            SolicitudEquipoDAO.Guardar(obj);
        }
        public void GuardarJustificaciones(int solicitudID, List<tblM_SM_Justificacion> array)
        {
            SolicitudEquipoDAO.GuardarJustificaciones(solicitudID, array);
        }
        public void GuardarSolicitudEvidencia(int solicitudID, string dir)
        {
            SolicitudEquipoDAO.GuardarSolicitudEvidencia(solicitudID, dir);
        }

        public List<SolicitudEquipoJustificacionDTO> getListaJustificacionSolicitud(int idSolicitud)
        {
            return SolicitudEquipoDAO.getListaJustificacionSolicitud(idSolicitud);
        }
        public List<tblM_SolicitudEquipo> getListPendientes(int idUsuario, int tipo)
        {

            return SolicitudEquipoDAO.getListPendientes(idUsuario, tipo);
        }

        public string GetFolio(string cc)
        {
            return SolicitudEquipoDAO.GetFolio(cc);
        }

        public List<SolicitudEquipoDTO> getListaDetalleSolicitud(int idSolicitud)
        {
            return SolicitudEquipoDAO.getListaDetalleSolicitud(idSolicitud);
        }

        public List<tblM_SolicitudEquipo> getListAutorizadas(int idUsuario)
        {
            return SolicitudEquipoDAO.getListAutorizadas(idUsuario);
        }

        public tblM_SolicitudEquipo listaSolicitudEquipo(int idSolicitud)
        {
            return SolicitudEquipoDAO.listaSolicitudEquipo(idSolicitud);
        }

        public tblM_SolicitudEquipoDet GetSolicitudEquipoDet(int obj)
        {
            return SolicitudEquipoDAO.GetSolicitudEquipoDet(obj);
        }

        public List<SolicitudEquipoDTO> getListaDetalleSolicitudAutorizacion(int idSolicitud)
        {
            return SolicitudEquipoDAO.getListaDetalleSolicitudAutorizacion(idSolicitud);
        }

        public tblM_SolicitudEquipo LoadDataSolicitud(string folio)
        {
            return SolicitudEquipoDAO.LoadDataSolicitud(folio);
        }
        public List<SolicitudDetalleDTO> getListaSolicitudesPendientes(int idSolicitud)
        {
            return SolicitudEquipoDAO.getListaSolicitudesPendientes(idSolicitud);
        }

        public tblM_SolicitudEquipo loadSolicitudById(int id)
        {
            return SolicitudEquipoDAO.loadSolicitudById(id);
        }
        public List<tblP_Autoriza> GetAutorizadores(string cc)
        {
            return SolicitudEquipoDAO.GetAutorizadores(cc);
        }

        public List<tblM_SolicitudEquipo> GetListaSolicitudesAutorizadas(int idUsuario)
        {
            return SolicitudEquipoDAO.GetListaSolicitudesAutorizadas(idUsuario);
        }
        public List<SolicitudesDetalleVacantesDTO> GetSolicitudesEquipo(string CentroCostos)
        {
            return SolicitudEquipoDAO.GetSolicitudesEquipo(CentroCostos);

        }
        public List<DetalleVacantesSolicitudDTO> GetDetalleSolicitud(int solicitudes)
        {
            return SolicitudEquipoDAO.GetDetalleSolicitud(solicitudes);
        }

        public List<DetalleGrupoPlantillaDTO> GetDataSolicitudesGrupo(int solicitudes, int grupo)
        {
            return SolicitudEquipoDAO.GetDataSolicitudesGrupo(solicitudes, grupo);
        }

        public List<tblM_SolicitudEquipo> GetEquipoNoMovimientos()
        {
            return SolicitudEquipoDAO.GetEquipoNoMovimientos();
        }

        public List<tblM_AutorizacionSolicitudes> getTiemposAsignacion(List<string> CentroCostos)
        {
            return SolicitudEquipoDAO.getTiemposAsignacion(CentroCostos);
        }

        public List<EquiposPendientesDTO> getEquiposPendientes(string cc, int Tipo, int grupo)
        {
            return SolicitudEquipoDAO.getEquiposPendientes(cc, Tipo, grupo);
        }

        public List<EquiposPendientesReemplazoDTO> getEquiposReemplazo(string cc, int Tipo, int grupo)
        {
            return SolicitudEquipoDAO.getEquiposReemplazo(cc, Tipo, grupo);
        }
        public List<tblM_AutorizacionSolicitudes> GetListSolicitudesCC(List<string> centro_costos)
        {
            return SolicitudEquipoDAO.GetListSolicitudesCC(centro_costos);
        }


        public string obtenerComentarioSolicitud(int solicitudID)
        {
            return SolicitudEquipoDAO.obtenerComentarioSolicitud(solicitudID);
        }
        //Funciones Envio Doc Gestor
        public bool insertEnvioGestor(int solicitudID)
        {
            return SolicitudEquipoDAO.insertEnvioGestor(solicitudID);
        }
        public tblM_SolicitudEquipo listaSolicitudEquipoPorEmpresa(int idSolicitud, int empresa)
        {
            return SolicitudEquipoDAO.listaSolicitudEquipoPorEmpresa(idSolicitud, empresa);
        }
        public List<SolicitudEquipoDTO> getListaDetalleSolicitudAutorizacionPorEmpresa(int idSolicitud, int empresa)        
        {
            return SolicitudEquipoDAO.getListaDetalleSolicitudAutorizacionPorEmpresa(idSolicitud, empresa);
        }
        public tblM_AutorizacionSolicitudes getAutorizadoresPorEmpresa(int idSolicitud, int empresa)        
        {
            return SolicitudEquipoDAO.getAutorizadoresPorEmpresa(idSolicitud, empresa);
        }
        public List<tblM_SolicitudEquipoDet> listaDetalleSolicitudPorEmpresa(int obj, int empresa)       
        {
            return SolicitudEquipoDAO.listaDetalleSolicitudPorEmpresa(obj, empresa);
        }
        public List<tblM_AsignacionEquipos> getAsignacionesByIDPorEmpresa(int id, int empresa)        
        {
            return SolicitudEquipoDAO.getAsignacionesByIDPorEmpresa(id, empresa);
        }

        public Dictionary<string, object> getAsginaciones(int economicoID)
        {
            return SolicitudEquipoDAO.getAsginaciones(economicoID);
        }
        public Dictionary<string, object> GetAutorizadoresAC()
        {
            return SolicitudEquipoDAO.GetAutorizadoresAC();
        }
        public void SetAutorizadorAC(int usarioID, string ac, int perfil)
        {
            SolicitudEquipoDAO.SetAutorizadorAC(usarioID,ac,perfil);
        }
       
    }
}
