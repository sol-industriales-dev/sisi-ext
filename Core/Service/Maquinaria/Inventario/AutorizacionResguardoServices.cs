using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AutorizacionResguardoServices : iAutorizacionResguardoDAO
    {
        #region Atributos
        private iAutorizacionResguardoDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private iAutorizacionResguardoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public AutorizacionResguardoServices(iAutorizacionResguardoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public void Guardar(tblM_AutorizacionResguardo obj)
        {
            interfazDAO.Guardar(obj);
        }
        public tblM_AutorizacionResguardo GetObjAutorizaciones(int obj)
        {
            return interfazDAO.GetObjAutorizaciones(obj);
        }
    }
}
