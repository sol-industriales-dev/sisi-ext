using Core.Entity.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Mantenimiento
{
    public interface IPMComponenteFiltroDAO
    {
        bool SaveOrUpdateAgrupacionComponenteFiltro(tblM_PMComponenteFiltro obj);
        List<tblM_PMComponenteFiltro> FillTblComponenteFiltro(int modeloID);
        ///  List<tblM_CatFiltroMant> FillCboCatFiltroMant();
        bool DeleteComponenteFiltro(tblM_PMComponenteFiltro obj);

    }
}
