using Core.DAO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Propuesta
{
    public class EstimacioProveedorServices : IEstimacioProveedorDAO
    {
        #region Atributos
        private IEstimacioProveedorDAO p_estProvDAO;
        #endregion
        #region Propiedades
        public IEstimacioProveedorDAO EstProvDAO 
        {
            get { return p_estProvDAO; }
            set { p_estProvDAO = value; }
        }
        #endregion
        #region Constructores
        public EstimacioProveedorServices(IEstimacioProveedorDAO estProvDAO)
        {
            EstProvDAO = estProvDAO;
        }
        #endregion
        public bool guardarEstProv(tblC_EstimacionProveedor estProv)
        {
            return EstProvDAO.guardarEstProv(estProv);
        }
        public List<tblC_EstimacionProveedor> getLstEstProv(DateTime min, DateTime max)
        {
            return EstProvDAO.getLstEstProv(min, max);
        }
    }
}
