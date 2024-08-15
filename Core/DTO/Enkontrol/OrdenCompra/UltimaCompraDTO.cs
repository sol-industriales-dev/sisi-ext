using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class UltimaCompraDTO
    {
        public string proveedorNum { get; set; }
        public string proveedorNom { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public string folioOC { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public decimal precio { get; set; }
        public int moneda { get; set; }
        public string monedaDesc { get; set; }
    }
}
