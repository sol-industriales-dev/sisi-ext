using Core.DAO.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Mantenimiento
{
    public class PMComponenteFiltroService : IPMComponenteFiltroDAO
    {
        private IPMComponenteFiltroDAO PMComponenteFiltroDAO { get; set; }

        public PMComponenteFiltroService(IPMComponenteFiltroDAO pmComponenteFiltroDAO)
        {
            this.PMComponenteFiltroDAO = pmComponenteFiltroDAO;
        }
        public bool SaveOrUpdateAgrupacionComponenteFiltro(tblM_PMComponenteFiltro obj)
        {
            return PMComponenteFiltroDAO.SaveOrUpdateAgrupacionComponenteFiltro(obj);
        }
        public List<tblM_PMComponenteFiltro> FillTblComponenteFiltro(int modeloID)
        {
            return PMComponenteFiltroDAO.FillTblComponenteFiltro(modeloID);
        }
        public bool DeleteComponenteFiltro(tblM_PMComponenteFiltro obj)
        {
            return PMComponenteFiltroDAO.DeleteComponenteFiltro(obj);
        }

    }
}
