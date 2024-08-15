using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AutorizaStandbyServices : IAutorizaStandbyDAO
    {

        #region Atributos
        private IAutorizaStandbyDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IAutorizaStandbyDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public AutorizaStandbyServices(IAutorizaStandbyDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void Guardar(tblM_AutorizaStandby obj)
        {
            interfazDAO.Guardar(obj);
        }

        public int GetUsuarioValida(int id, string CC)
        {
            return interfazDAO.GetUsuarioValida(id, CC);
        }

        public tblM_AutorizaStandby getAutorizacionesbyStandbyID(int id)
        {
            return interfazDAO.getAutorizacionesbyStandbyID(id);
        }
    }
}

