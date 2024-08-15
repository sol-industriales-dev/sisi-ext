using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ITerminacionObraDAO
    {
        void Guardar(tblPro_CierreObra obj);

        List<tblPro_CierreObra> GetObrasCerradasByID(int capaturaObrasID, int registroID);

        List<tblPro_CierreObra> GetObrasCerradas();

    }
}
