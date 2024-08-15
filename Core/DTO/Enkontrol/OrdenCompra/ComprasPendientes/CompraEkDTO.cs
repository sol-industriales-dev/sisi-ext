using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra.ComprasPendientes
{
    public class CompraEkDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int libre_abordo { get; set; }
        public int comprador { get; set; }
        public int proveedor { get; set; }
        public string st_impresa { get; set; }
        public string estatus { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public string ccNumero { get; set; }
    }
}
