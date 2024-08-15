using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class PrecioDieselDAO : GenericDAO<tblM_CapPrecioDiesel>, IPrecioDieselDAO
    {
        public void Guardar(tblM_CapPrecioDiesel obj)
        {
            if (!Exists(obj.fecha))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.CAPTURACOMBUSTIBLE);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CAPTURACOMBUSTIBLE);
            }
            else
            {
                if (obj.id == 0)
                    throw new Exception("Ya se capturo el diesel hoy.");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.CAPTURACOMBUSTIBLE);
            }
        }
        public bool Exists(DateTime obj)
        {
            return _context.tblM_CapPrecioDiesel.Where(x =>
                                        x.fecha == obj).ToList().Count > 0 ? true : false;
        }
        public tblM_CapPrecioDiesel GetPrecioDiesel()
        {
            DateTime Today = DateTime.Now;
            var data = _context.tblM_CapPrecioDiesel.OrderByDescending(X => X.id);


            return data.FirstOrDefault();
        }
    }
}
