using Core.DAO.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class TipoMantenimientoServices : ITiposMantenimientosDAO
    {
        #region Atributos
        private ITiposMantenimientosDAO m_tipoMantenimientoDAO;
        #endregion
        #region Propiedades
        public ITiposMantenimientosDAO TipoMantenimientoDAO
        {
            get { return m_tipoMantenimientoDAO; }
            set { m_tipoMantenimientoDAO = value; }
        }
        #endregion
        #region Constructores
        public TipoMantenimientoServices(ITiposMantenimientosDAO tipoMantenimiento)
        {
            this.TipoMantenimientoDAO = tipoMantenimiento;
        }
        #endregion
    }
}
