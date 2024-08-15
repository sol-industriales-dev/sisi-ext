using Core.DAO.Maquinaria.Mantenimiento;
using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using Core.Entity.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Mantenimiento
{
    public class PMComponenteLubricanteService : IPMComponenteLubricanteDAO
    {
        private IPMComponenteLubricanteDAO PMComponenteLubricanteDAO { get; set; }

        public PMComponenteLubricanteService(IPMComponenteLubricanteDAO pmComponenteLubricanteDAO)
        {
            this.PMComponenteLubricanteDAO = pmComponenteLubricanteDAO;
        }

        public bool SaveOrUpdateComponenteLubricante(tblM_PMComponenteLubricante obj)
        {
            return PMComponenteLubricanteDAO.SaveOrUpdateComponenteLubricante(obj);
        }

        public List<tblM_PMComponenteLubricante> getTblComponentesLubricantes(int modeloID)
        {
            return PMComponenteLubricanteDAO.getTblComponentesLubricantes(modeloID);
        }

        public List<tblM_CatSuministros> FillCboCatLubricantes()
        {
            return PMComponenteLubricanteDAO.FillCboCatLubricantes();
        }

        public List<setLubricantesAlta> tblComponenteLubricante(int modeloID)
        {
            return PMComponenteLubricanteDAO.tblComponenteLubricante(modeloID);
        }

      public  bool DeleteComponenteLubricante(tblM_PMComponenteLubricante obj)
        {
            return PMComponenteLubricanteDAO.DeleteComponenteLubricante(obj);
        }

    }
}
