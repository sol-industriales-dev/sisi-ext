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

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IAdministracionServiciosDAO
    {
        bool GuardarTipo(tblM_CatTipoServicioOverhaul obj);
        List<tblM_CatTipoServicioOverhaul> CargarTipoServicios(string nombre, bool estatus, int grupo, int modelo);
        string getModeloByID(int id);
        List<tblM_CatMaquina> getMaquinasByModeloID(int modeloID);
        bool Guardar(tblM_CatServicioOverhaul obj);
        List<ServiciosOverhaulActivosDTO> CargarServiciosActivos(string economico, string servicioNombre, string cc, int grupoMaquina, int modeloMaquina, bool estatus);
        List<tblM_CatTipoServicioOverhaul> getServiciosOverhaul(string term);
        List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>> CargarModalServiciosActivos(int idMaquina, string servicioNombre);
        bool Aplicar(int id, int idMaquina, bool isPlaneacion, List<ModeloArchivoDTO> archivos, DateTime fecha);
        bool DeshabilitarServicioOverhaul(int idServicio);
        bool Desasignar(int idServicio);
        List<tblM_trackServicioOverhaul> CargarHistorialServiciosActivos(int idServicio);
        tblM_CatServicioOverhaul GetServicioByID(int index);
        List<tblM_CatServicioOverhaul> GetServiciosByID(List<int> index);

        tblM_trackServicioOverhaul getTrackingByID(int trackID);

        bool guardarModificacionesServicios(decimal cicloVidaHoras, int estatusNuevo, string economico, string servicioNombre, string cc, int grupoMaquina, int modeloMaquina, bool estatus);
        bool ActualizarArchivoTrack(int trackID, ModeloArchivoDTO archivos);
        List<ModeloArchivoDTO> CargarArchivosEvidencia(int id);
    }
}
