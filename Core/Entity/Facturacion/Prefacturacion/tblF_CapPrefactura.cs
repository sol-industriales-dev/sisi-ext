using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Facturacion.Prefacturacion
{
    public class tblF_CapPrefactura
    {
        public int id { get; set; }
        public int idRepPrefactura { get; set; }
        public int Renglon { get; set; }
        public int Tipo { get; set; }
        public int? TipoInsumo { get; set; }
        public decimal Cantidad { get; set; }
        public string Unidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Importe { get; set; }
        public string Concepto { get; set; }
        public string cc { get; set; }
    }
}
