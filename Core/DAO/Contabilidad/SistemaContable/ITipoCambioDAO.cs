using Core.DTO.Contabilidad.SistemaContable.TipoCambio;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.SistemaContable
{
    public interface ITipoCambioDAO
    {
        #region Guardar
        bool GuardarTipoCambio(tblC_SC_TipoCambio tipoCambio);
        #endregion
        #region Captura Tipo Cambio
        tblC_SC_TipoCambio TipoCambioDelDia();
        List<TipoCambioDTO> HistoricoTipoCambio();
        List<tblC_SC_CatMoneda> CatMoneda();
        List<tblC_SC_CatMoneda> CatMonedaEnkontrol();
        #endregion
    }
}
