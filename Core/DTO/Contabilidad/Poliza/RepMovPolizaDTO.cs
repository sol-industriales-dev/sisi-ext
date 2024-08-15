using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public  class RepMovPolizaDTO
    {
        public DateTime fechapol { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public string generada { get; set; }
        public string status { get; set; }
        public int? usuario_movto { get; set; }
        public DateTime? fec_hora_movto { get; set; }
        public int usuario_crea { get; set; }
        public DateTime fecha_hora_crea { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public int digito { get; set; }
        public string cc { get; set; }
        public string referencia { get; set; }
        public string orden_compra { get; set; }
        public string concepto { get; set; }
        public string forma_pago { get; set; }
        public int tm { get; set; }
        public int itm { get; set; }
        public decimal monto { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public string error { get; set; }
        public string usuarioCreacion { get; set; }
        public string usuarioModificacion { get; set; }
    }
}
