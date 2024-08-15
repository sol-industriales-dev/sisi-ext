using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using Core.Entity.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Mantenimiento
{
    public interface IPMComponenteLubricanteDAO
    {
        bool DeleteComponenteLubricante(tblM_PMComponenteLubricante obj);
        bool SaveOrUpdateComponenteLubricante(tblM_PMComponenteLubricante obj);

        List<tblM_PMComponenteLubricante> getTblComponentesLubricantes(int modeloID);
        List<tblM_CatSuministros> FillCboCatLubricantes();
        List<setLubricantesAlta> tblComponenteLubricante(int modeloID);

    }
}
