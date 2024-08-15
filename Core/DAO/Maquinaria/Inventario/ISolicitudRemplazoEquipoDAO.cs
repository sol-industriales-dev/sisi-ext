using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface ISolicitudEquipoReemplazo
    {
        void Guardar(tblM_SolicitudReemplazoEquipo obj);
        List<tblM_SolicitudReemplazoEquipo> ListaSolicitudesEquipoByCC(string CC);
        tblM_SolicitudReemplazoEquipo GetSolicitudReemplazobyID(int idSolicitud);
        string GetFolio(string obj);
    }
}
