using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICatCCBaseDAO
    {
        void MigrarBaseHastaCP2017();
        List<tblC_CatCCBase> getHistorico();
    }
}
