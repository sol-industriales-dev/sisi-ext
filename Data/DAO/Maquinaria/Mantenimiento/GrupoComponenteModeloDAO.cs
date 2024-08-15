using Core.DAO.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Mantenimiento
{
    public class GrupoComponenteModeloDAO : GenericDAO<tblM_PMComponenteModelo>, IGrupoComponenteModeloDAO
    {

        public bool SaveOrUpdateAgrupacionComponenteModelo(tblM_PMComponenteModelo obj)
        {
            try
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.ComponenteModeloAgrupacion);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.ComponenteModeloAgrupacion);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool DeleteComponetneModelo(tblM_PMComponenteModelo obj)
        {
            try
            {
                if (obj.id != 0)
                {
                    tblM_PMComponenteModelo entidad = _context.tblM_PMComponenteModelo.FirstOrDefault(x => x.id.Equals(obj.id));
                    Delete(entidad, (int)BitacoraEnum.ComponenteModeloAgrupacion);

                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public tblM_PMComponenteModelo getPMComponenteModeloByID(int id)
        {
            return _context.tblM_PMComponenteModelo.FirstOrDefault(x => x.id == id);
        }

        public List<tblM_PMComponenteModelo> filltblAgrupacionComponenteModelo(int modeloID)
        {
            var returnData = _context.tblM_PMComponenteModelo.Where(x => x.modeloID == modeloID).ToList();

            return returnData;
        }
    }
}
