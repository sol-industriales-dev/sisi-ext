using Core.DAO.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Mantenimiento
{
    public class GrupoComponenteModeloService : IGrupoComponenteModeloDAO
    {
        private IGrupoComponenteModeloDAO grupoComponenteModeloDAO { get; set; }

        public GrupoComponenteModeloService(IGrupoComponenteModeloDAO grupoComponenteModeloDAO)
        {
            this.grupoComponenteModeloDAO = grupoComponenteModeloDAO;
        }

        public tblM_PMComponenteModelo getPMComponenteModeloByID(int id)
        {
            return grupoComponenteModeloDAO.getPMComponenteModeloByID(id);
        }

        public bool SaveOrUpdateAgrupacionComponenteModelo(tblM_PMComponenteModelo obj)
        {
            return grupoComponenteModeloDAO.SaveOrUpdateAgrupacionComponenteModelo(obj);
        }

        public List<tblM_PMComponenteModelo> filltblAgrupacionComponenteModelo(int modeloID)
        {
            return grupoComponenteModeloDAO.filltblAgrupacionComponenteModelo(modeloID);
        }

        public bool DeleteComponetneModelo(tblM_PMComponenteModelo obj)
        {
            return grupoComponenteModeloDAO.DeleteComponetneModelo(obj);
        }


    }
}
