using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_OCRetenciones_Interna
    {
        public int id { get; set; }
        public decimal cantidad { get; set; }
        public decimal porcentajeRetencion { get; set; }
        public decimal importe { get; set; }
        public decimal facturado { get; set; }
        public decimal retenido { get; set; }
        public bool aplica { get; set; }

        public int ordenCompra_id { get; set; }
        public virtual tblCom_OrdenCompra_Interna ordenCompra { get; set; }

        public int retencion_id { get; set; }
        public virtual tblCom_Retenciones_Interna retencion { get; set; }
    }
}
