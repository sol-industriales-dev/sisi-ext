using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Infrastructure.Utils;

namespace Data.DAO.Maquinaria.Inventario
{
    public class SolicitudEquipoReemplazoDAO : GenericDAO<tblM_SolicitudReemplazoEquipo>, ISolicitudEquipoReemplazo
    {
        public void Guardar(tblM_SolicitudReemplazoEquipo obj)
        {
            if (true)
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.SolicitudReempazo);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.SolicitudReempazo);
            }
            else
            {
                Update(obj, obj.id, (int)BitacoraEnum.SolicitudReempazo);
            }

        }
        public tblM_SolicitudReemplazoEquipo GetSolicitudReemplazobyID(int idSolicitud)
        {
            return _context.tblM_SolicitudReemplazoEquipo.FirstOrDefault(x => x.id.Equals(idSolicitud));
        }

        public List<tblM_SolicitudReemplazoEquipo> ListaSolicitudesEquipoByCC(string CC)
        {
            return null;
        }

        public string GetFolio(string obj)
        {
            int result = _context.tblM_SolicitudReemplazoEquipo.Where(x => x.CC.Equals(obj)).Count();
            return (result + 1).ToString();
        }

    }
}
