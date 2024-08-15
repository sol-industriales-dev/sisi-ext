using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class OrdenCompraRetencionesDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public int id_cpto { get; set; }
        public string descRet { get; set; }
        public int orden { get; set; }
        public decimal cantidad { get; set; }
        public decimal porc_ret { get; set; }
        public decimal importe { get; set; }
        public decimal facturado { get; set; }
        public decimal retenido { get; set; }
        public int aplica { get; set; }
        public int forma_pago { get; set; }
        public decimal? tm_descto { get; set; }
        public decimal calc_iva { get; set; }
        public string bit_afecta_oc { get; set; }
        public string afecta_fac { get; set; }
        public decimal facturado_ret { get; set; }
        public decimal facturado_iva { get; set; }
        public decimal facturado_total { get; set; }
        public decimal retenido_ret { get; set; }
        public decimal retenido_iva { get; set; }
        public decimal retenido_total { get; set; }
    }
}
