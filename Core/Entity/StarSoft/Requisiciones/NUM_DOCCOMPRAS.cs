using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    
    public class NUM_DOCCOMPRAS
    {
        [Key]
        public string CTNCODIGO { get; set; }
        public string CTDESCRIP { get; set; }
        public double CTNNUMINI { get; set; }
        public double CTNNUMFIN { get; set; }
        public double CTNNUMERO { get; set; }
        public string CTNUSER { get; set; }
        public string CTNFORMATO { get; set; }
        public string CTNLINEAS { get; set; }
        public string CTNFECHA { get; set; }
        public string CTNRUC { get; set; }
        public string CTNRAZSOCI { get; set; }
        public string CTNDIRECCI { get; set; }
        public string CTNORDCOMP { get; set; }
        public string CTNCOTIZAC { get; set; }
        public string CTNPEDIDO { get; set; }
        public string CTNALMACEN { get; set; }
        public string CTNMODARTI { get; set; }
        public string CTESTADO { get; set; }
        public string CTMUEVSTOCK { get; set; }
        public string CTALMA { get; set; }
        public string CTPTO { get; set; }
        public string CTSERNUM { get; set; }
        public string CTCAMBIO { get; set; }
        public string CTSTOCK { get; set; }
        public string CTIMPRESORA { get; set; }
        public string CTCONTROLADOR { get; set; }
        public string CTPUERTO { get; set; }
        public bool DIGITALIZA_LOGO { get; set; }
        public byte[] ARCHIVO_LOGO { get; set; }
        public string NOMBRE_ARCHIVO_LOGO { get; set; }
    }
}
