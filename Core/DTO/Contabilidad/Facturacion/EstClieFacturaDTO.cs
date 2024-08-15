using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Facturacion
{
    public class EstClieFacturaDTO
    {
        public int no { get; set; }
        public string numcte { get; set; }
        public string nombreCliente { get; set; }
        public string cc { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechavenc { get; set; }
        /// <summary>
        /// Este es lo que se mostrara en la tabla
        /// </summary>
        public string descripcion { get; set; }
        public decimal estimacion { get; set; }
        public decimal anticipo { get; set; }
        public decimal vencido { get; set; }
        public decimal pronostico { get; set; }
        public decimal cobrado { get; set; }
        /// <summary>
        /// Aún no se como se mostrará este, pero debe de mostrarse sin ser parte de la tabla
        /// </summary>
        public string linea { get; set; }
        public string grupo { get; set; }
        public string grupoCC { get; set; }
        public string clase { get; set; }
        public DateTime fechaFlujo { get; set; }
    }
}
