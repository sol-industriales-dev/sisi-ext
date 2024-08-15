using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class RetencionInfoDTO
    {
        public int id_cpto { get; set; }
        public string desc_ret { get; set; }
        public string porc_cant { get; set; }
        public decimal porc_default { get; set; }
        public int aplica { get; set; }
        public int naturaleza_ret { get; set; }
        public int insumo { get; set; }
        public string cta { get; set; }
        public string scta { get; set; }
        public string sscta { get; set; }
        public int forma_pago { get; set; }
        public decimal? tm_pago { get; set; }
        public int calc_iva { get; set; }
        public decimal? tm_descto { get; set; }
        public string afecta_fac { get; set; }
        public string bit_afecta_oc { get; set; }
        public string bit_ins_neg { get; set; }
        public int? tipo_retencion { get; set; }
        public decimal importe { get; set; }
    }
}
