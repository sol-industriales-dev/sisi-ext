using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class StandbyDetServices : IStandByDetDAO
    {
        #region Atributos
        private IStandByDetDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IStandByDetDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public StandbyDetServices(IStandByDetDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void GuardarStandByDet(List<standByDetDTO> obj, standByDTO standbyCliente, int StandByID)
        {
            interfazDAO.GuardarStandByDet(obj, standbyCliente, StandByID);

        }

        public List<tblM_DetStandby> getListaDetStandBy(int StandByID)
        {
            return interfazDAO.getListaDetStandBy(StandByID);

        }
        public void DeleteRow(tblM_DetStandby objDetSingle)
        {
            interfazDAO.DeleteRow(objDetSingle);
        }



    }
}
