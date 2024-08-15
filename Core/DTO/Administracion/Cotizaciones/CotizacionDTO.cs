using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cotizaciones
{
    public class CotizacionDTO
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string cc { get; set; }
        public string cliente { get; set; }
        public string proyecto { get; set; }
        public string monto { get; set; }
        public decimal vMonto { get; set; }
        public string estatus { get; set; }
        public int vEstatus { get; set; }
        public string Margen { get; set; }
        public string fechaStatus { get; set; }
        public string fechaCaptura { get; set; }
        public int comentariosCount { get; set; }
        public int noRevision { get; set; }
        public int tipoMoneda { get; set; }
        public string contacto { get; set; }
        public string fechaProbableF { get; set; }
        public string comentario { get; set; }
        public string nombreMoneda { get; set; }

    }
}
