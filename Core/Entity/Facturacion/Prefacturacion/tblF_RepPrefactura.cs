using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Facturacion.Prefacturacion
{
    public class tblF_RepPrefactura
    {
        public int id { get; set; }
        public string Folio { get; set; }
        public int Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string CC { get; set; }
        public string MetodoPago { get; set; }
        public int? MetodoPagoSAT { get; set; }
        public string TipoMoneda { get; set; }
        public string Usocfdi { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string CP { get; set; }
        public string RFC { get; set; }
        public bool PosicionImporte { get; set; }
        public bool VerCC { get; set; }
        public bool VerMetodoPago { get; set; }
        public bool VerTipoMoneda { get; set; }
        public bool CalAuto { get; set; }
        public int? TM { get; set; }
        public string CondEntrega { get; set; }
        public string TipoFlete { get; set; }
        public int? ReqOC { get; set; } //Requisicion/OC
        public int? CondicionesPago { get; set; }
        public string TipoPedido { get; set; }
        public string Observaciones { get; set; }
        public decimal? TipoCambio { get; set; }
        public decimal? IVA { get; set; }

        //REMISION
        public int? RegimenFiscal { get; set; }
        public string Serie { get; set; }
        public string Entregado { get; set; }

        //FACTURA
        public string Cuenta { get; set; }
        public string TipoFactura { get; set; }
        //public bool esCredito { get; set; } 

        //CLIENTE 
        public int? numcte { get; set; }
    }
}
