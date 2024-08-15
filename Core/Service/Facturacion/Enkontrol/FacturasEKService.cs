using Core.DAO.Facturacion.Enkontrol;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Facturacion.Enkontrol
{
    public class FacturasEKService : IFacturasSPDAO
    {
        private IFacturasSPDAO facturasInterfaz;

        public FacturasEKService(IFacturasSPDAO facturasTemp)
        {
            this.facturasInterfaz = facturasTemp;
        }

        public Dictionary<string, object> GuardarPrefactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto)
        {
            return facturasInterfaz.GuardarPrefactura(obj, lst, lstImpuesto);
        }

        #region PEDIDOS
        public Dictionary<string, object> GuardarPedidos(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido)
        {
            return facturasInterfaz.GuardarPedidos(obj, lst, lstImpuesto, idSPPedido);

        }
        #endregion

        #region REMISION
        public Dictionary<string, object> GuardarRemision(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision)
        {
            return facturasInterfaz.GuardarRemision(obj, lst, lstImpuesto, idSPPedido, idSPRemision);

        }
        #endregion

        #region FACTURAS
        public Dictionary<string, object> GuardarFactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision, int idSPFactura)
        {
            return facturasInterfaz.GuardarFactura(obj, lst, lstImpuesto, idSPPedido, idSPRemision, idSPFactura);

        }
        #endregion
    }
}
