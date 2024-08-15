using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface IEstimacioProveedorDAO
    {
        bool guardarEstProv(tblC_EstimacionProveedor estProv);
        List<tblC_EstimacionProveedor> getLstEstProv(DateTime min, DateTime max);
    }
}
