using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    /// <summary>
    /// Criterios de calidad
    /// </summary>
    public class tblCom_CC_CatConfiabilidad
    {
        public int Id { get; set; }
        public int TipoRequisicion { get; set; }
        public decimal Precio { get; set; }
        public decimal TiempoEntrega { get; set; }
        public decimal CondicionPago { get; set; }
        public decimal LAB { get; set; }
        public decimal ConfiabilidadProveedor { get; set; }
        public decimal Calidad { get; set; }
        public decimal ServicioPostVenta { get; set; }
    }
}
