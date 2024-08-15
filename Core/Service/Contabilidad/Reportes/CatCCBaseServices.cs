using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class CatCCBaseServices : ICatCCBaseDAO
    {
        #region Atributo
        public ICatCCBaseDAO c_base { get; set; }
        #endregion
        #region Propiedad
        public ICatCCBaseDAO Base
        {
            get { return c_base; }
            set { c_base = value; }
        }
        #endregion
        #region Constructor
        public CatCCBaseServices(ICatCCBaseDAO CCbase)
        {
            this.Base = CCbase;
        }
        #endregion
        public void MigrarBaseHastaCP2017()
        {
            this.Base.MigrarBaseHastaCP2017();
        }
        public List<tblC_CatCCBase> getHistorico()
        {
            return this.Base.getHistorico();
        }
    }
}
