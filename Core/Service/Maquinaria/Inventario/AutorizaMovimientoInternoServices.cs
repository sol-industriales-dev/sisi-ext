using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AutorizaMovimientoInternoServices : IAutorizaMovimientoInternoDAO
    {
                #region Atributos
        private IAutorizaMovimientoInternoDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IAutorizaMovimientoInternoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public AutorizaMovimientoInternoServices(IAutorizaMovimientoInternoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void GuardarActualizar(tblM_AutorizaMovimientoInterno obj, bool esUsuarioRecibe = false)
        {
            interfazDAO.GuardarActualizar(obj, esUsuarioRecibe);
        }

        public tblM_AutorizaMovimientoInterno GetAutorizadores(int id)
        {
            return interfazDAO.GetAutorizadores(id);
        }
        public int GetAutorizadores(int tipo, string cc)
        {
            return interfazDAO.GetAutorizadores(tipo, cc);
        }
    }
}
