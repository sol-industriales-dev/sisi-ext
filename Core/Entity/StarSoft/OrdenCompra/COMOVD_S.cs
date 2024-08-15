using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.OrdenCompra
{
    [Table("COMOVD_S")]
    public class COMOVD_S
    {
        [Key, Column(Order = 0)]
        public string OC_CNUMORD { get; set; }
        public string OC_CCODPRO { get; set; }
        public DateTime OC_DFECDOC { get; set; }
        [Key, Column(Order = 1)]
        public string OC_CITEM { get; set; }
        [Key, Column(Order = 2)]
        public string OC_CODSERVICIO { get; set; }
        public string OC_CDESREF { get; set; }
        public decimal OC_CANT { get; set; }
        public string OC_GLOSA { get; set; }
        public decimal OC_NPREUNI { get; set; }
        public decimal OC_NDSCPOR { get; set; }
        public decimal OC_NDESCTO { get; set; }
        public decimal OC_NIGV { get; set; }
        public decimal OC_NIGVPOR { get; set; }
        public decimal OC_NPRENET { get; set; }
        public decimal OC_NTOTVEN { get; set; }
        public decimal OC_NTOTNET { get; set; }
        public string OC_COMENTA { get; set; }
        public string OC_CESTADO { get; set; }
        public string OC_FUNICOM { get; set; }
        public decimal OC_NRECIBI { get; set; }
        public string OC_CCOMEN1 { get; set; }
        public string OC_CCOMEN2 { get; set; }
        public decimal OC_NCANTEN { get; set; }
        public decimal OC_NCANSAL { get; set; }
        public string OC_CREQSER { get; set; }
        public string OC_DORDFAB { get; set; }
        public string OC_CUENTA { get; set; }
        public string CENTCOST { get; set; }
        public string OC_ITEMOS { get; set; }
        public decimal? REQITEM_REF { get; set; }
    }
}
