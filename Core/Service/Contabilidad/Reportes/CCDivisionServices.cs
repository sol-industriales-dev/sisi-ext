using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class CCDivisionServices : ICCDivisionDAO
    {
        #region Atributos
        public ICCDivisionDAO c_CcDivisionDAO { get; set; }
        #endregion
        #region Propiedades
        public ICCDivisionDAO CcDivision
        {
            get { return c_CcDivisionDAO; }
            set { c_CcDivisionDAO = value; }
        }
        #endregion
        #region Constructor
        public CCDivisionServices(ICCDivisionDAO ccDivsion)
        {
            CcDivision = ccDivsion;
        }
        #endregion
        public bool Guardar(tblC_CCDivision obj)
        {
            return CcDivision.Guardar(obj);
        }
        public bool Guardar(List<tblC_RelCuentaDivision> lst)
        {
            return CcDivision.Guardar(lst);
        }
        public List<tblC_RelCuentaDivision> getLstRelCtaDiv()
        {
            return CcDivision.getLstRelCtaDiv();
        }
    }
}
