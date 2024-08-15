using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class OrdenCompraDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int proveedorNumero { get; set; }
        public string proveedorNombre { get; set; }
        public string proveedorNombreCorto { get; set; }
        public int moneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal total { get; set; }
    }
}