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
    public class RitmoHorometroDAO : GenericDAO<tblM_CapRitmoHorometro>, IRitmoHorometroDAO
    {
        public void GuardarRitmo(tblM_CapRitmoHorometro obj)
        {
            if (true)
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.RITMOHOROMETRO);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.RITMOHOROMETRO);
            }
            else
            {
                throw new Exception("Error al asignar ritmo a la maquina");
            }
        }

        public tblM_CapRitmoHorometro CapRitmoHorometro(string obj)
        {
            var result = (from r in _context.tblM_CapRitmoHorometro
                          where r.economico.Equals(obj)
                          select r);
            return result.FirstOrDefault();
        }

      

    }
}
