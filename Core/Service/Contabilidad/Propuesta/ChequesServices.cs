using Core.DAO.Contabilidad.Propuesta;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Propuesta
{
    public class ChequesServices : IChequesDAO
    {
        #region Atributos
        private IChequesDAO c_chequeDAO;
        #endregion
        #region Propiedades
        public IChequesDAO ChequeDAO 
        {
            get { return c_chequeDAO; }
            set { c_chequeDAO = value; }
        }
        #endregion
        #region Constructores
        public ChequesServices(IChequesDAO chequeDAO)
        {
            this.ChequeDAO = chequeDAO;
        }
        #endregion
        public List<ChequesDTO> getLstEdoCta(BusqConcentradoDTO busq)
        {
            return ChequeDAO.getLstEdoCta(busq);
        }
        public List<sbCuentaDTO> getConsultaConcentrado(BusqConsultaConcentradoDTO busq)
        {
            return ChequeDAO.getConsultaConcentrado(busq);
        }
        public List<sbCuentaDTO> getLstCuenta(List<string> lstCta)
        {
            return ChequeDAO.getLstCuenta(lstCta);
        }
        public List<ComboDTO> cboCuentaBanco()
        {
            return ChequeDAO.cboCuentaBanco();
        }
        public List<RptBanMovDTO> getLstBancosMov(BusqBancoMov busq)
        {
            return ChequeDAO.getLstBancosMov(busq);
        }
    }
}
