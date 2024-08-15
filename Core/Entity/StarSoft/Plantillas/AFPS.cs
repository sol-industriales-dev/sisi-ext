using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Plantillas
{
    [Table("AFPS")]
    public class AFPS
    {
        [Key]
        public string CODAFP { get; set; }
        public string NOMBRE { get; set; }
        public decimal APOROBLI { get; set; }
        public decimal SEGURO { get; set; }
        public decimal TOPESEGURO { get; set; }
        public decimal COMISIONRA { get; set; }
        public string CONTAAFP { get; set; }
        public decimal APOROBLIEMP { get; set; }
        public string CODIGO_TRANSFERENCIA { get; set; }
        public string CODIGO_TRANSFERENCIA_NET { get; set; }
        public string CODIGO_RTPS { get; set; }
        public string CTACONT_APORTOBLI { get; set; }
        public string CTACONT_SEGURO { get; set; }
        public string CTACONT_COMISION { get; set; }
        public decimal COMISION_FLUJO { get; set; }
        public decimal COMISION_SALDO { get; set; }
        public decimal APORTE_COMP_MINERIA { get; set; }
    }
}
