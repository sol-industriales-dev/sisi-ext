using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class ParosDAO : GenericDAO<tblM_Paros>, IParos
    {
        public List<tblM_Paros> getParosMaquina(int obj)
        {
            List<tblM_Paros> res = new List<tblM_Paros>();

            if(_context.tblM_Paros.Where(x => x.id_maquina == obj).ToList().Count > 0)
            {
                res = _context.tblM_Paros.Where(x => x.id_maquina == obj).ToList();
            }
            else
            {
                res.Add(new tblM_Paros());
            }
            
            return res;
        }
    }
}
