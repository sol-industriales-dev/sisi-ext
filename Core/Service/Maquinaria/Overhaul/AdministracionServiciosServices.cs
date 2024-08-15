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
using Core.DTO.Maquinaria.Catalogos;

namespace Core.Service.Maquinaria.Overhaul
{
    public class AdministracionServiciosServices : IAdministracionServiciosDAO
    {
                #region Atributos
        private IAdministracionServiciosDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IAdministracionServiciosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public AdministracionServiciosServices(IAdministracionServiciosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public bool GuardarTipo(tblM_CatTipoServicioOverhaul obj) {
            return interfazDAO.GuardarTipo(obj);
        }
        public List<tblM_CatTipoServicioOverhaul> CargarTipoServicios(string nombre, bool estatus, int grupo, int modelo)
        {
            return interfazDAO.CargarTipoServicios(nombre, estatus, grupo, modelo);
        }
        public string getModeloByID(int id)
        {
            return interfazDAO.getModeloByID(id);
        }
        public List<tblM_CatMaquina> getMaquinasByModeloID(int modeloID)
        {
            return interfazDAO.getMaquinasByModeloID(modeloID);
        }
        public bool Guardar(tblM_CatServicioOverhaul obj)
        {
            return interfazDAO.Guardar(obj);
        }
        public List<ServiciosOverhaulActivosDTO> CargarServiciosActivos(string economico, string servicioNombre, string cc, int grupoMaquina, int modeloMaquina, bool estatus) 
        {
            return interfazDAO.CargarServiciosActivos(economico, servicioNombre, cc, grupoMaquina, modeloMaquina, estatus);
        }
        public List<tblM_CatTipoServicioOverhaul> getServiciosOverhaul(string term)
        {
            return interfazDAO.getServiciosOverhaul(term);
        }
        public List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>> CargarModalServiciosActivos(int idMaquina, string servicioNombre)
        {
            return interfazDAO.CargarModalServiciosActivos(idMaquina, servicioNombre);
        }
        public bool Aplicar(int id, int idMaquina, bool isPlaneacion, List<ModeloArchivoDTO> archivos, DateTime fecha)
        {
            return interfazDAO.Aplicar(id, idMaquina, isPlaneacion, archivos, fecha);
        }
        public bool DeshabilitarServicioOverhaul(int idServicio)
        {
            return interfazDAO.DeshabilitarServicioOverhaul(idServicio);
        }
        public bool Desasignar(int idServicio)
        {
            return interfazDAO.Desasignar(idServicio);
        }
        public List<tblM_trackServicioOverhaul> CargarHistorialServiciosActivos(int idServicio)
        {
            return interfazDAO.CargarHistorialServiciosActivos(idServicio);
        }
        public tblM_CatServicioOverhaul GetServicioByID(int index)
        {
            return interfazDAO.GetServicioByID(index);
        }
        public List<tblM_CatServicioOverhaul> GetServiciosByID(List<int> index)
        {
            return interfazDAO.GetServiciosByID(index);
        }
        public tblM_trackServicioOverhaul getTrackingByID(int trackID)
        {
            return interfazDAO.getTrackingByID(trackID);
        }
        public bool guardarModificacionesServicios(decimal cicloVidaHoras, int estatusNuevo, string economico, string servicioNombre, string cc, int grupoMaquina, int modeloMaquina, bool estatus)
        {
            return interfazDAO.guardarModificacionesServicios(cicloVidaHoras, estatusNuevo, economico, servicioNombre, cc, grupoMaquina, modeloMaquina, estatus);
        }
        public bool ActualizarArchivoTrack(int trackID, ModeloArchivoDTO archivos)
        {
            return interfazDAO.ActualizarArchivoTrack(trackID, archivos);
        }
        public List<ModeloArchivoDTO> CargarArchivosEvidencia(int id)
        {
            return interfazDAO.CargarArchivosEvidencia(id);
        }
    }
}