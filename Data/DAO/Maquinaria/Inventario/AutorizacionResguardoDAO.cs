using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class AutorizacionResguardoDAO : GenericDAO<tblM_AutorizacionResguardo>, iAutorizacionResguardoDAO
    {

        public void Guardar(tblM_AutorizacionResguardo obj)
        {

            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.AutorizacionResguardo);
            else
                Update(obj, obj.id, (int)BitacoraEnum.AutorizacionResguardo);

        }

        public tblM_AutorizacionResguardo GetObjAutorizaciones(int obj)
        {
            return _context.tblM_AutorizacionResguardo.FirstOrDefault(x => x.ResguardoVehiculoID.Equals(obj));
        }

        
    }
}
