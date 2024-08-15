using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra.ComprasPendientes
{
    public class CompraSPDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public int idLibreAbordo { get; set; }
        public int tiempoEntregaDias { get; set; }
        public int areaCompra { get; set; }
        public int cuentaCompra { get; set; }
    }
}
