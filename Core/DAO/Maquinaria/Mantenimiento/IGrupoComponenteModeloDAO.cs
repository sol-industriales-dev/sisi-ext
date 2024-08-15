using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Mantenimiento
{
    public interface IGrupoComponenteModeloDAO
    {
        bool SaveOrUpdateAgrupacionComponenteModelo(tblM_PMComponenteModelo obj);
        List<tblM_PMComponenteModelo> filltblAgrupacionComponenteModelo(int modeloID);
        bool DeleteComponetneModelo(tblM_PMComponenteModelo obj);
        tblM_PMComponenteModelo getPMComponenteModeloByID(int id);
    }
}
