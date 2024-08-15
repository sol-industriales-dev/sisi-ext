using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface ISolicitudEquipoReemplazoDetDAO
    {
        void Guardar(List<tblM_SolicitudReemplazoDet> obj);
        void GuardarDetalleArchivos(tblM_SolicitudReemplazoDet obj, HttpPostedFileBase file);
        List<tblM_SolicitudReemplazoDet> GetSolicitudReemplazoDetByIdSolicitud(int id);
        List<tblM_SolicitudReemplazoEquipo> GetSolicitudesPendientes(int tipo);
        tblM_SolicitudReemplazoDet GetSolicitudReemplazoDetById(int id);
        void GuardarSingle(tblM_SolicitudReemplazoDet obj);
    }
}
