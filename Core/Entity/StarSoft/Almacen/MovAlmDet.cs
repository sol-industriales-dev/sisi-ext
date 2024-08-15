using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("MovAlmDet")]
    public class MovAlmDet
    {
        [Key, Column(Order = 0)]
        public string DEALMA { get; set; }
        [Key, Column(Order = 1)]
        public string DETD { get; set; }
        [Key, Column(Order = 2)]
        public string DENUMDOC { get; set; }
        [Key, Column(Order = 3)]
        public int DEITEM { get; set; }
        public string DECODIGO { get; set; }
        public string DECODREF { get; set; }
        public decimal? DECANTID { get; set; }
        public decimal? DECANTENT { get; set; }
        public decimal? DECANREF { get; set; }
        public decimal? DECANFAC { get; set; }
        public string DEORDEN { get; set; }
        public decimal? DEPREUNI { get; set; }
        public decimal? DEPRECIO { get; set; }
        public decimal? DEPRECI1 { get; set; }
        public decimal? DEDESCTO { get; set; }
        public string DESTOCK { get; set; }
        public decimal? DEIGV { get; set; }
        public decimal? DEIMPMN { get; set; }
        public decimal? DEIMPUS { get; set; }
        public string DESERIE { get; set; }
        public string DESITUA { get; set; }
        public DateTime? DEFECDOC { get; set; }
        public string DECENCOS { get; set; }
        public string DERFALMA { get; set; }
        public string DETR { get; set; }
        public string DEESTADO { get; set; }
        public string DECODMOV { get; set; }
        public decimal? DEVALTOT { get; set; }
        public string DECOMPRO { get; set; }
        public string DECODMON { get; set; }
        public string DETIPO { get; set; }
        public decimal? DETIPCAM { get; set; }
        public decimal? DEPREVTA { get; set; }
        public string DEMONVTA { get; set; }
        public DateTime? DEFECVEN { get; set; }
        public decimal? DEDEVOL { get; set; }
        public string DESOLI { get; set; }
        public string DEDESCRI { get; set; }
        public decimal? DEPORDES { get; set; }
        public decimal? DEIGVPOR { get; set; }
        public decimal? DEDESCLI { get; set; }
        public decimal? DEDESESP { get; set; }
        public string DENUMFAC { get; set; }
        public string DELOTE { get; set; }
        public string DEUNIDAD { get; set; }
        public decimal? DECANTBRUTA { get; set; }
        public decimal? DEDSCTCANTBRUTA { get; set; }
        public string DEORDFAB { get; set; }
        public string DEQUIPO { get; set; }
        public decimal? DEFLETE { get; set; }
        public string DEITEMI { get; set; }
        public string DEGLOSA { get; set; }
        public bool DEVALORIZADO { get; set; }
        public string DESECUENORI { get; set; }
        public string DEREFERENCIA { get; set; }
        public string UMREFERENCIA { get; set; }
        public decimal? CANTREFERENCIA { get; set; }
        public string DECUENTA { get; set; }
        public string DETEXTO { get; set; }
        public string CTA_CONSUMO { get; set; }
        public string CODPARTE { get; set; }
        public string CODPLANO { get; set; }
        public int DETPRODUCCION { get; set; }
        public string MPMA { get; set; }
        public decimal? PorcentajeCosto { get; set; }
        public decimal? SALDO_NC { get; set; }
        public decimal? DEPRECIOREF { get; set; }
    }
}
