using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.SistemaContable
{
    public class MesProcServices : IMesProcDAO
    {
        #region Atributos
        private IMesProcDAO c_MesDAO;
        #endregion
        #region Propiedades
        public IMesProcDAO CMesDAO 
        {
            get { return c_MesDAO; }
            set { c_MesDAO = value; } 
        }
        #endregion
        #region Contructor
        public MesProcServices(IMesProcDAO cMes)
        {
            CMesDAO = cMes;
        }
        #endregion
        public List<MesProcResumenDTO> getProcesosValidos(SistemasEnkontrolEnum sistema)
        {
            return c_MesDAO.getProcesosValidos(sistema);
        }
        public List<MesProcResumenDTO> getProcesosAbiertos(SistemasEnkontrolEnum sistema = SistemasEnkontrolEnum.General)
        {
            return c_MesDAO.getProcesosAbiertos(sistema);
        }
        public List<MesProcResumenDTO> getProcesosAbiertosPruebas(SistemasEnkontrolEnum sistema = SistemasEnkontrolEnum.General)
        {
            return c_MesDAO.getProcesosAbiertosPruebas(sistema);
        }
    }
}
