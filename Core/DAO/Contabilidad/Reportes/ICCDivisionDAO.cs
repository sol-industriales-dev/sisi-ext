using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICCDivisionDAO
    {
        bool Guardar(tblC_CCDivision obj);
        bool Guardar(List<tblC_RelCuentaDivision> lst);
        List<tblC_RelCuentaDivision> getLstRelCtaDiv();
    }
}
