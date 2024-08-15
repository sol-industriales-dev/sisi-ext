using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IAutorizacionSolicitudReemplazoDAO
    {
        void Guardar(tblM_AutorizacionSolicitudReemplazo obj);

        tblM_AutorizacionSolicitudReemplazo GetAutorizacionReemplazoByID(int id);
        tblM_AutorizacionSolicitudReemplazo getAutorizadores(int id);
        List<tblM_SolicitudReemplazoEquipo> getListPendientes(int idUsuario, int tipo, string folio);
         tblM_AutorizacionSolicitudReemplazo GetAutorizacionReemplazoByIDReemplazo(int id);
         string obtenerComentarioSolicitudReemplazo(int solicitudID);
    }
}
