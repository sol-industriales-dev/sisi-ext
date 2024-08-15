using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.iTipoMovimiento;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.SistemaContable
{
    public interface IiTipoMovimientoDAO
    {
        bool saveRelitm(List<tblC_TM_Relitm> lst);
        List<tblC_TM_Relitm> ReliTmEmpresas(BusqAsignacionCuenta busq);
        List<iTmEmpresaDTO> ITipoMovimientoEmpresa(List<string> iSistemas);
    }
}
