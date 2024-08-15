using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Almacen
{
    [Table("COMGUIDET")]
    public class COMGUIDET
    {
        [Key, Column(Order = 0)]
        public string DCTD { get; set; }
        [Key, Column(Order = 1)]
        public string DCNUMSER { get; set; }
        [Key, Column(Order = 2)]
        public string DCNUMDOC { get; set; }
        [Key, Column(Order = 3)]
        public string DCCODPRO { get; set; }
        [Key, Column(Order = 4)]
        public int DCITEM { get; set; }
        public string DCALMA { get; set; }
        public string DCCODIGO { get; set; }
        public decimal DCCANTID { get; set; }
        public decimal DCPRECIO { get; set; }
        public decimal DCIGV { get; set; }
        public decimal DCIGVPOR { get; set; }
        public decimal DCIMPUS { get; set; }
        public decimal DCIMPMN { get; set; }
        public string DCUNIDAD { get; set; }
        public string DCSERIE { get; set; }
        public string DCDESCRI { get; set; }
        public string DCLOTE { get; set; }
        public string DCDESCUENTO { get; set; }
        public decimal DCCANBRUTA { get; set; }
        public decimal DCIMPMNBRUTO { get; set; }
        public decimal DCIMPUSBRUTO { get; set; }
        public decimal DCPORDESCT { get; set; }
        public string DCCUENTA { get; set; }
        public string DCCENTCOST { get; set; }
        public string DCORDFAB { get; set; }
        public decimal DCDESCTIMP { get; set; }
        public string CCRFNUMDOC { get; set; }
    }
}
