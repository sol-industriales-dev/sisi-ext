using Core.DAO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Propuesta
{
    public class EstimacionCobranzaServices : IEstimacionCobranzaDAO
    {
        #region Atributos
        private IEstimacionCobranzaDAO p_estCob { get; set; }
        #endregion
        #region Propiedades
        public IEstimacionCobranzaDAO EstCobDAO 
        {
            get { return p_estCob; }
            set { p_estCob = value; }
        }
        #endregion
        #region Contructores
        public EstimacionCobranzaServices(IEstimacionCobranzaDAO estCobDAO)
        {
            EstCobDAO = estCobDAO;
        }
        #endregion
        public bool guardarEstimacionCobro(List<tblC_EstimacionCobranza> lst)
        {
            return EstCobDAO.guardarEstimacionCobro(lst);
        }
        public List<tblC_EstimacionCobranza> getLstEstimacionCobranza(DateTime fecha)
        {
            return EstCobDAO.getLstEstimacionCobranza(fecha);
        }
    }
}
