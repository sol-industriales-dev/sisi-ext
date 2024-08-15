
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Validacion
{
    public class OrdenCompraValidacionDTO
    {
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int numero { get; set; }
        public int proveedor { get; set; }
        public DateTime fecha { get; set; }
        public int moneda { get; set; }
        public decimal total { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total_rec { get; set; }
        public decimal total_fac { get; set; }
        public decimal total_fac_rec { get; set; }
        public decimal total_pag { get; set; }
        public decimal monto_disponible { get; set; }
        public int empresa { get; set; }
        public string rfcEmpresa { get; set; }
    }
}
