using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class InsumosDTO
    {
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public decimal PRECIO_INSUMO { get; set; }
        public DateTime FE_PRECIO { get; set; }
        public int OBRA { get; set; }
        public int consecutivo_bit { get; set; }
    }
}
