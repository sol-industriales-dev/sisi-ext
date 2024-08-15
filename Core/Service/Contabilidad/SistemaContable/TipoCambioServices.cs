using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.TipoCambio;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.SistemaContable
{
    public class TipoCambioServices : ITipoCambioDAO
    {
        #region Atributos
        private ITipoCambioDAO sc_TipoCambioDAO;
        #endregion
        #region Propiedades
        public ITipoCambioDAO scTipoCambioDAO
        {
            get { return sc_TipoCambioDAO; }
            set {  sc_TipoCambioDAO = value;}
        }
        #endregion
        #region Constructor
        public TipoCambioServices(ITipoCambioDAO scTipoCambio)
        {
            scTipoCambioDAO = scTipoCambio;
        }
        #endregion
        #region Guardar
        public bool GuardarTipoCambio(tblC_SC_TipoCambio tipoCambio)
        {
            return scTipoCambioDAO.GuardarTipoCambio(tipoCambio);
        }
        #endregion
        #region Captura Tipo Cambios
        public tblC_SC_TipoCambio TipoCambioDelDia()
        {
            return scTipoCambioDAO.TipoCambioDelDia();
        }
        public List<TipoCambioDTO> HistoricoTipoCambio()
        {
            return scTipoCambioDAO.HistoricoTipoCambio();
        }
        public List<tblC_SC_CatMoneda> CatMoneda()
        {
            return sc_TipoCambioDAO.CatMoneda();
        }
        public List<tblC_SC_CatMoneda> CatMonedaEnkontrol()
        {
            return sc_TipoCambioDAO.CatMonedaEnkontrol();
        }
        #endregion
    }
}
