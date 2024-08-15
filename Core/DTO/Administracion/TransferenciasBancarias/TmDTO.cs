using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class TmDTO
    {
        public int tm { get; set; }
        public string descripcion { get; set; }
        public string afectacompra { get; set; }
        public int naturaleza { get; set; }
        public string pide_iva { get; set; }
        public string tp_factura { get; set; }
        public int tm_banco { get; set; }
        public int tm_pago { get; set; }
        public string valida_almacen { get; set; }
        public string valida_recibido_autorizar { get; set; }
    }
}
