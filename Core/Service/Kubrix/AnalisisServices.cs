using Core.DAO.Kubrix;
using Core.DTO.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Kubrix
{
    public class AnalisisServices : IAnalisisDAO
    {
        #region Atributos
        private IAnalisisDAO k_AnalisisDAO;
        #endregion
        #region Propiedades
        public IAnalisisDAO AnalisisDAO
        {
            get { return k_AnalisisDAO; }
            set { k_AnalisisDAO = value; }
        }
        #endregion
        #region Constructores
        public AnalisisServices(IAnalisisDAO analisisDAO)
        {
            this.AnalisisDAO = analisisDAO;
        }
        #endregion
        public List<ComboDTO> getCboDivision()
        {
            return this.AnalisisDAO.getCboDivision();
        }
    }
}
