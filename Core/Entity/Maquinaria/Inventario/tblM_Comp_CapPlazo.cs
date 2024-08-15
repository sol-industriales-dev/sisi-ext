using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_Comp_CapPlazo
    {

        public int id { get; set; }
        public int financieroID { get; set; }
        public int tipoOperacion { get; set; }
        public decimal opcionCompra { get; set; }
        public decimal enganche { get; set; }
        public decimal depositoPorcentaje { get; set; }
        public decimal depositoMoneda { get; set; }
        public int moneda { get; set; }
        public int plazo { get; set; }
        public decimal tasaInteres { get; set; }
        public decimal gastosFijos { get; set; }
        public decimal comision { get; set; }
        public int rentasGarantia { get; set; }
        public decimal crecimientoPagos { get; set; }
        public int estado { get; set; }
        public int usuarioRegistra { get; set; }
        public DateTime fechaRegistro { get; set; }

        public virtual tblM_Comp_CatFinanciero financiero { get; set; }
    }
}
