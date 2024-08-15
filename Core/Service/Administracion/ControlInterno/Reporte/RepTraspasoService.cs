using Core.DAO.Administracion.ControlInterno.Reporte;
using Core.DTO.Administracion.ControlInterno.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.ControlInterno.Reporte
{
    public class RepTraspasoService : IRepTrapasoDAO
    {
        #region Atributos
        private IRepTrapasoDAO m_RepTrapasoDAO;
        #endregion
        #region Propiedades
        public IRepTrapasoDAO RepTraspasoDAO
        {
            get { return m_RepTrapasoDAO; }
            set { m_RepTrapasoDAO = value; }
        }
        #endregion
        #region Constructores
        public RepTraspasoService(IRepTrapasoDAO repTrapasoDAO)
        {
            this.RepTraspasoDAO = repTrapasoDAO;
        }
        #endregion
        public List<RepTraspasoDTO> getLstMovCerrados(string cc, string folio, string almacen, DateTime fechaIni, DateTime fechaFin)
        {
           return this.RepTraspasoDAO.getLstMovCerrados(cc, folio, almacen,fechaIni,fechaFin);
        }
        public List<RepTraspasoDTO> getLstMovAbiertos(string cc, string folio, string almacen, DateTime fechaIni, DateTime fechaFin)
        {
            return this.RepTraspasoDAO.getLstMovAbiertos(cc, folio, almacen, fechaIni, fechaFin);
        }
    }
}
