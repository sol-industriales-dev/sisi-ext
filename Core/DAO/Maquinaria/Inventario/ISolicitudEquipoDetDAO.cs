using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface ISolicitudEquipoDetDAO
    {
        void Guardar(List<tblM_SolicitudEquipoDet> obj);

        void delete(int id);

        List<tblM_SolicitudEquipoDet> listaDetalleSolicitud(int obj);
        tblM_SolicitudEquipoDet DetSolicitud(int obj);
        void Guardar(tblM_SolicitudEquipoDet obj);

        List<tblM_SolicitudEquipo> ListaSolicitudes(string cc);


        tblM_SolicitudEquipo getSolicitudbyID(int obj);
    }
}
