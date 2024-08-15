using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Facturacion.Enkontrol
{
    public interface IFacturasSPDAO
    {

        Dictionary<string, object> GuardarPrefactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto);


        #region PEDIDOS
        Dictionary<string, object> GuardarPedidos(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido);
        
        #endregion

        #region REMISION
        Dictionary<string, object> GuardarRemision(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision);
		 
	    #endregion

        #region FACTURA
        Dictionary<string, object> GuardarFactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision, int idSPFactura);
        
        #endregion

    }
}
