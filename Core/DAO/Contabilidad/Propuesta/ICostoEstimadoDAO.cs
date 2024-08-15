using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface ICostoEstimadoDAO
    {
        bool guardarLstCostoEstimado(List<tblC_CostoEstimado> lst);
        List<tblC_CostoEstimado> getLstCostoEstimado(DateTime fecha);
    }
}
