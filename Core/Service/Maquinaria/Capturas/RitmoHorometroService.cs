using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class RitmoHorometroService : IRitmoHorometroDAO
    {
                #region Atributos
        private IRitmoHorometroDAO m_RitmoHorometroDAO;
        #endregion
        #region Propiedades
        public IRitmoHorometroDAO RitmoHorometroDAO
        {
            get { return m_RitmoHorometroDAO; }
            set { m_RitmoHorometroDAO = value; }
        }
        #endregion
        #region Constructores
        #endregion
        public RitmoHorometroService(IRitmoHorometroDAO ritmoHorometroDAO)
        {
            this.RitmoHorometroDAO = ritmoHorometroDAO;
        }
        public void GuardarRitmo(tblM_CapRitmoHorometro obj)
        {
            RitmoHorometroDAO.GuardarRitmo(obj);
        }

        public tblM_CapRitmoHorometro CapRitmoHorometro(string obj)
        {
            return RitmoHorometroDAO.CapRitmoHorometro(obj);
        }
    }
}
