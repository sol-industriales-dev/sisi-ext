using Core.DAO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Propuesta
{
    public class CostoEstimadoServices : ICostoEstimadoDAO
    {
        #region Atributos
        private ICostoEstimadoDAO p_costoDAO;
        #endregion
        #region Propiedades
        public ICostoEstimadoDAO CostoDAO {
            get { return p_costoDAO; }
            set { p_costoDAO = value; }
        }
        #endregion
        #region Constructores
        public CostoEstimadoServices(ICostoEstimadoDAO costoDAO)
        {
            CostoDAO = costoDAO;
        }
        #endregion
        public bool guardarLstCostoEstimado(List<tblC_CostoEstimado> lst)
        {
            return CostoDAO.guardarLstCostoEstimado(lst);
        }
        public List<tblC_CostoEstimado> getLstCostoEstimado(DateTime fecha)
        {
            return CostoDAO.getLstCostoEstimado(fecha);
        }
    }
}
