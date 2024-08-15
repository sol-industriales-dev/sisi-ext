using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.Obra;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.SistemaContable
{
    public class ObraServices : IObraDAO
    {
        #region Atributos
        private IObraDAO c_ObraDAO;
        #endregion
        #region Propiedades
        public IObraDAO CObraDAO
        {
            get { return c_ObraDAO; }
            set { c_ObraDAO = value; }
        }
        #endregion
        #region Contructor
        public ObraServices(IObraDAO cObra)
        {
            CObraDAO = cObra;
        }
        #endregion
        public bool saveRelObras(List<tblC_CC_RelObras> lst)
        {
            return CObraDAO.saveRelObras(lst);
        }
        public bool DeleteObra(tblC_CC_RelObras obra)
        {
            return CObraDAO.DeleteObra(obra);
        }
        public List<tblC_CC_RelObras> RelObras(BusqAsignacionCuenta busq)
        {
            return CObraDAO.RelObras(busq);
        }
        public List<CentroCostoEmpresaDTO> CatObraEmpresa()
        {
            return CObraDAO.CatObraEmpresa();
        }
    }
}
