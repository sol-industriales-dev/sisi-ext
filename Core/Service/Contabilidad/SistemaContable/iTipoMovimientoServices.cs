using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.iTipoMovimiento;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.SistemaContable
{
    public class iTipoMovimientoServices : IiTipoMovimientoDAO
    {
        #region Atributos
        private IiTipoMovimientoDAO c_iTmDAO;
        #endregion
        #region Propiedades
        public IiTipoMovimientoDAO CiTmDAO
        {
            get { return c_iTmDAO; }
            set { c_iTmDAO = value; }
        }
        #endregion
        #region Constructor
        public iTipoMovimientoServices(IiTipoMovimientoDAO ciTm)
        {
            CiTmDAO = ciTm;
        }
        #endregion
        #region Guardar
        public bool saveRelitm(List<tblC_TM_Relitm> lst)
        {
            return CiTmDAO.saveRelitm(lst);
        }
        #endregion
        #region Relacion iTipo Movimiento
        public List<tblC_TM_Relitm> ReliTmEmpresas(BusqAsignacionCuenta busq)
        {
            return CiTmDAO.ReliTmEmpresas(busq);
        }
        public List<iTmEmpresaDTO> ITipoMovimientoEmpresa(List<string> iSistemas)
        {
            return CiTmDAO.ITipoMovimientoEmpresa(iSistemas);
        }
        #endregion
    }
}
