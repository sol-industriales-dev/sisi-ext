using Core.DAO.Contabilidad.Reportes;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class CatGiroProvServices : ICatGiroProvDAO
    {
        #region Atributos
        private ICatGiroProvDAO c_giroDAO;
        #endregion
        #region Propiedades
        public ICatGiroProvDAO CatGiroDAO
        {
            get { return c_giroDAO; }
            set { c_giroDAO = value; }
        }
        #endregion
        #region Constructores
        public CatGiroProvServices(ICatGiroProvDAO catGiroDAO)
        {
            CatGiroDAO = catGiroDAO;
        }
        #endregion
        public bool saveGiro(tblC_CatGiro giro)
        {
            return CatGiroDAO.saveGiro(giro);
        }
        public List<tblC_CatGiro> getAllGiro()
        {
            return CatGiroDAO.getAllGiro();
        }
        public List<tblC_CatGiro> getLstGiro()
        {
            return CatGiroDAO.getLstGiro();
        }
        public List<ComboDTO> getCboGiro()
        {
            return CatGiroDAO.getCboGiro();
        }
    }
}
