using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface ISaldoConciliadoDAO
    {
        bool setConciliacion(List<ConcentradoDTO> lst);
        List<tblC_SaldoConciliado> getLstSaldosConciliados(BusqConcentradoDTO busq);
        List<tblC_SaldoConciliado> getLstSaldosConciliadosAnterior(BusqConcentradoDTO busq);
    }
}
