using Core.DAO.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento;
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
    public class PMComponenteFiltroDAO : GenericDAO<tblM_PMComponenteFiltro>, IPMComponenteFiltroDAO
    {

        public bool SaveOrUpdateAgrupacionComponenteFiltro(tblM_PMComponenteFiltro obj)
        {
            try
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.ComponenteFiltroAgrupacion);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.ComponenteFiltroAgrupacion);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<tblM_PMComponenteFiltro> FillTblComponenteFiltro(int modeloID)
        {
            var dataSet = (from cm in _context.tblM_PMComponenteModelo
                           join lc in _context.tblM_PMComponenteFiltro
                           on cm.componenteID equals lc.componenteID
                           where cm.modeloID == modeloID && lc.modeloID == modeloID
                           select lc).ToList();

            return dataSet;// _context.tblM_PMComponenteFiltro.Where(x => x.componenteID == componenteID).ToList();
        }
        public bool DeleteComponenteFiltro(tblM_PMComponenteFiltro obj)
        {
            try
            {
                if (obj.id != 0)
                {
                    tblM_PMComponenteFiltro entidad = _context.tblM_PMComponenteFiltro.FirstOrDefault(x => x.id.Equals(obj.id));
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

    }
}
