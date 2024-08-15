using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    [NotMapped] 
    public class FE_MovPolDTO : tblC_FE_MovPol
    {
        public string fecha { get; set; }
        public string concepto { get; set; }
        public string folio { get; set; }
        public string ctaStr { get; set; }
        public string ctaDesc { get; set; }
        public string itmDesc { get; set; }
        public string orden_compra { get; set; }
        public int? numpro { get; set; }
        public string proveedor { get; set; }
    }
}
