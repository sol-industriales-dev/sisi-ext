using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class StandbyServices : IStandByDAO
    {
        #region Atributos
        private IStandByDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IStandByDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public StandbyServices(IStandByDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void GuardarStandBy(tblM_CapStandBy obj)
        {
            interfazDAO.GuardarStandBy(obj);

        }
        public List<tblM_CapStandBy> getListaStandBy(List<string> listCC, DateTime fechainicio, DateTime fechaFin, int filtro)
        {
            return interfazDAO.getListaStandBy(listCC, fechainicio, fechaFin, filtro);
        }
        public List<StandbyGridDTO> GetListMaquinaria(List<string> listCC, DateTime fechaInicio, DateTime fechaFin)
        {
            return interfazDAO.GetListMaquinaria(listCC, fechaInicio, fechaFin);
        }

        public tblM_CapStandBy getStandByID(int id)
        {
            return interfazDAO.getStandByID(id);
        }
    }
}
