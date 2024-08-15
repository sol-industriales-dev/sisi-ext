using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class TerminacionObraDAO : GenericDAO<tblPro_CierreObra>, ITerminacionObraDAO
    {


        public void Guardar(tblPro_CierreObra obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.TERMINACIONOBRA);
            else
                Update(obj, obj.id, (int)BitacoraEnum.TERMINACIONOBRA);
        }

        public List<tblPro_CierreObra> GetObrasCerradasByID(int capaturaObrasID, int registroID)
        {
            return _context.tblPro_CierreObra.Where(x => x.capturadeObrasID == capaturaObrasID && x.registroID == registroID).ToList();
        }

        public List<tblPro_CierreObra> GetObrasCerradas()
        {
            return _context.tblPro_CierreObra.ToList();
        }

    }
}
