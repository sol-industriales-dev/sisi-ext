using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("MAEART")]
    public class MAEART
    {
        [Key]
        public string ACODIGO { get; set; }
        public string ACODIGO2 { get; set; }
        public string ADESCRI { get; set; }
        public string ADESCRI2 { get; set; }
        public string AFAMILIA { get; set; }
        public string AMODELO { get; set; }
        public string AUNIDAD { get; set; }
        public string AGRUPO { get; set; }
        public string ACUENTA { get; set; }
        public string AFSERIE { get; set; }
        public decimal APRECIO { get; set; }
        public decimal ADESCTO { get; set; }
        public decimal APRECOM { get; set; }
        public string ACODPRO { get; set; }
        public DateTime AFECHA { get; set; }
        public string ACASILLERO { get; set; }
        public string AFSTOCK { get; set; }
        public string AUSER { get; set; }
        public string AESTADO { get; set; }
        public DateTime? AFECVEN { get; set; }
        public string ACODMON { get; set; }
        public string ATIPO { get; set; }
        public string ACOMENTA { get; set; }
        public string AFLOTE { get; set; }
        public string AFDECI { get; set; }
        public decimal AUNIART { get; set; }
        public decimal APESO { get; set; }
        public decimal AISCPOR { get; set; }
        public decimal AIGVPOR { get; set; }
        public DateTime? AHORA { get; set; }
        public string AFPRELIB { get; set; }
        public string AFRESTA { get; set; }
        public string AFUNIREF { get; set; }
        public string AUNIREF { get; set; }
        public decimal AFACREF { get; set; }
        public decimal APDIS { get; set; }
        public decimal APCOM { get; set; }
        public string ACODMONC { get; set; }
        public bool AFLAGIGV { get; set; }
        public string ACOLOR { get; set; }
        public string AMARCA { get; set; }
        public string AFOTO { get; set; }
        public string ATALLA { get; set; }
        public int CODAUTO { get; set; }
        public string AFLAGPERC { get; set; }
        public string COD_IMAGEN { get; set; }
        public string UMREFERENCIA { get; set; }
        public string AMODELO1 { get; set; }
        public string AORIGEN { get; set; }
        public bool TERCERO { get; set; }
        public string MAR_CODIGO { get; set; }
        public string ZON_CODIGO { get; set; }
        public DateTime? AULTFECCOMP { get; set; }
        public string AULTPROV { get; set; }
        public string AULTPAISPROV { get; set; }
        public string COD_ARANCEL { get; set; }
        public double? APREFOB { get; set; }
        public double? APRECIF { get; set; }
        public string AMONFOB { get; set; }
        public string AMONCIF { get; set; }
        public string CODIGO_SUNAT { get; set; }
        public string COD_AUDITORIA { get; set; }
        public bool? FLG_EXONERADO_IGV { get; set; }
        public bool? FLG_ECOMMERCE { get; set; }
        public bool? FLG_DETRACCION { get; set; }
        public string COD_DETRACCION { get; set; }
        //public string DEUNIDAD { get; set; }
    }
}
