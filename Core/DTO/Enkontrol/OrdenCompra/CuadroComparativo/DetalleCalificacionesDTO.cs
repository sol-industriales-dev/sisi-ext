using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo
{
    public class DetalleCalificacionesDTO
    {
        public int oc { get; set; }
        public int requisicion { get; set; }
        public string cc { get; set; }
        public string proveedor { get; set; }
        public int proveedorID { get; set; }
        public DateTime fecha { get; set; }
    }
}
