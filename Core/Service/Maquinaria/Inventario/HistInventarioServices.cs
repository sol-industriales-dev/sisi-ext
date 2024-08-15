using Core.DAO.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class HistInventarioServices : IHistInventarioDAO
    {
        #region Atributos
        private IHistInventarioDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IHistInventarioDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public HistInventarioServices(IHistInventarioDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public string GetInfoHistorial(DateTime fecha)
        {
            return interfazDAO.GetInfoHistorial(fecha);
        }
    }
}
