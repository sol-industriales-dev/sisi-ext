using Core.DAO.Maquinaria.Mantenimiento;
using Core.Service.Maquinaria.Mantenimiento;
using Data.DAO.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Mantenimiento
{
    public class PMComponenteFiltroFactoryServices
    {
        public IPMComponenteFiltroDAO getPMComponenteFiltroFactoryServices()
        {
            return new PMComponenteFiltroService(new PMComponenteFiltroDAO());
        }
    }
}
