using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("COMGUICAB")]
    public class COMGUICAB
    {
        [Key, Column(Order = 0)]
        public string CCTD { get; set; }
        [Key, Column(Order = 1)]
        public string CCNUMSER { get; set; }
        [Key, Column(Order = 2)]
        public string CCNUMDOC { get; set; }
        [Key, Column(Order = 3)]
        public string CCCODPRO { get; set; }
        public DateTime CCFECDOC { get; set; }
        public DateTime? CCFECVEN { get; set; }
        public string CCORDCOM { get; set; }
        public string CCRUC { get; set; }
        public string CCALMA { get; set; }
        public decimal CCIMPORTE { get; set; }
        public decimal CCSALDO { get; set; }
        public decimal CCTIPCAM { get; set; }
        public string CCCODMON { get; set; }
        public string CCRFTD { get; set; }
        public string CCRFNUMSER { get; set; }
        public string CCRFNUMDOC { get; set; }
        public DateTime? CCFECCRE { get; set; }
        public DateTime? CCFECMOD { get; set; }
        public string CCESTADO { get; set; }
        public string CCUSER { get; set; }
        public string CCGLOSA { get; set; }
        public decimal CCIGV { get; set; }
        public string CCRESPONSABLE { get; set; }
        public string CCBITSERVICIO { get; set; }
        public string CCORDFAB { get; set; }
        public int CCNROFACTURA { get; set; }
        public string COD_AUDITORIA { get; set; }
    }
}
