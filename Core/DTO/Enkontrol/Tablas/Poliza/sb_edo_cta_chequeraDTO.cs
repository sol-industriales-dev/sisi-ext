using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.Poliza
{
    public class sb_edo_cta_chequeraDTO
    {
        public int cuenta { get; set; }
        public DateTime fecha_mov { get; set; }
        public int tm { get; set; }
        public int numero { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public decimal tc { get; set; }
        public string origen_mov { get; set; }
        public string generada { get; set; }
        public string st_consilia { get; set; }
        public int? num_consilia { get; set; }
        public string st_che { get; set; }
        public int? ref_che_inverso { get; set; }
        public int? ref_tm_inverso { get; set; }
        public string motivo_cancelado { get; set; }
        public int? iyear { get; set; }
        public int? imes { get; set; }
        public int? ipoliza { get; set; }
        public string itp { get; set; }
        public int? ilinea { get; set; }
        public int? banco { get; set; }
        public string tp { get; set; }
        public string desc { get; set; }
        public int? folio { get; set; }
        public string tipo_iva { get; set; }
        public decimal? porc_iva { get; set; }
        public decimal? monto_iva { get; set; }
        public int? folio_iva { get; set; }
        public DateTime? fecha_reten { get; set; }
        public DateTime? fecha_reten_fin { get; set; }
        public int? id_num_credito { get; set; }
        public string prototipo { get; set; }
        public int? consecutivo_minist { get; set; }
        public string acredita_iva { get; set; }
        public int? clave_sub_tm { get; set; }
        public int? folio_imp { get; set; }
        public int? linea_imp { get; set; }
    }
}
