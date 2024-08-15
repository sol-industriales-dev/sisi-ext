using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.Obra;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.SistemaContable
{
    public interface IObraDAO
    {
        bool saveRelObras(List<tblC_CC_RelObras> lst);
        bool DeleteObra(tblC_CC_RelObras obra);
        List<tblC_CC_RelObras> RelObras(BusqAsignacionCuenta busq);
        List<CentroCostoEmpresaDTO> CatObraEmpresa();
    }
}
