using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface IEstimacionCobranzaDAO
    {
        bool guardarEstimacionCobro(List<tblC_EstimacionCobranza> lst);
        List<tblC_EstimacionCobranza> getLstEstimacionCobranza(DateTime fecha);
    }
}
