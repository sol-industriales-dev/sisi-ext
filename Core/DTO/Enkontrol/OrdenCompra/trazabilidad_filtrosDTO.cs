using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class trazabilidad_filtrosDTO
    {
        public string cc { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public int autorecepcionable { get; set; }
        public string tipo_surtido { get; set; }
        public int tiene_entrada { get; set; }
        public int tiene_factura { get; set; }
        public int tiene_contrarecibo { get; set; }
        public bool sisun { get; set; }
        public int req { get; set; }
        public int oc { get; set; }
        public int proveedor { get; set; }
    }
}
