using Core.DAO.Contabilidad.Propuesta;
using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Propuesta
{
    public class SaldoConciliadoServices : ISaldoConciliadoDAO
    {
        #region Atributos
        private ISaldoConciliadoDAO c_saldoDAO;
        #endregion
        #region Propiedades
        public ISaldoConciliadoDAO SaldoDAO
        {
            get { return c_saldoDAO; }
            set { c_saldoDAO = value; }
        }
        #endregion
        #region Constructores
        public SaldoConciliadoServices(ISaldoConciliadoDAO saldoDAO)
        {
            this.SaldoDAO = saldoDAO;
        }
        #endregion
        public bool setConciliacion(List<ConcentradoDTO> lst)
        {
            return SaldoDAO.setConciliacion(lst);
        }
        public List<tblC_SaldoConciliado> getLstSaldosConciliados(BusqConcentradoDTO busq)
        {
            return SaldoDAO.getLstSaldosConciliados(busq);
        }
        public List<tblC_SaldoConciliado> getLstSaldosConciliadosAnterior(BusqConcentradoDTO busq)
        {
            return SaldoDAO.getLstSaldosConciliadosAnterior(busq);
        }
    }
}
