using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.cotizaciones
{
    public class tblAD_Cotizaciones
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string cc { get; set; }
        public string cliente { get; set; }
        public string proyecto { get; set; }
        public decimal monto { get; set; }
        public int estatus { get; set; }
        public DateTime fechaStatus { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool activo { get; set; }
        public int comentariosCount { get; set; }
        public virtual List<tblAD_CotizacionComentarios> comentarios { get; set; }
        public decimal Margen { get; set; }
        public int revision { get; set; }
        public int year { get; set; }
        public int tipoMoneda { get; set; }

        public DateTime fechaProbableF { get; set; }
        public string contacto { get; set; }

    }
}
