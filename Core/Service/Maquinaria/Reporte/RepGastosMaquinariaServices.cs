using Core.DAO.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte
{
    public class RepGastosMaquinariaServices
    {
        #region Atributos
            private IRepGastosMaquinariaDAO m_repGastosMaquinariaDAO;
            #endregion
        #region Propiedades
        public IRepGastosMaquinariaDAO RepGastosDAO
        {
            get { return m_repGastosMaquinariaDAO; }
            set { m_repGastosMaquinariaDAO = value; }
        }
        #endregion
        #region Constructores
        public RepGastosMaquinariaServices(IRepGastosMaquinariaDAO repGastosDAO)
        {
            this.RepGastosDAO = repGastosDAO;
        }
        #endregion
    }
}
