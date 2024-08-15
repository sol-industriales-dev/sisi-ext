using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CuentasPorCobrar
{
    public class tblCXC_CuentasPorCobrar
    {
        public int id { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public string cc { get; set; }
        public int numcte { get; set; }
        public string nombreCliente { get; set; }
        public DateTime? fechaVencOrig { get; set; }
        public DateTime? fechaVenc { get; set; }
        public string factura { get; set; }
        public decimal total { get; set; }
        public decimal pronosticado { get; set; }
        public bool aplicaPropuesta { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
